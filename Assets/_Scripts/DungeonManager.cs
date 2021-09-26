using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts;
using AdaptiveS.System;
using AdaptiveSystemDemo.Character;
using AdaptiveSystemDemo.Health;
using AdaptiveSystemDemo.RoomManagement;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace AdaptiveSystemDemo.Enemy
{
    public class DungeonManager : MonoBehaviour
    {
        public static List<GameObject> enemiesGO = new List<GameObject>();
        public RoomManager roomManager;
        private AdaptiveSystem adaptiveSystem;

        [Header("Enemies Stuff")] [SerializeField]
        private ObjectPool enemiePool;

        private bool canSpawn;
        public int wave = 0;
        public int enemiesPerWave;
        public Vector2Int extraEnemies;
        private int enemiesThisWave;

        [Header("Wave Info")] 
        [SerializeField] private GameObject waveParent;
        [SerializeField] private GameObject waveNextLevelGO;
        [SerializeField] private TextMeshProUGUI waveCurrentLevel;
        [SerializeField] private TextMeshProUGUI enemiesRemainingText;
        [SerializeField] private float timeWaveBetweenWaves;
        

        [Header("End Screen")] 
        [SerializeField] private EndScreen endScreenParent;

        private void Awake()
        {
            adaptiveSystem = AdaptiveSystemManager.NewAdaptiveSystem("player");
        }

        private void ResetValues()
        {
            waveNextLevelGO.SetActive(false);
            waveParent.SetActive(true);
        }

        public void OnPlayerDeath()
        {
            endScreenParent.PlayEndScreen();
            foreach (var enemie in enemiesGO)
            {
                enemie.ReturnToPool();
            }
            
        }

        private void Start()
        {
            StartCoroutine(WaveIntro());
        }

        private IEnumerator WaveIntro()
        {
            canSpawn = false;
            wave++;
            //Debug.Log($"Wave: {wave}");
            waveNextLevelGO.SetActive(true);
            waveNextLevelGO.transform.Find("text").GetComponent<TextMeshProUGUI>().text = $"WAVE {wave}";
            waveCurrentLevel.text = $"WAVE {wave}";
            yield return new WaitForSeconds(timeWaveBetweenWaves);
            waveNextLevelGO.SetActive(false);
            //Debug.Log($"Wave {wave} started!");
            SpawnEnemies();
            canSpawn = true;
        }

        private void SpawnEnemies()
        {
            enemiesThisWave = (enemiesPerWave * wave) +
                              (int) Mathf.CeilToInt(AdaptiveSystemManager.CalculateValue(adaptiveSystem, extraEnemies.x,
                                  extraEnemies.y, true)) + wave;
            var spawns = roomManager.GetRandomSpawns(enemiesThisWave);
            foreach (var spawn in spawns)
            {
                var go = enemiePool.GetObject();
                go.GetComponent<NavMeshAgent>().Warp(spawn);
                enemiesGO.Add(go);
            }
        }

        private void Update()
        {
            enemiesRemainingText.text = $"Enemies Remaining:{enemiesGO.Count()}";
            if (enemiesGO.Count() <= 0 && canSpawn)
            {
                canSpawn = false;
                StartCoroutine(WaveIntro());
            }
        }


      
    }
}