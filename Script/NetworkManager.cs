if (!isLocalPlayer) return; // Isse ek player dusre ke player ko move nahi kar payega
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    [Header("Custom Spawn Settings")]
    public GameObject playerPrefabToSpawn; // Aapka Player Character Prefab
    public Transform[] spawnPoints;        // Map par alag-alag spawn locations

    // 1. Jab koi Client Server se successfully connect ho jaye
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Ek random spawn point select karna
        Transform startPoint = spawnPoints.Length > 0 
            ? spawnPoints[Random.Range(0, spawnPoints.Length)] 
            : null;

        Vector3 spawnPos = startPoint != null ? startPoint.position : Vector3.zero;
        Quaternion spawnRot = startPoint != null ? startPoint.rotation : Quaternion.identity;

        // Player game object ko server par create karna
        GameObject player = Instantiate(playerPrefabToSpawn, spawnPos, spawnRot);

        // Player ko network par baaki saare clients ke sath sync karna
        NetworkServer.AddPlayerForConnection(conn, player);
        
        Debug.Log($"Player connected and spawned at: {spawnPos}");
    }

    // 2. Jab koi client disconnect ho jaye
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // Server par cleanup logic (jaise match list se hatana)
        base.OnServerDisconnect(conn);
        Debug.Log("Player disconnected from server.");
    }

    // 3. Clientside helper functions (UI buttons ke liye)
    public void StartHostGame()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            StartHost(); // Wahi player server bhi banega aur client bhi (Like Custom Room Creator)
        }
    }

    public void JoinGame(string ipAddress)
    {
        this.networkAddress = ipAddress;
        StartClient(); // Dusre ka room join karne ke liye
    }
}
