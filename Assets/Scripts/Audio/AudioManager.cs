using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioMixer mixer;
        public bool debug;
        public List<AudioTrack> tracks;

        private Hashtable audioTable; // relationship between Audio Types (key) and audio tracks (value)
        private Hashtable jobTable; // relationship between Audio Types (key) and jobs (value) (Coroutine, IEnumerator)

        [Serializable]
        public class AudioObject
        {
            public AudioTypeEnum audioType;
            public AudioClip audioClip;
        }

        [Serializable]
        public class AudioTrack
        {
            public AudioSource source;
            public List<AudioObject> audio;
        }

        private class AudioJob
        {
            public AudioAction action;
            public AudioTypeEnum type;
            public float delay, fadeInTime, fadeOutTime;
            public bool fadeIn, fadeOut;

            public AudioJob(AudioAction _action, AudioTypeEnum _type, float _delay = 0f, bool _fadeIn = false, float _fadeInTime = 0f, bool _fadeOut = false, float _fadeOutTime = 0f)
            {
                action = _action;
                type = _type;
                delay = _delay;
                fadeIn = _fadeIn;
                fadeInTime = _fadeInTime;
                fadeOut = _fadeOut;
                fadeOutTime = _fadeOutTime;
            }
        }

        private enum AudioAction
        {
            START,
            STOP,
            RESTART
        }

        #region Unity Functions
        private void Start()
        {
            ChangeMixerVolume(PlayerPrefs.GetFloat("masterVolume", 1f));
        }

        public override void ExtraToDo()
        {
            Configure();
        }

        private void OnDisable()
        {
            Dispose();
        }
        #endregion

        #region Public Functions
        public void PlayAudio(AudioTypeEnum type, float delay = 0f, bool fadeIn = false, float fadeInTime = 0f, bool fadeOut = false, float fadeOutTime = 0f)
        {
            AddJob(new AudioJob(AudioAction.START, type, delay, fadeIn, fadeInTime, fadeOut, fadeOutTime));
        }

        public void StopAudio(AudioTypeEnum type, float delay = 0f, bool fadeIn = false, float fadeInTime = 0f, bool fadeOut = false, float fadeOutTime = 0f)
        {
            AddJob(new AudioJob(AudioAction.STOP, type, delay, fadeIn, fadeInTime, fadeOut, fadeOutTime));
        }

        public void RestartAudio(AudioTypeEnum type, float delay = 0f, bool fadeIn = false, float fadeInTime = 0f, bool fadeOut = false, float fadeOutTime = 0f)
        {
            AddJob(new AudioJob(AudioAction.RESTART, type, delay, fadeIn, fadeInTime, fadeOut, fadeOutTime));
        }

        public void ChangeMixerVolume(float sliderValue)
        {
            mixer.SetFloat("masterVolume", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("masterVolume", sliderValue);
        }
        #endregion

        #region Private Functions
        private void Configure()
        {
            audioTable = new Hashtable();
            jobTable = new Hashtable();
            GenerateAudioTable();
        }

        private void GenerateAudioTable()
        {
            foreach (AudioTrack track in tracks)
            {
                IEnumerable<AudioTypeEnum> audioTypeDistinct = track.audio.Select(x => x.audioType).Distinct();
                foreach (AudioTypeEnum at in audioTypeDistinct)
                {
                    audioTable.Add(at, track);
                    Log("Registering audio [" + at + "]");
                }
            }
        }

        private void AddJob(AudioJob job)
        {
            //remove conflicting jobs
            RemoveConflictingJobs(job.type);

            //start job
            IEnumerator jobRunner = RunAudioJob(job);
            jobTable.Add(job.type, jobRunner);
            StartCoroutine(jobRunner);
            Log("Starting job on [" + job.type + "] with operation [" + job.action + "]");
        }

        private void RemoveConflictingJobs(AudioTypeEnum type)
        {
            if (jobTable.ContainsKey(type))
            {
                RemoveJob(type);
            }

            AudioTypeEnum conflictAudio = AudioTypeEnum.None;
            foreach(DictionaryEntry entry in jobTable)
            {
                AudioTypeEnum audioType = (AudioTypeEnum)entry.Key;
                AudioTrack audioTrackInUse = (AudioTrack)audioTable[audioType];
                AudioTrack audioTrackNeeded = (AudioTrack)audioTable[type];

                if (audioTrackInUse.source == audioTrackNeeded.source)
                {
                    //Conflict
                    conflictAudio = audioType;
                }
            }

            if (conflictAudio != AudioTypeEnum.None)
            {
                RemoveJob(conflictAudio);
            }
        }

        private void RemoveJob(AudioTypeEnum type)
        {
            if (!jobTable.ContainsKey(type))
            {
                LogWarning("Trying to stop a job [" + type + "] that is not running");
                return;
            }

            IEnumerator runningJob = (IEnumerator)jobTable[type];
            StopCoroutine(runningJob);
            jobTable.Remove(runningJob);
        }

        private IEnumerator RunAudioJob(AudioJob job)
        {
            yield return new WaitForSecondsRealtime(job.delay);

            AudioTrack audioTrack = (AudioTrack)audioTable[job.type];
            audioTrack.source.clip = GetAudioClipFromAudioTrack(job.type, audioTrack);

            switch (job.action)
            {
                case AudioAction.START:
                    StartCoroutine(AudioActionPlay(job, audioTrack));
                    break;
                case AudioAction.STOP:
                    StartCoroutine(AudioActionStop(job, audioTrack));
                    break;
                case AudioAction.RESTART:
                    StartCoroutine(AudioActionStop(job, audioTrack));
                    StartCoroutine(AudioActionPlay(job, audioTrack));
                    break;
            }

            jobTable.Remove(job.type);
            Log("Job count: " + jobTable.Count);

            yield return null;
        }

        private IEnumerator AudioActionStop(AudioJob job, AudioTrack audioTrack)
        {
            if (job.fadeOut)
            {
                float initialValue = 1f;
                float target = 0f;
                float duration = job.fadeInTime;
                float timer = 0f;

                while (timer < duration)
                {
                    audioTrack.source.volume = Mathf.Lerp(initialValue, target, timer / duration);
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            audioTrack.source.Stop();
        }

        private IEnumerator AudioActionPlay(AudioJob job, AudioTrack audioTrack)
        {
            audioTrack.source.Play();

            if (job.fadeIn)
            {
                float initialValue = 0f;
                float target = 1f;
                float duration = job.fadeInTime;
                float timer = 0f;

                while (timer < duration)
                {
                    audioTrack.source.volume = Mathf.Lerp(initialValue, target, timer / duration);
                    timer += Time.deltaTime;
                    yield return null;
                }
            }
        }

        private AudioClip GetAudioClipFromAudioTrack(AudioTypeEnum type, AudioTrack track)
        {
            foreach(AudioObject ao in track.audio)
            {
                if (ao.audioType == type)
                {
                    return ao.audioClip;
                }
            }
            return null;
        }

        private void Dispose()
        {
            foreach(DictionaryEntry entry in jobTable)
            {
                IEnumerator job = (IEnumerator)entry.Value;
                StopCoroutine(job);
            }
        }

        private void Log(string _msg)
        {
            if (!debug) return;
            Debug.Log("[Audio Controller]: " + _msg);
        }

        private void LogWarning(string _msg)
        {
            if (!debug) return;
            Debug.LogWarning("[Audio Controller]: " + _msg);
        }
        #endregion
    }
}