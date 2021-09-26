using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptiveS.System
{
    public static class AdaptiveSystemManager
    {
        private static Dictionary<string, AdaptiveSystem> systems = new Dictionary<string, AdaptiveSystem>();

        /// <summary>
        /// Add a new Adaptive System to the manager. If exists with that name, returns the existing one, otherwise, creates a new one
        /// </summary>
        /// <param name="systemName">Name of the system</param>
        public static AdaptiveSystem NewAdaptiveSystem(string systemName)
        {
            if (!systems.ContainsKey(systemName))
            {
                AdaptiveSystem system = new AdaptiveSystem();
                system.nameIdentifier = systemName;
                systems.Add(systemName, system);
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
        /// <param name="system">Already created adaptive system</param>
        public static void NewAdaptiveSystem(AdaptiveSystem system)
        {
            if (!systems.ContainsKey(system.nameIdentifier))
            {
                systems.Add(system.nameIdentifier, system);
            }
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
                system.dados.Add(name, new Data(currentValue, referenceValue));
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
        public static void AddDataToAnalyse<T>(this T variable, float currentValue, float referenceValue, AdaptiveSystem system) where T : struct
        {
            var name = variable.ToString().ToLower();
            if (!system.dados.ContainsKey(name))
            {
                system.dados.Add(name, new Data(currentValue, referenceValue));
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
                system.dados[name].UpdateData(currentValue, referenceValue);
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
        public static void UpdateInfo<T>(this T variable, float currentValue, float referenceValue, AdaptiveSystem system)
        {
            var name = variable.ToString().ToLower();
            if (system.dados.ContainsKey(name))
            {
                system.dados[name].UpdateData(currentValue, referenceValue);
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
        public static float DataGetRatio<T>(this T variable, AdaptiveSystem system)
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
            float ratioSum = 0;
            foreach (var dado in system.dados)
            {
                if (float.IsNaN(dado.Value.GetRatio()))
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
        public static float CalculateValue(AdaptiveSystem system, float min, float max, bool rWanted)
        {
            //Pretendido = ratioplayer * ratiowanted
            //normal = ratioplayer
            if (!rWanted)
            {
                return (float) Math.Round((double) Mathf.Lerp(min, max, system.playerRatio), 2);
            }
            else
            {
                return (float) Math.Round((double) Mathf.Lerp(min, max, system.playerRatio * system.GameRatio), 2);
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
            return (float) Math.Round((double) Mathf.Lerp(min, max, ratio), 2);
        }


        /// <summary>
        /// Get the difficult ratio (player ratio * wanted ratio)
        /// </summary>
        /// <param name="system">Adaptive System where we get the ratios</param>
        /// <returns></returns>
        public static float GetCurrentWantedRatio(AdaptiveSystem system)
        {
            return system.playerRatio * system.GameRatio;
        }


        /// <summary>
        /// Get the game difficulty
        /// </summary>
        /// <param name="system">Adaptive system where it will get the ratio</param>
        /// <returns></returns>
        public static float GetCurrentGameRatio(AdaptiveSystem system)
        {
            return system.GameRatio;
        }


        /// <summary>
        /// Get the player skill
        /// </summary>
        /// <param name="system">Adaptive system where it will get the ratio</param>
        /// <returns></returns>
        public static float GetCurrentPlayerRatio(AdaptiveSystem system)
        {
            return system.playerRatio;
        }


        /// <summary>
        /// Change the game difficulty
        /// </summary>
        /// <param name="system"></param>
        /// <param name="value"></param>
        public static void ChangeGameDifficulty(AdaptiveSystem system, float value)
        {
            systems[system.nameIdentifier].GameRatio = value;
        }
    }


    public class AdaptiveSystem
    {
        public string nameIdentifier;
        public Dictionary<string,Data> dados = new Dictionary<string, Data>();
        private float pRatio = 0.5f ;
        private float gRatio=0.5f;

        public float playerRatio
        {
            get => pRatio;
            set
            {
                if (value<0)
                {
                    pRatio = 0;
                }else if (value>1)
                {
                    pRatio = 1;
                }
                else
                {
                    pRatio = value;
                }
            }
        }
        public float GameRatio
        {
            get => gRatio;
            set
            {
                if (value<0)
                {
                    gRatio = 0;
                }else if (value>1)
                {
                    gRatio = 1;
                }
                else
                {
                    gRatio = value;
                }
            }
        }


        public delegate void dgRatioUpdated(float ratio);
        public event dgRatioUpdated evRatioUpdated;

        public void InvokeRatioUpdated()
        {
            evRatioUpdated?.Invoke(playerRatio);
        }
        
        public delegate void dgDataUpdated(Dictionary<string, Data> dados);
        public event dgDataUpdated evDataUpdated;

        public void InvokeDataUpdated()
        {
            evDataUpdated?.Invoke(dados);
        }
    }

}
