using System;
using UnityEngine;

public class VIPSubscriptionManager : MonoBehaviour
{
    [System.Serializable]
    public class VIPProfile
    {
        public bool isVIPSubscriber = false;
        public string subscriptionExpiryDate;
        public string lastClaimedDate; // यह चेक करने के लिए कि प्लेयर आज का रिवॉर्ड ले चुका है या नहीं
        public int dailyDiamondReward = 50; // रोज मिलने वाले डायमंड्स
    }

    public VIPProfile playerVIPData;

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<VIPSubscriptionManager>(this);
        }
    }

    void Start()
    {
        LoadVIPData();
    }

    // जब प्लेयर स्टोर से VIP मंथली पास खरीदेगा
    public void PurchaseVIPSubscription()
    {
        playerVIPData.isVIPSubscriber = true;
        // आज से 30 दिन आगे की एक्सपायरी सेट करें
        playerVIPData.subscriptionExpiryDate = DateTime.UtcNow.AddDays(30).ToString("yyyy-MM-dd");
        playerVIPData.lastClaimedDate = ""; // अभी तक कोई डेली रिवॉर्ड क्लेम नहीं किया

        Debug.LogWarning("🎉 [VIP] प्लेयर सफलतापूर्वक VIP सब्सक्राइबर बन गया है! 30 दिनों का लूप शुरू।");
        SaveVIPData();
    }

    // प्लेयर हर रोज गेम खोलकर इस बटन पर क्लिक करेगा
    public void ClaimDailyVIPReward()
    {
        if (!playerVIPData.isVIPSubscriber)
        {
            Debug.LogError("[VIP] आपके पास वीआईपी पास नहीं है!");
            return;
        }

        // चेक करें कि क्या सब्सक्रिप्शन की तारीख खत्म तो नहीं हो गई
        DateTime expiry = DateTime.Parse(playerVIPData.subscriptionExpiryDate);
        if (DateTime.UtcNow > expiry)
        {
            playerVIPData.isVIPSubscriber = false;
            Debug.LogError("[VIP] आपका मंथली पास एक्सपायर हो चुका है! कृपया रीन्यू करें।");
            SaveVIPData();
            return;
        }

        string today = DateTime.UtcNow.ToString("yyyy-MM-dd");

        // चेक करें कि प्लेयर आज का रिवॉर्ड पहले ही तो नहीं ले चुका (टाइम-चीटिंग को रोकने के लिए)
        if (playerVIPData.lastClaimedDate == today)
        {
            Debug.Log("[VIP] आप आज का रिवॉर्ड पहले ही क्लेम कर चुके हैं। कल दोबारा आएं!");
            return;
        }

        // रिवॉर्ड दें
        playerVIPData.lastClaimedDate = today;
        
        EconomyManager economy = GameServiceLocator.Instance?.GetService<EconomyManager>();
        if (economy != null)
        {
            economy.EarnGold(playerVIPData.dailyDiamondReward); // प्लेयर के वॉलेट में करेंसी ऐड करें
            Debug.LogWarning($"🔥 [VIP CLAIM] सफलता पूर्वक आज के {playerVIPData.dailyDiamondReward} डायमंड्स क्लेम कर लिए गए हैं!");
        }

        SaveVIPData();
    }

    // ---- डेटा सेव और लोड ----
    private void SaveVIPData()
    {
        string json = JsonUtility.ToJson(playerVIPData);
        PlayerPrefs.SetString("VIPSubscriptionData", json);
        PlayerPrefs.Save();
    }

    private void LoadVIPData()
    {
        if (PlayerPrefs.HasKey("VIPSubscriptionData"))
        {
            string json = PlayerPrefs.GetString("VIPSubscriptionData");
            playerVIPData = JsonUtility.FromJson<VIPProfile>(json);
        }
    }
}
