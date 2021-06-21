using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSensor : MonoBehaviour
{
    [SerializeField]private Room room;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            room.manager.UpdateRoomPosition(room);
            room.manager.currentRoom=room;
        }else if (other.CompareTag("Enemy"))
        {
            AddEnemies(other.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            RemoveEnemies(other.transform.parent.gameObject);
        }
    }

    private void RemoveEnemies(GameObject go)
    {
        room.enemys.Remove(go);
    }

    public void AddEnemies(GameObject go)
    {
        room.enemys.Add(go);
    }
}
