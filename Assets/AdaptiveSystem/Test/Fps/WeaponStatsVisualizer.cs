using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponStatsVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;
    void Awake()
    {
        Weapon.evBullets+= UpdateBullets;
        
    }

    private void UpdateBullets(WeaponStatus stat,int current, int max)
    {
        if (stat== WeaponStatus.Reloading)
        {
            infoText.text = "Reloading";
            
        }
        else
        {
            infoText.text = $"{current}/{max}";
        }
        
    }
}
