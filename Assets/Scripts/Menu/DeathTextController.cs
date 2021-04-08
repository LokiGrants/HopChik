using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathTextController : MonoBehaviour
{
    private TextMeshProUGUI deathText;

    private void Start()
    {
        deathText = GetComponent<TextMeshProUGUI>();
        deathText.text = "Deaths in this session: " + GameFlags.Instance.deathCounter;
    }

    private void OnEnable()
    {
        if (deathText == null)
            deathText = GetComponent<TextMeshProUGUI>();
        deathText.text = "Deaths in this session: " + GameFlags.Instance.deathCounter;
    }
}
