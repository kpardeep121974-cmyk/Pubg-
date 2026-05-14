using UnityEngine;
using Firebase;
using Firebase.Firestore;
using System.Threading.Tasks;
using System.Collections.Generic;

public class DatabaseHandler : MonoBehaviour
{
    private FirebaseFirestore db;

    // Player Data Structure
    [System.Serializable]
    public class PlayerData
    {
        public string username;
        public int level;
        public int coins;
        public int totalKills;
    }

    void Start()
    {
        // Firebase Initialize Karna
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase Database Successfully Connected!");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    // 1. Data SAVE Karne Ka Function
    public async Task SavePlayerData(string playerId, string name, int lvl, int currentCoins, int kills)
    {
        if (db == null) { Debug.LogError("Database not initialized!"); return; }

        DocumentReference docRef = db.Collection("players").Document(playerId);

        Dictionary<string, object> userMap = new Dictionary<string, object>
        {
            { "username", name },
            { "level", lvl },
            { "coins", currentCoins },
            { "totalKills", kills }
        };

        await docRef.SetAsync(userMap).ContinueWith(task => {
            if (task.IsCompleted)
            {
                Debug.Log($"Player data successfully saved for ID: {playerId}");
            }
            else
            {
                Debug.LogError("Failed to save player data.");
            }
        });
    }

    // 2. Data LOAD Karne Ka Function
    public async Task<PlayerData> LoadPlayerData(string playerId)
    {
        if (db == null) { Debug.LogError("Database not initialized!"); return null; }

        DocumentReference docRef = db.Collection("players").Document(playerId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            PlayerData data = new PlayerData();
            Dictionary<string, object> userMap = snapshot.ToDictionary();

            if (userMap.ContainsKey("username")) data.username = userMap["username"].ToString();
            if (userMap.ContainsKey("level")) data.level = int.Parse(userMap["level"].ToString());
            if (userMap.ContainsKey("coins")) data.coins = int.Parse(userMap["coins"].ToString());
            if (userMap.ContainsKey("totalKills")) data.totalKills = int.Parse(userMap["totalKills"].ToString());

            Debug.Log($"Player Data Loaded: {data.username}, Level: {data.level}");
            return data;
        }
        else
        {
            Debug.LogWarning("Player not found in database.");
            return null;
        }
    }
}
