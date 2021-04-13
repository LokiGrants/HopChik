using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    private Slider slider;

    public void Start()
    {
        slider = GetComponent<Slider>();

        slider.value = PlayerPrefs.GetFloat("masterVolume", 1f);

        slider.onValueChanged.AddListener(delegate { AudioManager.Instance.ChangeMixerVolume(slider.value); });
    }
}
