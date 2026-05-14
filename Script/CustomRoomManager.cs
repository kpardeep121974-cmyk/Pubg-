using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomRoomSettings
{
    public string roomName;
    public string roomPassword;
    public string selectedMap;    // "Erangel", "Bermuda", etc.
    public string gameMode;       // "Classic Squad", "TDM 1v1", "Gun Game"
    public int maxPlayers = 100;
    public bool allowSpectators = true;
}

public class CustomRoomManager : MonoBehaviour
{
    [Header("Connected Core Systems")]
    public EconomyManager economy;       // Room Card check karne ke liye
    public UserProfileManager profile;   // Room owner ka naam lene ke liye

    [Header("Room Status")]
    public bool isInsideCustomRoom = false;
    public string activeRoomID = "";
    public CustomRoomSettings currentRoomSettings;

    [Header("Player Slots Lists")]
    public List<string> joinedPlayersUID = new List<string>();
    public List<string> spectatorPlayersUID = new List<string>();

    // 1. BGMI/FF Style Custom Room Create Karne Ka Function
    public void CreateCustomRoom(string rName, string rPassword, string map, string mode, int maxP)
    {
        // PlayerPrefs se check karenge ki player ke paas Room Card Inventory mein hai ya nahi
        int ownedRoomCards = PlayerPrefs.GetInt("CustomRoomCards", 0);

        if (ownedRoomCards <= 0)
        {
            Debug.Log("❌ ROOM CREATION FAILED: Aapke paas Custom Room Card nahi hai! Shop se buy karein.");
            return;
        }

        // Room Card Consume/Minus karna
        ownedRoomCards--;
        PlayerPrefs.SetInt("CustomRoomCards", ownedRoomCards);

        // Settings Apply Karna
        isInsideCustomRoom = true;
        activeRoomID = "ROOM_" + Random.Range(100000, 999999).ToString();
        
        currentRoomSettings = new CustomRoomSettings {
            roomName = rName,
            roomPassword = rPassword,
            selectedMap = map,
            gameMode = mode,
            maxPlayers = maxP,
            allowSpectators = true
        };

        // Owner ko automatically slot 1 mein daal dena
        if (profile != null)
        {
            joinedPlayersUID.Add(profile.myProfile.characterID);
        }

        Debug.Log("=========================================");
        Debug.Log("🏠 CUSTOM ROOM CREATED SUCCESSFULLY!");
        Debug.Log("Room ID: " + activeRoomID + " | Password: " + currentRoomSettings.roomPassword);
        Debug.Log("Mode: " + currentRoomSettings.gameMode + " | Map: " + currentRoomSettings.selectedMap);
        Debug.Log("Cards Remaining: " + ownedRoomCards);
        Debug.Log("=========================================");
    }

    // 2. Doston ke Join karne ka Logic (ID aur Password validation)
    public void JoinCustomRoom(string enteredRoomID, string enteredPassword, string guestUID, bool wantToSpectate)
    {
        if (enteredRoomID != activeRoomID)
        {
            Debug.Log("❌ Invalid Room ID!");
            return;
        }

        if (currentRoomSettings.roomPassword != "" && enteredPassword != currentRoomSettings.roomPassword)
        {
            Debug.Log("❌ Wrong Password! Entry Denied.");
            return;
        }

        if (wantToSpectate)
        {
            spectatorPlayersUID.Add(guestUID);
            Debug.Log("👁️ Player " + guestUID + " joined as a Spectator.");
        }
        else
        {
            if (joinedPlayersUID.Count >= currentRoomSettings.maxPlayers)
            {
                Debug.Log("❌ Room is full! Cannot join.");
                return;
            }
            joinedPlayersUID.Add(guestUID);
            Debug.Log("🎮 Player " + guestUID + " joined the battle slots. Current Players: " + joinedPlayersUID.Count);
        }
    }

    // 3. Room Owner dwara match start karne ka command
    public void StartCustomRoomMatch()
    {
        if (!isInsideCustomRoom) return;

        if (joinedPlayersUID.Count < 2)
        {
            Debug.Log("⚠️ Cannot start match alone! At least 2 players required.");
            return;
        }

        Debug.Log("🚀 SERVER MESSAGE: Custom Room Match is starting on map: " + currentRoomSettings.selectedMap);
        // Yahan se load match scene ka logic invoke hoga
    }

    // 4. Room se exit karne ka logic
    public void LeaveCustomRoom(string playerUID)
    {
        if (joinedPlayersUID.Contains(playerUID)) joinedPlayersUID.Remove(playerUID);
        if (spectatorPlayersUID.Contains(playerUID)) spectatorPlayersUID.Remove(playerUID);

        if (profile != null && playerUID == profile.myProfile.characterID)
        {
            // Agar owner khud leave karega toh room dissolve ho jayega
            isInsideCustomRoom = false;
            activeRoomID = "";
            joinedPlayersUID.Clear();
            spectatorPlayersUID.Clear();
            Debug.Log("🏠 Room closed because the host left.");
        }
    }
}
