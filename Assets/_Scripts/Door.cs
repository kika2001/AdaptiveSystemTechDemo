using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AdaptiveSystemDemo.RoomManagement
{
    public class Door : MonoBehaviour
    {
        public Room room;
        public Transform spawnPoint;
    
        public Door nextDoor;
        private bool locked = false;
        private float lastTime;

        public void MoveSomethingToSpawn(GameObject go, Vector3 dir)
        {
            if (!locked)
            {
                if (go.CompareTag("Player"))
                {
                    go.transform.position = spawnPoint.transform.position +dir.normalized ;
                    StartCoroutine(Cooldown());
                    room.MoveEnemies();
                }else if (go.CompareTag("Enemy"))
                {
                    go.transform.parent.gameObject.GetComponent<NavMeshAgent>().Warp(spawnPoint.transform.position + dir.normalized);
                    StartCoroutine(Cooldown());
                }
            }
        }

        private IEnumerator Cooldown()
        {
            locked = true;
            yield return new WaitForSeconds(1f);
            locked = false;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!locked)
            {
                var dir = spawnPoint.transform.position - other.transform.position;
                nextDoor.MoveSomethingToSpawn(other.gameObject, dir);
            }
        
        }
    }

}
