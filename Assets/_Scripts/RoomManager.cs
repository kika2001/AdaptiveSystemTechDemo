using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace AdaptiveSystemDemo.RoomManagement
{
    public class RoomManager : MonoBehaviour
{
    public List<Room> rooms = new List<Room>();
    public Room currentRoom;
    public List<Vector3> GetRandomSpawns(int amount)
    {
        List<Vector3> spawns = new List<Vector3>();
        for (int i = 0; i < amount; i++)
        {
            bool deuPick = false;
            do
            {
                int pos = Random.Range(0, rooms.Count);
                if (rooms[pos] != currentRoom)
                {
                    spawns.Add(rooms[pos].spawnPosition.position);
                    deuPick = true;
                }
            } while (deuPick==false);
        }

        return spawns;
    }
}
}

