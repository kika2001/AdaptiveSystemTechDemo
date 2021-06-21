using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static List<GameObject> enemiesGO = new List<GameObject>();
    public RoomManager roomManager;
    [SerializeField] private AdaptiveSystem adaptiveSystem;
    [Header("Enemies Stuff")]
    [SerializeField] private ObjectPool enemiePool;
    private bool canSpawn;
    public float timeBetweenSpawns;
    public int wave = 0;
    public int enemiesPerWave;
    public Vector2Int extraEnemies;
    private int enemiesThisWave;

    [Header("Wave Info")]
    [SerializeField] private GameObject waveGO;
    [SerializeField] private TextMeshProUGUI waveLevel;
    [SerializeField] private TextMeshProUGUI enemiesRemainingText;
    [SerializeField] private float timeWaveBetweenWaves;

    private void Awake()
    {
        adaptiveSystem = AdaptiveSystemManager.NewAdaptiveSystem("player");
    }

    private void Start()
    {
        StartCoroutine(WaveIntro());
    }

    private IEnumerator WaveIntro()
    {
        canSpawn = false;
        wave++;
        Debug.Log($"Wave: {wave}");
        waveGO.SetActive(true);
        waveGO.transform.Find("text").GetComponent<TextMeshProUGUI>().text = $"WAVE {wave}";
        waveLevel.text = $"WAVE {wave}";
        yield return new WaitForSeconds(timeWaveBetweenWaves);
        waveGO.SetActive(false);
        Debug.Log($"Wave {wave} started!");
        SpawnEnemies();
        canSpawn = true;
    }

    private void SpawnEnemies()
    {
        enemiesThisWave = (enemiesPerWave*wave) + (int)Mathf.CeilToInt(AdaptiveSystemManager.CalculateValue(adaptiveSystem,extraEnemies.x, extraEnemies.y, true))+wave;
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
        if (enemiesGO.Count()<=0 && canSpawn)
        {
            canSpawn = false;
            StartCoroutine(WaveIntro());
            
        }
    }

    public void ReSpawnEnemies(int amount)
    {
        for (int i = 0; i < enemiesGO.Count(); i++)
        {
            enemiesGO[i].ReturnToPool();
            enemiesGO[i] = null;
        }
        enemiesGO.Clear();
        var spawns = roomManager.GetRandomSpawns(amount);
        foreach (var spawn in spawns)
        {
            var go = enemiePool.GetObject();
            go.transform.position = spawn;
            enemiesGO.Add(go);
        }
    }
   
}
