using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdaptiveSystemVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ratioUI;
    [SerializeField] private TextMeshProUGUI dataUI;
    private AdaptiveSystem adaptiveSystem;
    private void Awake()
    {
        adaptiveSystem=AdaptiveSystemManager.NewAdaptiveSystem("player");
        adaptiveSystem.evRatioUpdated+= UpdateRatioText;
        adaptiveSystem.evDataUpdated += UpdateDataText;
    }

    private void UpdateDataText(Dictionary<string, Dado> dados)
    {
        dataUI.text = "";
        foreach (var dado in dados.Keys)
        {
            dataUI.text += $"{dado}: {dados[dado].GetRatio()} \n";
        }
    }

    private void UpdateRatioText(float ratio)
    {
        ratioUI.text = $"Current Ratio:{ratio}";
    }
}
