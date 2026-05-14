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
    
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameMap
{
    public string mapName;
    public string mapSize;          // e.g., "8x8 km", "2x2 km"
    public bool isDownloaded;
    public string mapType;          // "Classic", "TDM", "Training"
}

public class MapManager : MonoBehaviour
{
    [Header("Available Maps Database")]
    public List<GameMap> availableMaps = new List<GameMap>();
    public GameMap currentlySelectedMap;

    void Start()
    {
        InitializeMaps();
    }

    // 1. BGMI Maps with Training Ground Option
    void InitializeMaps()
    {
        availableMaps.Clear();

        // Classic Battle Royale Maps
        availableMaps.Add(new GameMap { mapName = "Erangel", mapSize = "8x8 km", isDownloaded = true, mapType = "Classic" });
        availableMaps.Add(new GameMap { mapName = "Sanhok", mapSize = "4x4 km", isDownloaded = true, mapType = "Classic" });
        availableMaps.Add(new GameMap { mapName = "Livik", mapSize = "2x2 km", isDownloaded = true, mapType = "Classic" });

        // TDM Maps
        availableMaps.Add(new GameMap { mapName = "Inventory (TDM)", mapSize = "Small", isDownloaded = true, mapType = "TDM" });

        // NEW: BGMI Style Training Ground / Cheer Park
        availableMaps.Add(new GameMap { 
            mapName = "BGMI Training Ground", 
            mapSize = "1x1 km", 
            isDownloaded = true, 
            mapType = "Training" 
        });

        // Default selection setup
        currentlySelectedMap = availableMaps[0]; // Default: Erangel
    }

    // 2. Map Select Karne Ka Logic
    public void SelectMap(string targetMapName)
    {
        foreach (var map in availableMaps)
        {
            if (map.mapName == targetMapName)
            {
                if (!map.isDownloaded)
                {
                    Debug.Log("❌ Cannot select " + targetMapName + ". Map download required!");
                    return;
                }

                currentlySelectedMap = map;
                Debug.Log("🗺️ Map Selected: " + map.mapName + " [" + map.mapType + " Mode]");
                return;
            }
        }
    }

    // 3. UI Start Button Click Par Zone/Map Load Karne Ka Logic
    public void LoadSelectedMapScene()
    {
        Debug.Log("=========================================");
        Debug.Log("🚀 LOADING SCENE: " + currentlySelectedMap.mapName);
        
        if (currentlySelectedMap.mapType == "Training")
        {
            Debug.Log("🎯 Entering Training Mode: Infinite Ammo and Target Dummies spawned.");
            // Yahan infinite ammo aur safe health variables trigger honge
        }
        else
        {
            Debug.Log("🪂 Entering Matchmaking Arena: Total 100 Players Grid Initialized.");
        }
        Debug.Log("=========================================");
    }
}
