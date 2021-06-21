using System;
using System.Collections.Generic;
using UnityEngine;

public static class AdaptiveSystemManager
{
    private static Dictionary<string,AdaptiveSystem> systems = new Dictionary<string, AdaptiveSystem>();

    /// <summary>
    /// Add a new Adaptive System to the manager
    /// </summary>
    /// <param name="systemName"></param>
    public static AdaptiveSystem NewAdaptiveSystem(string systemName)
    {
        if (!systems.ContainsKey(systemName))
        {
            AdaptiveSystem system = new AdaptiveSystem();
            system.nameIdentifier = systemName;
            systems.Add(systemName,system);
            return system;
        }
        else
        {
            return systems[systemName];
        }
    }
    /// <summary>
    /// Add an Adaptive System to the manager that already exits
    /// </summary>
    /// <param name="system"></param>
    public static void NewAdaptiveSystem(AdaptiveSystem system)
    {
        systems.Add(system.nameIdentifier,system);
    }
    /// <summary>
    /// Add new data to be analysed to a certain adaptive system
    /// </summary>
    /// <param name="name">Name of the Data</param>
    /// <param name="currentValue">Current value of the Data</param>
    /// <param name="referenceValue">Reference value of the Data</param>
    /// <param name="system">Adaptive system which will be added</param>
    public static void AddDataToAnalyse(string name, float currentValue, float referenceValue, AdaptiveSystem system)
    {
        name = name.ToLower();
        if (!system.dados.ContainsKey(name))
        {
            system.dados.Add(name,new Dado(currentValue,referenceValue));
            system.InvokeDataUpdated();
        }
    }
    /// <summary>
    /// Add new data to be analysed to a certain adaptive system
    /// </summary>
    /// <param name="variable">Variable of type float or int recommended</param>
    /// <param name="currentValue">Current value of the Data</param>
    /// <param name="referenceValue">Reference valye of the Data</param>
    /// <param name="system">Adaptive system which will be added</param>
    public static void AddDataToAnalyse<T>(this T variable, float currentValue, float referenceValue, AdaptiveSystem system) where T: struct
    {
        var name = variable.ToString().ToLower();
        if (!system.dados.ContainsKey(name))
        {
            system.dados.Add(name,new Dado(currentValue,referenceValue));
            system.InvokeDataUpdated();
        }
    }
    
    /// <summary>
    /// Update values from existing data from an Adaptive System
    /// </summary>
    /// <param name="name">Name of the existing data</param>
    /// <param name="currentValue">Current value of the data</param>
    /// <param name="system"> Adaptive system where the data will be updated</param>
    public static void UpdateInfo(string name, float currentValue, AdaptiveSystem system)
    {
        name = name.ToLower();
        if (system.dados.ContainsKey(name))
        {
            system.dados[name].UpdateData(currentValue);
            system.InvokeDataUpdated();
        }
    }
    /// <summary>
    /// Update values from existing data from Adaptive Sytem
    /// </summary>
    /// <param name="variable">Variable where we will get the name</param>
    /// <param name="currentValue">Current value of the data</param>
    /// <param name="system">Adaptive system where the data will be updated</param>
    public static void UpdateInfo<T>(this T variable, float currentValue, AdaptiveSystem system)
    {
        var name = variable.ToString().ToLower();
        if (system.dados.ContainsKey(name))
        {
            system.dados[name].UpdateData(currentValue);
            system.InvokeDataUpdated();
        }
    }
    /// <summary>
    /// Update values from existing data from an Adaptive System
    /// </summary>
    /// <param name="name">Name of the existing data</param>
    /// <param name="currentValue">Current value of the data</param>
    /// <param name="referenceValue">Reference value of the Data</param>
    /// <param name="system"> Adaptive system where the data will be updated</param>
    
    public static void UpdateInfo(string name, float currentValue, float referenceValue, AdaptiveSystem system)
    {
        name = name.ToLower();
        if (system.dados.ContainsKey(name))
        {
            system.dados[name].UpdateData(currentValue,referenceValue);
            system.InvokeDataUpdated();
        }
    }
    /// <summary>
    /// Update values from existing data from an Adaptive System
    /// </summary>
    /// <param name="variable">Variable where we will get the name</param>
    /// <param name="currentValue">Current value of the data</param>
    /// <param name="referenceValue">Reference value of the Data</param>
    /// <param name="system"> Adaptive system where the data will be updated</param>
    /// <typeparam name="T"></typeparam>
    public static void UpdateInfo<T>(this T variable, float currentValue, float referenceValue, AdaptiveSystem system )
    {
        var name = variable.ToString().ToLower();
        if (system.dados.ContainsKey(name))
        {
            system.dados[name].UpdateData(currentValue,referenceValue);
            system.InvokeDataUpdated();
        }
    }
    /// <summary>
    /// Returns the ratio of one Data
    /// </summary>
    /// <param name="name">Name of the existing data</param>
    /// <param name="system">Adaptive system where he will get the ratio</param>
    /// <returns></returns>

    public static float DataGetRatio(string name, AdaptiveSystem system)
    {
        if (system.dados.ContainsKey(name))
        {
            return system.dados[name].GetRatio();
        }
        else
        {
            return -1f;
        }
    }
    /// <summary>
    /// Returns the ratio of one Data
    /// </summary>
    /// <param name="variable">Variable where we will get the name</param>
    /// <param name="system">Adaptive system where he will get the ratio</param>
    /// <returns></returns>
    public static float DataGetRatio<T>(this T variable,AdaptiveSystem system)
    {
        var name = variable.ToString().ToLower();
        if (system.dados.ContainsKey(name))
        {
            return system.dados[name].GetRatio();
        }
        else
        {
            return -1f;
        }
    }

    /// <summary>
    /// Calculate ratio of an entire Adaptive System
    /// </summary>
    /// <param name="system">Adaptive system where he will get the ratio</param>
    public static void CalculateRatio(AdaptiveSystem system)
    {
        float ratioSum=0;
        foreach (var dado in system.dados)
        {
            
            if (float.IsNaN( dado.Value.GetRatio()))
            {
                //Debug.Log($"NAN {dado.Key}");
                ratioSum += 0.5f;
            }
            else
            {
                ratioSum += dado.Value.GetRatio();
                
            }
            //Debug.Log($"Dado: {dado.Key} | Ratio: {dado.Value.GetRatio()}");
        }
        system.playerRatio = ratioSum / system.dados.Count;
        system.InvokeRatioUpdated();
        system.InvokeDataUpdated();
        //Debug.Log($"Ratio Total: {ratio}");
    }

    /// <summary>
    /// Get the value based on the ratios of player or player ratio with wanted ratio mixed
    /// </summary>
    /// <param name="system">Adaptive system where we will get the ratios</param>
    /// <param name="min">Minimum value of the pretended value</param>
    /// <param name="max">Maximum value of the pretended value</param>
    /// <param name="rWanted">Wants to be affected by wanted ratio </param>
    /// <returns></returns>
    public static float CalculateValue(AdaptiveSystem system,float min, float max, bool rWanted)
    {
        //Pretendido = ratioplayer * ratiowanted
        //normal = ratioplayer
        if (!rWanted)
        {
            return (float)Math.Round((double)Mathf.Lerp(min, max, system.playerRatio),2);
        }
        else
        {
            return (float)Math.Round((double)Mathf.Lerp(min, max, system.playerRatio*system.wantedRatio),2);
        }
        
    }
    /// <summary>
    /// Get the value based on a ratio given
    /// </summary>
    /// <param name="min">Minimum value of the pretended value</param>
    /// <param name="max">Maximum value of the pretended value</param>
    /// <param name="ratio">Ratio that will give the value between min and max</param>
    /// <returns></returns>
    public static float CalculateValue(float min, float max, float ratio)
    {
        return (float)Math.Round((double)Mathf.Lerp(min, max, ratio),2);
    }

    /// <summary>
    /// Get the difficult ratio (player ratio * wanted ratio)
    /// </summary>
    /// <param name="system">Adaptive System where we get the ratios</param>
    /// <returns></returns>
    public static float GetCurrentWantedRatio(AdaptiveSystem system)
    {
        return system.playerRatio * system.wantedRatio;
    }
    
}
[Serializable]
public class AdaptiveSystem
{
    public string nameIdentifier;
    public Dictionary<string,Dado> dados = new Dictionary<string, Dado>();
    public float playerRatio = 0.5f;
    public float wantedRatio=0.5f;
    
    
    public delegate void dgRatioUpdated(float ratio);
    public event dgRatioUpdated evRatioUpdated;

    public void InvokeRatioUpdated()
    {
        evRatioUpdated?.Invoke(playerRatio);
    }
    
    public delegate void dgDataUpdated(Dictionary<string, Dado> dados);
    public event dgDataUpdated evDataUpdated;

    public void InvokeDataUpdated()
    {
        evDataUpdated?.Invoke(dados);
    }
}

