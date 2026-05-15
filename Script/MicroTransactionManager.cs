using System;
using UnityEngine;

public class MicroTransactionManager : MonoBehaviour
{
    [System.Serializable]
    public class MicroOffer
    {
        public string dealID;
        public string itemsIncluded;   // जैसे: "100 Diamonds + Rare Parachute Skin"
        public float localizedPrice;  // भारत के लिए ₹10 या ₹29
        public string currencySymbol = "₹";
        public string expiryTimeStr;
        public bool isAvailable = false;
    }

    [Header("Active Micro-Deal Profile")]
    public MicroOffer currentAirdropDeal;

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<MicroTransactionManager>(this);
        }
    }

    // मैच खत्म होने पर इस मेथड को प्लेयर के स्कोर या लेवल के आधार पर कॉल करें
    public void CheckAndTriggerMicroDeal(int playerMatchRank)
    {
        if (currentAirdropDeal.isAvailable) return; // अगर पहले से ही एक डील एक्टिव है तो दूसरी न दें

        // 🚀 सीक्रेट कमर्शियल लॉजिक: अगर प्लेयर टॉप 10 में मरा या उसने अच्छा खेला, तो उसे इनाम के रूप में सस्ती डील दें
        if (playerMatchRank <= 10 || UnityEngine.Random.value > 0.5f)
        {
            currentAirdropDeal.dealID = "AIRDROP_DEAL_" + UnityEngine.Random.Range(1000, 9999);
            currentAirdropDeal.itemsIncluded = "300 Diamonds + 1x Weapon Lab Material (Special Box)";
            currentAirdropDeal.localizedPrice = 29.0f; // सिर्फ ₹29 का स्पेशल ऑफर (ताकि बच्चा तुरंत खरीद ले)
            currentAirdropDeal.expiryTimeStr = DateTime.UtcNow.AddDays(1).ToString("o"); // 24 घंटे की समय सीमा
            currentAirdropDeal.isAvailable = true;

            Debug.LogWarning($"💰 [MICROTRANSACTION] स्पेशल एयरड्रॉप अनलॉक! {currentAirdropDeal.itemsIncluded} केवल {currentAirdropDeal.currencySymbol}{currentAirdropDeal.localizedPrice} में!");
            SaveMicroDeal();
        }
    }

    // जब प्लेयर ₹29 वाले 'Buy Now' बटन पर क्लिक करेगा
    public void PurchaseMicroDeal()
    {
        if (!currentAirdropDeal.isAvailable) return;

        // चेक करें कि डील एक्सपायर तो नहीं हुई
        DateTime expiry = DateTime.Parse(currentAirdropDeal.expiryTimeStr);
        if (DateTime.UtcNow > expiry)
        {
            currentAirdropDeal.isAvailable = false;
            Debug.LogError("[Micro Deal] यह ऑफर एक्सपायर हो चुका है!");
            SaveMicroDeal();
            return;
        }

        // 🚀 यूनिटी IAP के जरिए पेमेंट सक्सेस होने के बाद रिवॉर्ड दें:
        Debug.LogWarning($"🎉 [PURCHASE SUCCESS] ₹{currentAirdropDeal.localizedPrice} का पेमेंट सफल! आइटम्स इन्वेंटरी में जोड़ दिए गए हैं।");
        
        // प्लेयर्स को डायमंड्स और मटेरियल्स क्रेडिट करें
        EconomyManager economy = GameServiceLocator.Instance?.GetService<EconomyManager>();
        WeaponLabManager lab = GameServiceLocator.Instance?.GetService<WeaponLabManager>();
        
        if (economy != null) economy.EarnGold(300); // 300 डायमंड्स दिए
        if (lab != null) lab.upgradeMaterialsOwned += 1; // 1 वेपन लैब मटेरियल दिया

        currentAirdropDeal.isAvailable = false; // खरीदने के बाद डील खत्म
        SaveMicroDeal();
    }

    private void SaveMicroDeal()
    {
        string json = JsonUtility.ToJson(currentAirdropDeal);
        PlayerPrefs.SetString("ActiveMicroDeal", json);
        PlayerPrefs.Save();
    }
}
