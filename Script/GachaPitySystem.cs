using UnityEngine;

public class GachaPitySystem : MonoBehaviour
{
    [System.Serializable]
    public class PlayerGachaState
    {
        public int currentLuckPoints = 0;
        public int maxPityThreshold = 50; // 50वें स्पिन पर ग्रैंड प्राइज मिलना 100% पक्का है
        public bool grandPrizeWon = false;
    }

    [Header("Player Gacha Profile")]
    public PlayerGachaState playerLuckData;

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<GachaPitySystem>(this);
        }
    }

    // जब प्लेयर 1 रीचार्ज टोकन/डायमंड का स्पिन (Spin) करेगा
    public void ExecuteGachaSpin()
    {
        playerLuckData.currentLuckPoints++;
        
        // 1. चेक करें कि क्या प्लेयर Pity Threshold (मैक्स लिमिट) पर पहुँच गया है
        if (playerLuckData.currentLuckPoints >= playerLuckData.maxPityThreshold)
        {
            AwardGrandPrize("🎉 [JACKPOT] बधाई हो! Pity System के ज़रिए आपको अल्ट्रा-रेयर गन स्किन/बंडल मिला है!");
            return;
        }

        // 2. अगर मैक्स लिमिट पर नहीं है, तो लक के आधार पर रैंडम चांस कैलकुलेट करें
        // जैसे-जैसे लक पॉइंट्स बढ़ेंगे, जीतने की संभावना (Probability) भी बढ़ती जाएगी
        float baseWinningChance = 0.02f; // बेस चांस सिर्फ 2% है
        float dynamicChance = baseWinningChance + (playerLuckData.currentLuckPoints * 0.01f); // हर स्पिन पर 1% चांस बढ़ेगा

        float randomRoll = Random.value; // 0.0 से 1.0 के बीच रैंडम नंबर

        if (randomRoll <= dynamicChance)
        {
            AwardGrandPrize("🔥 [LUCK WIN] आपकी किस्मत चमकी! आपको रैंडम स्पिन में ही ग्रैंड प्राइज मिल गया!");
        }
        else
        {
            // प्लेयर जैकपॉट चूका, उसे कोई साधारण रिवॉर्ड दें (जैसे कॉइन्स या फूड)
            GiveConsolationPrize();
        }

        SaveGachaData();
    }

    private void AwardGrandPrize(string winMessage)
    {
        Debug.LogWarning(winMessage);
        
        // प्लेयर की इन्वेंटरी में रेयर आइटम जोड़ें
        // InventoryManager.Instance.AddRareItem("SKIN_MYSTIC_DRAGO");

        // जैकपॉट मिलने के बाद प्लेयर का लक वापस 0 पर रीसेट करें ताकि वह फिर से स्पिन के लिए रीचार्ज करे
        playerLuckData.currentLuckPoints = 0;
        playerLuckData.grandPrizeWon = true;
    }

    private void GiveConsolationPrize()
    {
        Debug.Log($"[Gacha] साधारण इनाम मिला। अगला स्पिन करें! वर्तमान लक: {playerLuckData.currentLuckPoints}/{playerLuckData.maxPityThreshold}");
        
        EconomyManager economy = GameServiceLocator.Instance?.GetService<EconomyManager>();
        if (economy != null)
        {
            economy.EarnGold(50); // सिर्फ 50 डमी कॉइन्स
        }
    }

    // ---- डेटा सेव और लोड ----
    private void SaveGachaData()
    {
        string json = JsonUtility.ToJson(playerLuckData);
        PlayerPrefs.SetString("GachaLuckData", json);
        PlayerPrefs.Save();
    }
}
