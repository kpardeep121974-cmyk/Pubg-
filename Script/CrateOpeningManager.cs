using System.Collections.Generic;
using UnityEngine;

public class CrateOpeningManager : MonoBehaviour
{
    public enum Rarity { Common, Rare, Epic, Legendary }

    [System.Serializable]
    public class CrateItem
    {
        public string itemName;
        public Rarity rarity;
        public GameObject itemSkinPrefab; // प्लेयर को मिलने वाला इनाम
    }

    [Header("Crate Price")]
    public int crateCostGems = 60; // एक क्रेट खोलने की कीमत (60 UC/Gems)

    [Header("Loot Pool")]
    public List<CrateItem> crateItemsPool = new List<CrateItem>();

    private void Awake()
    {
        // इसे central Service Locator में रजिस्टर करें
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<CrateOpeningManager>(this);
        }
    }

    // UI बटन पर क्लिक होने पर इस मेथड को कॉल करें
    public void OpenCrate()
    {
        EconomyManager economy = GameServiceLocator.Instance?.GetService<EconomyManager>();

        if (economy == null)
        {
            Debug.LogError("EconomyManager नहीं मिला!");
            return;
        }

        // 1. चेक करें कि प्लेयर के पास पर्याप्त जेम्स (UC) हैं या नहीं
        if (economy.playerGems >= crateCostGems)
        {
            // जेम्स काटें (आप अपने EconomyManager में SpendGems का मेथड बना सकते हैं)
            economy.playerGems -= crateCostGems; 
            PlayerPrefs.SetInt("PlayerGems", economy.playerGems);

            // 2. क्रेट खोलकर इनाम तय करें
            CrateItem wonItem = RollCrateItem();
            Debug.LogWarning($"[CRATE SUCCESS] आपको मिला: {wonItem.itemName} ({wonItem.rarity})");

            // 3. इस आइटम को प्लेयर की परमानेंट इन्वेंटरी में जोड़ें
            DeliverItemToInventory(wonItem);
        }
        else
        {
            Debug.LogWarning("क्रेट खोलने के लिए पर्याप्त Gems (UC) नहीं हैं! टॉप-अप करें।");
        }
    }

    private CrateItem RollCrateItem()
    {
        // भाग्य (Luck) का फैसला करने के लिए 0 से 100 के बीच एक रैंडम नंबर चुनें
        float roll = Random.Range(0f, 100f);

        // संभावना की दर (Probability Rates):
        // Common: 70%, Rare: 20%, Epic: 8%, Legendary: 2%
        Rarity selectedRarity = Rarity.Common;

        if (roll <= 2f) selectedRarity = Rarity.Legendary;      // 2% चांस
        else if (roll <= 10f) selectedRarity = Rarity.Epic;     // 8% चांस (2 + 8)
        else if (roll <= 30f) selectedRarity = Rarity.Rare;     // 20% चांस (10 + 20)
        else selectedRarity = Rarity.Common;                     // बाकी 70% चांस

        // चुने गए Rarity वाले आइटम्स की एक अलग लिस्ट बनाएं
        List<CrateItem> matchingItems = crateItemsPool.FindAll(item => item.rarity == selectedRarity);

        // अगर उस रेरिटी का कोई आइटम पूल में नहीं है, तो सेफ्टी के लिए पहला आइटम दे दें
        if (matchingItems.Count == 0) return crateItemsPool[0];

        // उस लिस्ट में से रैंडमली एक आइटम चुनकर प्लेयर को दें
        int randomIndex = Random.Range(0, matchingItems.Count);
        return matchingItems[randomIndex];
    }

    private void DeliverItemToInventory(CrateItem item)
    {
        // आपके WeaponSkinManager में इस स्किन को हमेशा के लिए अनलॉक करने का लॉजिक
        WeaponSkinManager skinManager = GameServiceLocator.Instance?.GetService<WeaponSkinManager>();
        if (skinManager != null)
            {
            // मान लेते हैं आपके SkinManager में अनलॉक करने का मेथड है
            // skinManager.UnlockSkin(item.itemName);
            Debug.Log($"{item.itemName} आपकी इन्वेंटरी में सेव कर दिया गया है।");
        }
    }
}
