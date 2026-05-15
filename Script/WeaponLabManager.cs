using UnityEngine;

public class WeaponLabManager : MonoBehaviour
{
    [System.Serializable]
    public class UpgradableSkin
    {
        public string skinID;
        public string weaponName;
        public int currentSkinLevel = 1;
        public int maxLevel = 7;

        [Header("Level Features Unlock Status")]
        public bool hasKillEffect = false;   // Level 2
        public bool hasKillMessage = false;  // Level 4 (Kill Feed Customization)
        public bool hasFinalForm = false;    // Level 7 (Ultimate Look)
    }

    [Header("Materials Required For Upgrade")]
    public int upgradeMaterialsOwned = 0; // विशेष इन-गेम टोकन जो सिर्फ क्रेट्स से मिलते हैं

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<WeaponLabManager>(this);
        }
    }

    // गन स्किन को अपग्रेड करने का कमर्शियल लॉजिक
    public void UpgradeWeaponSkin(UpgradableSkin skin)
    {
        if (skin.currentSkinLevel >= skin.maxLevel)
        {
            Debug.Log("[Weapon Lab] गन पहले से ही मैक्स लेवल पर है!");
            return;
        }

        // हर लेवल के लिए आवश्यक मटेरियल की गणना (लेवल बढ़ने पर खर्च बढ़ेगा)
        int requiredMaterials = skin.currentSkinLevel * 2; 

        if (upgradeMaterialsOwned >= requiredMaterials)
        {
            upgradeMaterialsOwned -= requiredMaterials;
            skin.currentSkinLevel++;

            // फीचर्स अनलॉक करें
            EvaluateUnlockedFeatures(skin);
            
            Debug.LogWarning($"🎉 [WEAPON LAB] आपकी {skin.weaponName} स्किन लेवल {skin.currentSkinLevel} पर अपग्रेड हो गई है!");
            SaveLabData();
        }
        else
        {
            Debug.LogError($"[Weapon Lab] अपग्रेड के लिए पर्याप्त मटेरियल्स नहीं हैं! जरूरत है: {requiredMaterials}, आपके पास हैं: {upgradeMaterialsOwned}");
            // यहाँ प्लेयर को स्टोर/क्रेट ओपनिंग स्क्रीन पर रीडायरेक्ट करने का UI ट्रिगर करें
        }
    }

    private void EvaluateUnlockedFeatures(UpgradableSkin skin)
    {
        if (skin.currentSkinLevel >= 2) skin.hasKillEffect = true;
        if (skin.currentSkinLevel >= 4) skin.hasKillMessage = true;
        if (skin.currentSkinLevel >= 7) skin.hasFinalForm = true;

        // जब मैच में किल होगा, तो यह किल फीड मैनेजर (`HUDManager`) को स्पेशल इफ़ेक्ट भेजने के लिए ट्रिगर करेगा
    }

    private void SaveLabData()
    {
        PlayerPrefs.SetInt("UpgradeMaterials", upgradeMaterialsOwned);
        PlayerPrefs.Save();
    }
}
