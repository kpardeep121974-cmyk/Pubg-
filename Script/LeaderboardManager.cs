using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardPlayer
{
    public int globalRank;
    public string playerName;
    public int totalKills;
    public string title; // e.g., "Grandmaster", "Conqueror", "Evogungod"
}

public class LeaderboardManager : MonoBehaviour
{
    [Header("Global Rankings Data")]
    public List<LeaderboardPlayer> topPlayersList = new List<MemoryBag.LeaderboardPlayer>();

    void Start()
    {
        GenerateMockLeaderboard();
    }

    // 1. Top 5 Global Players ki List Generate Karna (Free Fire/BGMI Concept)
    void GenerateMockLeaderboard()
    {
        topPlayersList.Add(new LeaderboardPlayer { globalRank = 1, playerName = "AWM_King_YT", totalKills = 14200, title = "Grandmaster" });
        topPlayersList.Add(new LeaderboardPlayer { globalRank = 2, playerName = "Dynamo_Fan_01", totalKills = 12950, title = "Conqueror" });
        topPlayersList.Add(new LeaderboardPlayer { globalRank = 3, playerName = "Alok_God", totalKills = 11400, title = "Grandmaster" });
        topPlayersList.Add(new LeaderboardPlayer { globalRank = 4, playerName = "Jonathan_True", totalKills = 10800, title = "Conqueror" });
        topPlayersList.Add(new LeaderboardPlayer { globalRank = 5, playerName = "Panda_Lover", totalKills = 9500, title = "Elite Warrior" });
    }

    // 2. Match ke baad agar player top par aata hai toh data refresh karna
    public void RefreshLeaderboardData()
    {
        Debug.Log("=========================================");
        Debug.Log("LOADING GLOBAL LEADERBOARD (Luxury Server)...");
        
        foreach (var player in topPlayersList)
        {
            Debug.Log("#" + player.globalRank + " | Name: " + player.playerName + " | Kills: " + player.totalKills + " | Title: [" + player.title + "]");
        }
        
        Debug.Log("=========================================");
    }

    // 3. Player ki khud ki ranking profile check karne ka function
    public void DisplayMyRank(string myName, int myKills)
    {
        Debug.Log("Your Name: " + myName + " | Total Kills: " + myKills + " | Estimated Rank: Top 1%");
    }
}
