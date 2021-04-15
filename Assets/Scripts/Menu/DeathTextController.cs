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
        deathText.text = GameFlags.Instance.deathCounter.ToString();
    }

    private void OnEnable()
    {
        if (deathText == null)
            deathText = GetComponent<TextMeshProUGUI>();
        deathText.text = GameFlags.Instance.deathCounter.ToString();
    }
}
