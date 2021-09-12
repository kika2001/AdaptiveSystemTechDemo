using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptiveSystemDemo.RoomManagement
{
    public class RoomSensor : MonoBehaviour
    {
        [SerializeField]private Room room;
    
        /*
         private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                room.manager.currentRoom=room;
                room.manager.UpdateRoomPosition(room);
            }else if (other.CompareTag("Enemy"))
            {
                AddEnemies(other.transform.parent.gameObject);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                room.manager.TpPlayer(room,other.gameObject);
                room.manager.currentRoom=room;
            }else if (other.CompareTag("Enemy"))
            {
                AddEnemies(other.transform.parent.gameObject);
            }
        }
        */
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //room.manager.TpPlayer(room,other.gameObject);
                room.manager.currentRoom=room;
            }else if (other.CompareTag("Enemy"))
            {
                //Debug.LogWarning($"Enemy : {other.gameObject}");
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
}

