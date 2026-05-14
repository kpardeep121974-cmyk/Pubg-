using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetworkPlayer
{
    public string playerID;
    public string playerName;
    public int rankPoints;
    public string selectedCharacter;
    public string equippedWeaponSkin;
}

public class MatchmakingServer : MonoBehaviour
{
    [Header("Server Lobby Configuration")]
    public List<NetworkPlayer> waitingQueue = new List<NetworkPlayer>();
    public int playersRequiredPerMatch = 50; // Hybrid Battle Royale format (50 Players)
    public bool isServerSearching = false;

    [Header("Connected Map Reference")]
    public string currentActiveMap = "Erangel"; // Default BGMI Map

    // 1. Player jab "START MATCH" dabaega
    public void JoinMatchmakingQueue(string id, string name, int points, string character, string skin)
    {
        NetworkPlayer newPlayer = new NetworkPlayer
        {
            playerID = id,
            playerName = name,
            rankPoints = points,
            selectedCharacter = character,
            equippedWeaponSkin = skin
        };

        if (!waitingQueue.Exists(p => p.playerID == id))
        {
            waitingQueue.Add(newPlayer);
            Debug.Log(name + " joined the luxury matchmaking server. Players in queue: " + waitingQueue.Count);
        }

        if (!isServerSearching)
        {
            StartCoroutine(ProcessMatchmakingServer());
        }
    }

    // 2. Server Processing Loop (Background Me Chalta Hai)
    IEnumerator ProcessMatchmakingServer()
    {
        isServerSearching = true;
        Debug.Log("Server Status: Searching for balanced opponents...");

        while (waitingQueue.Count < playersRequiredPerMatch)
        {
            // Simulation: Real game mein server players ka wait karega
            yield return new WaitForSeconds(1.0f);
        }

        // 3. Match Ready Hone Par Execution
        LaunchBattleRoyaleMatch();
    }

    void LaunchBattleRoyaleMatch()
    {
        Debug.Log("=========================================");
        Debug.Log("SERVER STATUS: MATCH FOUND!");
        Debug.Log("Loading Map: " + currentActiveMap);
        Debug.Log("Spawning " + playersRequiredPerMatch + " Players in the Spawn Island!");
        Debug.Log("Notice: Gloo Walls are disabled. Pure tactical gunplay activated!");
        Debug.Log("=========================================");

        // Match start hone ke baad queue clear ho jayegi agle match ke liye
        waitingQueue.RemoveRange(0, playersRequiredPerMatch);
        isServerSearching = false;
    }

    // 4. Player agar matchmaking cancel karna chahe
    public void LeaveMatchmakingQueue(string id)
    {
        waitingQueue.RemoveAll(p => p.playerID == id);
        Debug.Log("Player left the queue. Current count: " + waitingQueue.Count);
    }
}
