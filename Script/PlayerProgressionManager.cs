using UnityEngine;

public class PlayerProgressionManager : MonoBehaviour
{
    [Header("Player Stats")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpRequiredForNextLevel = 1000;

    private void Awake()
    {
        // इसे central Service Locator में रजिस्टर करें
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<PlayerProgressionManager>(this);
        }
    }

    void Start()
    {
        // लोकल डेटा लोड करना (बाद में इसे क्लाउड/बैकएंड से बदल सकते हैं)
        LoadProgressionData();
    }

    // जब कोई मैच खत्म होता है, तो इस मेथड को कॉल किया जाएगा
    public void AddMatchRewards(int kills, float survivalTimeInSeconds)
    {
        // XP कैलकुलेशन का फॉर्मूला (जैसे: हर किल पर 50 XP और हर सेकंड सर्वाइव करने पर 1 XP)
        int xpGained = (kills * 50) + Mathf.RoundToInt(survivalTimeInSeconds * 1f);
        
        Debug.Log($"[Progression] मैच खत्म! आपको मिला: {xpGained} XP");
        AddXP(xpGained);
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        // चेक करें कि क्या प्लेयर का लेवल अप हुआ है (यह लूप तब तक चलेगा जब तक XP लिमिट के अंदर न आ जाए)
        while (currentXP >= xpRequiredForNextLevel)
        {
            LevelUp();
        }

        SaveProgressionData();
    }

    private void LevelUp()
    {
        currentXP -= xpRequiredForNextLevel;
        currentLevel++;
        
        // अगले लेवल के लिए डिफिकल्टी बढ़ाएं (हर लेवल पर 15% ज्यादा XP की जरूरत होगी)
        xpRequiredForNextLevel = Mathf.RoundToInt(xpRequiredForNextLevel * 1.15f);

        Debug.LogWarning($"[LEVEL UP] बधाई हो! आपका लेवल अब {currentLevel} हो गया है!");

        // प्लेयर को लेवल अप बोनस कॉइन्स देना (हमारे EconomyManager का इस्तेमाल करके)
        EconomyManager economy = GameServiceLocator.Instance?.GetService<EconomyManager>();
        if (economy != null)
        {
            economy.EarnGold(500); // लेवल अप होने पर 500 गोल्ड फ्री
        }
    }

    private void SaveProgressionData()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerXP", currentXP);
        PlayerPrefs.SetInt("XPRequired", xpRequiredForNextLevel);
        PlayerPrefs.Save();
    }

    private void LoadProgressionData()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentXP = PlayerPrefs.GetInt("PlayerXP", 0);
        xpRequiredForNextLevel = PlayerPrefs.GetInt("XPRequired", 1000);
    }
}
