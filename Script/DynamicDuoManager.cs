using UnityEngine;

public class DynamicDuoManager : MonoBehaviour
{
    [System.Serializable]
    public class DuoConnection
    {
        public bool hasPartner = false;
        public string partnerID = "";
        public string partnerName = "";
        
        [Header("Progression")]
        public int intimacyPoints = 0;
        public int duoLevel = 1;
        public int pointsRequiredForNextLevel = 100;
        
        [Header("Rewards Status")]
        public int lastClaimedLevelReward = 0;
    }

    [Header("Local Player Duo State")]
    public DuoConnection myDuoData;

    private void Awake()
    {
        // इसे central Service Locator में रजिस्टर करें
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<DynamicDuoManager>(this);
        }
    }

    void Start()
    {
        // सर्वर या क्लाउड से डुओ डेटा लोड करें
        LoadDuoData();
    }

    // 1. किसी दोस्त के साथ डायनामिक डुओ कनेक्शन बनाना
    public void FormDuoConnection(string partnerID, string partnerName)
    {
        if (myDuoData.hasPartner)
        {
            Debug.LogWarning("आपके पास पहले से ही एक डायनामिक डुओ पार्टनर है!");
            return;
        }

        myDuoData.hasPartner = true;
        myDuoData.partnerID = partnerID;
        myDuoData.partnerName = partnerName;
        myDuoData.intimacyPoints = 0;
        myDuoData.duoLevel = 1;
        myDuoData.pointsRequiredForNextLevel = 100;

        Debug.Log($"[Dynamic Duo] बधाई हो! आप और {partnerName} अब डायनामिक डुओ हैं।");
        SaveDuoData();
    }

    // 2. साथ में मैच खेलने या गिफ़्ट भेजने पर पॉइंट्स जोड़ना
    public void AddIntimacyPoints(int amount)
    {
        if (!myDuoData.hasPartner) return;

        myDuoData.intimacyPoints += amount;
        Debug.Log($"[Dynamic Duo] +{amount} इंटिमेसी पॉइंट्स मिले! कुल पॉइंट्स: {myDuoData.intimacyPoints}");

        // चेक करें कि क्या डुओ लेवल अपग्रेड हुआ है
        while (myDuoData.intimacyPoints >= myDuoData.pointsRequiredForNextLevel)
        {
            DuoLevelUp();
        }

        SaveDuoData();
    }

    private void DuoLevelUp()
    {
        myDuoData.intimacyPoints -= myDuoData.pointsRequiredForNextLevel;
        myDuoData.duoLevel++;
        
        // अगले लेवल के लिए पॉइंट्स की लिमिट बढ़ाएं
        myDuoData.pointsRequiredForNextLevel = Mathf.RoundToInt(myDuoData.pointsRequiredForNextLevel * 1.5f);
        
        Debug.LogWarning($"🎉 [DUO LEVEL UP] आपका डायनामिक डुओ लेवल अब {myDuoData.duoLevel} हो गया है!");
    }

    // 3. लेवल अप होने पर मिलने वाले रिवॉर्ड्स को क्लेम करना
    public void ClaimDuoLevelReward()
    {
        if (!myDuoData.hasPartner) return;

        if (myDuoData.duoLevel > myDuoData.lastClaimedLevelReward)
        {
            int currentRewardLevel = myDuoData.lastClaimedLevelReward + 1;
            
            // EconomyManager या SkinManager के ज़रिए स्पेशल गिफ़्ट दें
            EconomyManager economy = GameServiceLocator.Instance?.GetService<EconomyManager>();
            if (economy != null)
            {
                // उदाहरण के लिए: हर डुओ लेवल अप पर 1000 बोनस गोल्ड
                economy.EarnGold(1000); 
                Debug.Log($"[Duo Reward] लेवल {currentRewardLevel} का इनाम क्लेम किया गया!");
            }

            myDuoData.lastClaimedLevelReward = currentRewardLevel;
            SaveDuoData();
        }
        else
        {
            Debug.Log("[Duo Reward] क्लेम करने के लिए कोई नया इनाम नहीं है।");
        }
    }

    // 4. कनेक्शन तोड़ने का लॉजिक (Dissolve Connection)
    public void DissolveDuoConnection()
    {
        if (!myDuoData.hasPartner) return;

        Debug.LogWarning($"{myDuoData.partnerName} के साथ डायनामिक डुओ कनेक्शन तोड़ दिया गया है।");
        myDuoData = new DuoConnection(); // डेटा रीसेट करें
        SaveDuoData();
    }

    // ---- सेव और लोड ----
    private void SaveDuoData()
    {
        string json = JsonUtility.ToJson(myDuoData);
        PlayerPrefs.SetString("DynamicDuoData", json);
        PlayerPrefs.Save();
    }

    private void LoadDuoData()
    {
        if (PlayerPrefs.HasKey("DynamicDuoData"))
        {
            string json = PlayerPrefs.GetString("DynamicDuoData");
            myDuoData = JsonUtility.FromJson<DuoConnection>(json);
        }
    }
}
