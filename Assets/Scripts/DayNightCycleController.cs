using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraFollow))]
public class DayNightCycleController : MonoBehaviour
{
    public Material sky;
    public Light directionalLight;
    public CameraFollow cameraFollow;
    public float blendThreshold;

    public Gradient gradient;

    private float valueForGradientPosition;
    private float currentBlend;

    private void Start()
    {
        currentBlend = 0f;
        sky.SetFloat("_Blend", currentBlend);
        valueForGradientPosition = cameraFollow.maxPos.x - cameraFollow.minPos.x;
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
        } else if (currentBlend > 0f)
        {
            currentBlend = 0f;
            sky.SetFloat("_Blend", currentBlend);
        }
    }
}
