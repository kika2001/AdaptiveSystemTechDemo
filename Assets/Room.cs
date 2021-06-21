using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomManager manager;
    public List<GameObject> enemys = new List<GameObject>();
    public Transform spawnPosition;

    public void ReturnEnemiesToPool()
    {
        foreach (var enemy in enemys)
        {
            enemy.ReturnToPool();
        }
        enemys.Clear();
    }
}