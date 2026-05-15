using System.Collections.Generic;
using UnityEngine;

public class MapStreamingManager : MonoBehaviour
{
    [System.Serializable]
    public class MapChunk
    {
        public string chunkName;
        public GameObject chunkObject; // मैप का वह हिस्सा (Building/Terrain) जो इस ग्रिड में है
        public Vector3 chunkCenter;    // उस हिस्से का केंद्र बिंदु
    }

    [Header("Player Tracking")]
    public Transform playerTransform;
    
    [Header("Streaming Settings")]
    public float renderDistance = 300f; // कितनी दूरी तक का मैप दिखाई देना चाहिए
    public float checkInterval = 1.5f;   // हर कितने सेकंड में प्लेयर की पोजीशन चेक करनी है

    public List<MapChunk> mapChunks = new List<MapChunk>();
    private float nextCheckTime;

    private void Awake()
    {
        // इसे Service Locator में रजिस्टर करें
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<MapStreamingManager>(this);
        }
    }

    void Start()
    {
        // अगर प्लेयर का ट्रांसफॉर्म असाइन नहीं है, तो लोकल प्लेयर को ढूंढें
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }

        // गेम शुरू होते ही एक बार तुरंत चेक करें
        UpdateMapChunks();
    }

    void Update()
    {
        // परफॉर्मेंस बचाने के लिए इसे हर फ्रेम (Update) में चलाने के बजाय एक इंटरवल पर चलाएंगे
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            UpdateMapChunks();
        }
    }

    void UpdateMapChunks()
    {
        if (playerTransform == null) return;

        Vector3 playerPos = playerTransform.position;

        foreach (MapChunk chunk in mapChunks)
        {
            if (chunk.chunkObject == null) continue;

            // प्लेयर और मैप चंक के बीच की दूरी मापें
            float distance = Vector3.Distance(playerPos, chunk.chunkCenter);

            if (distance <= renderDistance)
            {
                // अगर प्लेयर चंक के करीब है, तो उसे लोड (Activate) करें
                if (!chunk.chunkObject.activeSelf)
                {
                    chunk.chunkObject.SetActive(true);
                    Debug.Log($"[MapStreaming] Loaded: {chunk.chunkName}");
                }
            }
            else
            {
                // अगर प्लेयर बहुत दूर चला गया है, तो उसे अनलोड (Deactivate) करके रैम (RAM) खाली करें
                if (chunk.chunkObject.activeSelf)
                {
                    chunk.chunkObject.SetActive(false);
                    Debug.Log($"[MapStreaming] Unloaded: {chunk.chunkName}");
                }
            }
        }
    }
}
