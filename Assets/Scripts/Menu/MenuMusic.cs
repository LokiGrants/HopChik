using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayAudio(AudioTypeEnum.SoundTrack_01, 0, true, 0.5f, true, 1f);
    }
}
