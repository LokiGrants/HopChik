using System;
using System.Collections;
using UnityEngine;

namespace Audio
{
    public class AudioController : Singleton<AudioController>
    {
        public bool debug;

        private Hashtable audioTable; // relationship between Audio Types (key) and audio tracks

        internal static void PlayAudio(AudioType soundTrack_01)
        {
            throw new NotImplementedException();
        }

        internal static void StopAudio(AudioType soundTrack_01)
        {
            throw new NotImplementedException();
        }

        internal static void RestartAudio(AudioType soundTrack_01)
        {
            throw new NotImplementedException();
        }


        #region Unity Functions
        #endregion

        #region Public Functions
        #endregion

        #region Private Functions
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