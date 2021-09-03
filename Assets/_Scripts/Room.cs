using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AdaptiveSystemDemo.RoomManagement
{
    public class Room : MonoBehaviour
    {
        public RoomManager manager;
        public List<GameObject> enemys = new List<GameObject>();
        public Transform spawnPosition;

        public bool works=true;
        /*
        public void ReturnEnemiesToPool()
        {
            foreach (var enemy in enemys)
            {
                enemy.ReturnToPool();
            }
            enemys.Clear();
        }
        */
        private void Update()
        {
            if (!works)
            {
                //Debug.LogError($"Enemys Count: {enemys.Count}");
            }
        }

        public void MoveEnemies()
        {
            //Debug.Log($"MoveEnemies : {enemys.Count}");
            var places = manager.GetRandomSpawns(enemys.Count);
            for (int i = 0; i < places.Count; i++)
            {
                enemys[i].GetComponent<NavMeshAgent>().Warp(places[i]);
            }
        }
    }
}
