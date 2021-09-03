using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace AdaptiveSystemDemo.RoomManagement
{
    public class RoomManager : MonoBehaviour
{
    /*
    public ObjectPool Pool;
    public float boxSize=19;
    private Vector3 currentRoomPos;
    public int amountEachSide=3;
    public Room[,] rooms;
    private static Vector3 centerPos;
    [SerializeField] private EnemySpawner enemySpawner;
    public Room currentRoom;

    private void Awake()
    {
        rooms = new Room[amountEachSide,amountEachSide];
        CreateRooms();
    }

    private void CreateRooms()
    {
        for (int i = 0; i < amountEachSide; i++)
        {
            for (int j = 0; j < amountEachSide; j++)
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
        for (int i = 0; i < amountEachSide; i++)
        {
            for (int j = 0; j < amountEachSide; j++)
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
            for (int i = 0; i < amountEachSide; i++)
            {
                for (int j = 0; j < amountEachSide; j++)
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
        for (int i = 0; i < amountEachSide; i++)
        {
            for (int j = 0; j < amountEachSide; j++)
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

    
    public void TpPlayer(Room room, GameObject go)
    {
        int x=0,y=0;
        for (int i = 0; i < rooms.GetLength(0); i++)
        {
            for (int j = 0; j < rooms.GetLength(1); j++)
            {
                if (rooms[i,j]==room)
                {
                    x = i;
                    y = j;
                }
            }
        }

        if (y==rooms.GetLength(0)-2)
        {
            go.transform.position = new Vector3(go.transform.position.x- (boxSize * y),go.transform.position.y,go.transform.position.z);
        }
        else if(y==1)
        {
            go.transform.position = new Vector3(go.transform.position.x+ (boxSize * rooms.GetLength(0)-2),go.transform.position.y,go.transform.position.z);
        }
        if (x==rooms.GetLength(1)-2)
        {
            go.transform.position = new Vector3(go.transform.position.x,go.transform.position.y,go.transform.position.z- (boxSize * x));
        }
        else if(x==1)
        {
            go.transform.position = new Vector3(go.transform.position.x,go.transform.position.y,go.transform.position.z+ (boxSize * rooms.GetLength(1)-2));
        }
        
    }
    */
     
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

