using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Audio
{
    public class TestAudio : MonoBehaviour
    {
        #region Unity Functions
        #if UNITY_EDITOR
        AudioManager am;

        public List<AudioTest> allAudioTestList = new List<AudioTest>();

        private void Start()
        {
            am = AudioManager.Instance;
        }

        public TestAudio()
        {
            List<AudioTypeEnum> test = Enum.GetValues(typeof(AudioTypeEnum)).Cast<AudioTypeEnum>().ToList().Skip(1).ToList();
            foreach (var v in test)
            {
                AudioTest newAudioTest = new AudioTest();
                newAudioTest.audioType = v;
                allAudioTestList.Add(newAudioTest);
            }
        }   

        [Serializable]
        public class AudioTest
        {
            public AudioTypeEnum audioType;
            public KeyCode playCode;
            public KeyCode stopCode;
            public KeyCode restartCode;
            public float delay, fadeInTime, fadeOutTime;
            public bool fadeIn, fadeOut;
        }

        private void Update()
        {
            if (am.debug)
            {
                foreach (AudioTest at in allAudioTestList)
                {
                    if (Input.GetKeyUp(at.playCode))
                    {
                        am.PlayAudio(at.audioType, at.delay, at.fadeIn, at.fadeInTime, at.fadeOut, at.fadeOutTime);
                    }
                    if (Input.GetKeyUp(at.stopCode))
                    {
                        am.StopAudio(at.audioType, at.delay, at.fadeIn, at.fadeInTime, at.fadeOut, at.fadeOutTime);
                    }
                    if (Input.GetKeyUp(at.restartCode))
                    {
                        am.RestartAudio(at.audioType, at.delay, at.fadeIn, at.fadeInTime, at.fadeOut, at.fadeOutTime);
                    }
                }
            }
        }

        #endif
        #endregion
    }
}