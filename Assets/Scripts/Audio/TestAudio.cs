using UnityEngine;

namespace Audio
{
    public class TestAudio : MonoBehaviour
    {
        #region Unity Functions
        #if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                AudioController.PlayAudio(AudioType.SoundTrack_01);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                AudioController.StopAudio(AudioType.SoundTrack_01);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                AudioController.RestartAudio(AudioType.SoundTrack_01);
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                AudioController.PlayAudio(AudioType.SoundTrack_02);
            }
            if (Input.GetKeyUp(KeyCode.X))
            {
                AudioController.StopAudio(AudioType.SoundTrack_02);
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                AudioController.RestartAudio(AudioType.SoundTrack_02);
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                AudioController.PlayAudio(AudioType.SoundEffects_01);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                AudioController.StopAudio(AudioType.SoundEffects_01);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                AudioController.RestartAudio(AudioType.SoundEffects_01);
            }
        }

        #endif
        #endregion
    }
}