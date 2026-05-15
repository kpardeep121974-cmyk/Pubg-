using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionPingManager : MonoBehaviour
{
    [System.Serializable]
    public class ServerRegion
    {
        public string regionName;     // जैसे: "Asia (Mumbai)", "Europe (Frankfurt)"
        public string serverIPAddress; // सर्वर का IP या URL
        public int currentPing = 999;  // मिलिसैकंड (ms) में पिंग
    }

    [Header("Available Game Servers")]
    public List<ServerRegion> availableRegions = new List<ServerRegion>();
    public ServerRegion bestRegion;

    [Header("Live In-Game Ping")]
    public int currentLivePing = 0;
    public float pingUpdateInterval = 2.0f; // हर 2 सेकंड में इन-गेम पिंग अपडेट होगा

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<RegionPingManager>(this);
        }
    }

    void Start()
    {
        // डमी सर्वर डेटा सेट करना (इंस्पेक्टर से भी भरा जा सकता है)
        if (availableRegions.Count == 0)
        {
            availableRegions.Add(new ServerRegion { regionName = "Asia", serverIPAddress = "192.168.1.1" });
            availableRegions.Add(new ServerRegion { regionName = "Europe", serverIPAddress = "192.168.1.2" });
            availableRegions.Add(new ServerRegion { regionName = "North America", serverIPAddress = "192.168.1.3" });
        }

        // लॉबी में आते ही सभी सर्वर्स का पिंग टेस्ट शुरू करें
        StartCoroutine(TestAllRegionsPingRoutine());
    }

    // 1. सभी सर्वर्स का पिंग मापना और सबसे बेस्ट सर्वर चुनना
    IEnumerator TestAllRegionsPingRoutine()
    {
        Debug.Log("[Ping Manager] सभी रीजन्स का पिंग टेस्ट शुरू हो रहा है...");
        ServerRegion lowestPingRegion = null;
        int minPing = 9999;

        foreach (ServerRegion region in availableRegions)
        {
            // 🚀 यूनिटी का इन-बिल्ट Ping क्लास इस्तेमाल करें (यह असली नेटवर्क पिंग मापेगा)
            // Ping testPing = new Ping(region.serverIPAddress);
            // yield return new WaitUntil(() => testPing.isDone);
            // region.currentPing = testPing.time;

            // सिमुलेशन के लिए रैंडम पिंग जनरेट कर रहे हैं:
            yield return new WaitForSeconds(0.1f);
            if (region.regionName == "Asia") region.currentPing = Random.Range(20, 45);      // इंडिया के लिए एशिया बेस्ट
            else if (region.regionName == "Europe") region.currentPing = Random.Range(120, 180);
            else region.currentPing = Random.Range(250, 350);

            Debug.Log($"[Server] {region.regionName} Ping: {region.currentPing}ms");

            if (region.currentPing < minPing)
            {
                minPing = region.currentPing;
                lowestPingRegion = region;
            }
        }

        bestRegion = lowestPingRegion;
        Debug.LogWarning($"[Ping Manager] बेस्ट सर्वर चुना गया: {bestRegion.regionName} ({bestRegion.currentPing}ms)");
        
        // बेस्ट सर्वर से कनेक्ट होने के बाद इन-गेम लाइव पिंग लूप शुरू करें
        StartCoroutine(LivePingUpdateRoutine());
    }

    // 2. मैच के दौरान स्क्रीन के कोने में लाइव पिंग दिखाना
    IEnumerator LivePingUpdateRoutine()
    {
        while (true)
        {
            // मैच के दौरान पिंग में थोड़ा बहुत उतार-चढ़ाव सिमुलेट करें
            if (bestRegion != null)
            {
                currentLivePing = bestRegion.currentPing + Random.Range(-5, 5);
                UpdatePingUI(currentLivePing);
            }

            yield return new WaitForSeconds(pingUpdateInterval);
        }
    }

    private void UpdatePingUI(int pingValue)
    {
        // यहाँ आप अपने HUD UI टेक्स्ट को अपडेट करेंगे और कलर बदलेंगे
        // Green: < 60ms | Yellow: 60ms - 150ms | Red: > 150ms
        
        string colorTag = "<color=green>";
        if (pingValue > 150) colorTag = "<color=red>";
        else if (pingValue > 60) colorTag = "<color=yellow>";

        // डिबग लॉग में कलर के साथ प्रिंट करना
        // Debug.Log($"[HUD Ping] {colorTag}{pingValue} ms</color>");
    }
}
