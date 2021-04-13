using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraFollow))]
public class DayNightCycleController : Singleton<DayNightCycleController>
{
    public Material sky;
    public Light directionalLight;
    public float blendThreshold;

    public Gradient gradient;

    private float valueForGradientPosition;
    private float currentBlend;
    private CameraFollow cameraFollow;

    private bool isDay = true;

    private void Start()
    {
        cameraFollow = CameraFollow.Instance;
        currentBlend = 0f;
        sky.SetFloat("_Blend", currentBlend);
        valueForGradientPosition = cameraFollow.maxPos.x - cameraFollow.minPos.x;

        AudioManager.Instance.PlayAudio(AudioTypeEnum.SoundTrack_02, 0, true, 0.5f, true, 0.5f);
    }

    private void FixedUpdate()
    {
        float currValue = (transform.position.x - cameraFollow.minPos.x) / valueForGradientPosition;
        Color nextColor = gradient.Evaluate(currValue);
        directionalLight.color = nextColor;
        sky.SetFloat("_Exposure", 2f-currValue); //2f to 1f
        sky.SetFloat("_FogIntensity", (1f - currValue) * .6f + .2f);
        //sky.SetFloat("_FogIntensity", (currValue * 0.2f) + 0.8f); //0.8f to 1f
        sky.SetColor("_TintColor", new Color(nextColor.r/2f, nextColor.g/2f, nextColor.b/2f));

        if (currValue > blendThreshold)
        {
            currentBlend = (currValue - blendThreshold) / (1f - blendThreshold);
            sky.SetFloat("_Blend", currentBlend);

            if (isDay)
            {
                isDay = !isDay;
                AudioManager.Instance.PlayAudio(AudioTypeEnum.SoundTrack_03, 0, true, 0.5f, true, 0.5f);
            }
        } else if (currentBlend > 0f)
        {
            currentBlend = 0f;
            sky.SetFloat("_Blend", currentBlend);

            if (!isDay)
            {
                isDay = !isDay;
                AudioManager.Instance.PlayAudio(AudioTypeEnum.SoundTrack_02, 0, true, 0.5f, true, 0.5f);
            }
        }
    }
}
