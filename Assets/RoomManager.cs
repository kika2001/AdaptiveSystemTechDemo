using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class RoomManager : MonoBehaviour
{
    public ObjectPool Pool;
    public float boxSize=19;
    private Vector3 currentRoomPos;
    public Room[,] rooms = new Room[3,3];
    private Room[,] roomstemp = new Room[3,3];
    private static Vector3 centerPos;
    [SerializeField] private EnemySpawner enemySpawner;
    public Room currentRoom;

    private void Awake()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var go = Pool.GetObject().GetComponent<Room>();
                go.transform.parent = transform;
                rooms[i, j] = go;
                go.manager = this;
                go.transform.localPosition = new Vector3(-boxSize + (j*boxSize),0,-boxSize+(i*boxSize));
            }
        }
    }
    private void CreateRooms(Vector3 roomPos)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var go = Pool.GetObject().GetComponent<Room>();
                go.transform.parent = transform;
                rooms[i, j] = go;
                go.manager = this;
                go.transform.localPosition = new Vector3(-boxSize + (j*boxSize) + roomPos.x,0,-boxSize+(i*boxSize)+ roomPos.z);
            }
        }
        Debug.LogWarning("Created new rooms");
    }
    public void UpdateRoomPosition(Room room)
    {
        int amountEnemiesToSpawn = 0;
        if (currentRoomPos != room.transform.position)
        {
            currentRoomPos = room.transform.position;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    amountEnemiesToSpawn += rooms[i, j].enemys.Count();
                    //Bug - Ele não dá respawn direito
                    //rooms[i,j].ReturnEnemiesToPool();
                    rooms[i,j].gameObject.ReturnToPool();
                    rooms[i, j] = null;
                }
            }
            Debug.LogWarning("Destroyed rooms");
            CreateRooms(currentRoomPos);
            //Bug - Ele não dá respawn direito
            //enemySpawner.ReSpawnEnemies(amountEnemiesToSpawn);
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
               rooms[i,j].GetComponent<NavMeshSurface>().BuildNavMesh();
            }
        }
        
    }

    public List<Vector3> GetRandomSpawns(int amount)
    {
        List<Vector3> spawns = new List<Vector3>();
        for (int i = 0; i < amount; i++)
        {
            bool deuPick = false;
            do
            {
                Vector2Int pos = new Vector2Int(Random.Range(0,rooms.GetLength(0)),Random.Range(0,rooms.GetLength(1)));
                if (rooms[pos.x,pos.y] != currentRoom)
                {
                    spawns.Add(rooms[pos.x,pos.y].spawnPosition.position);
                    deuPick = true;
                }
            } while (deuPick==false);
        }

        return spawns;
    }
}
