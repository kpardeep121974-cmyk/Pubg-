using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGMIMap
{
    public string mapName;
    public string terrainType; // e.g., Grassland, Desert, Jungle
    public string mapSize;      // e.g., 8x8, 4x4
    public bool hasDynamicWeather;
    public int maxPlayers;
}

public class MapManager : MonoBehaviour
{
    public List<BGMIMap> mapRotation = new List<BGMIMap>();

    void Start()
    {
        InitializeBGMIMaps();
    }

    void InitializeBGMIMaps()
    {
        // 1. ERANGEL (The Classic)
        mapRotation.Add(new BGMIMap { 
            mapName = "Erangel", 
            terrainType = "Grassland", 
            mapSize = "8x8 km", 
            hasDynamicWeather = true, 
            maxPlayers = 100 
        });

        // 2. MIRAMAR (The Desert)
        mapRotation.Add(new BGMIMap { 
            mapName = "Miramar", 
            terrainType = "Desert", 
            mapSize = "8x8 km", 
            hasDynamicWeather = false, 
            maxPlayers = 100 
        });

        // 3. SANHOK (The Jungle - Fast Paced)
        mapRotation.Add(new BGMIMap { 
            mapName = "Sanhok", 
            terrainType = "Rainforest", 
            mapSize = "4x4 km", 
            hasDynamicWeather = true, 
            maxPlayers = 100 
        });

        // 4. VIKENDI (The Snow Map)
        mapRotation.Add(new BGMIMap { 
            mapName = "Vikendi", 
            terrainType = "Snow", 
            mapSize = "6x6 km", 
            hasDynamicWeather = true, 
            maxPlayers = 100 
        });

        // 5. LIVIK (Fast Match)
        mapRotation.Add(new BGMIMap { 
            mapName = "Livik", 
            terrainType = "Mixed (Nordic)", 
            mapSize = "2x2 km", 
            hasDynamicWeather = true, 
            maxPlayers = 52 
        });
    }

    public void LoadMap(string name)
    {
        Debug.Log("Luxury Loading: " + name + "... Prepare for Battle!");
        // Yahan aapka scene loading logic aayega
    using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CustomRoom
{
    public string roomID;
    public string roomName;
    public string password;
    public string selectedMap;
    public bool isLuxuryRoom; // Special Room with extra rewards
    public int currentPlayers;
    public int maxPlayers = 100;
}

public class RoomManager : MonoBehaviour
{
    public List<CustomRoom> activeRooms = new List<CustomRoom>();
    public EconomyManager economy;

    // Room Card ki cost (Luxury price)
    public int roomCardCostGems = 100;

    public void CreateRoom(string name, string pass, string map, bool luxury)
    {
        // Pehle check karein ki player ke paas Gems hain ya nahi
        if (economy.PurchaseItem(roomCardCostGems, true))
        {
            CustomRoom newRoom = new CustomRoom();
            newRoom.roomID = "ROOM_" + Random.Range(1000, 9999);
            newRoom.roomName = name;
            newRoom.password = pass;
            newRoom.selectedMap = map;
            newRoom.isLuxuryRoom = luxury;
            newRoom.currentPlayers = 1;

            activeRooms.Add(newRoom);
            Debug.Log("Room Created Successfully! ID: " + newRoom.roomID);
        }
        else
        {
            Debug.Log("Not enough Gems to buy a Room Card!");
        }
    }

    public void JoinRoom(string id, string pass)
    {
        foreach (var room in activeRooms)
        {
            if (room.roomID == id && room.password == pass)
            {
                if (room.currentPlayers < room.maxPlayers)
                {
                    room.currentPlayers++;
                    Debug.Log("Joined Room: " + room.roomName);
                }
                else
                {
                    Debug.Log("Room is Full!");
                }
                return;
            }
        }
        Debug.Log("Invalid Room ID or Password!");
    }
}
