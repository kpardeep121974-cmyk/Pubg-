using UnityEngine;

[System.Serializable]
public class BGMIProfileData
{
    [Header("Basic Information")]
    public string playerName;
    public string characterID;       // 10-digit unique BGMI style ID
    public int playerLevel = 1;
    public int evoLevel = 1;
    public int achievementPoints = 0;
    public long popularityCount = 0; // Popularity jo dost bhejte hain

    [Header("Current Season Stats")]
    public string currentTier = "Bronze V";
    public int matchesPlayed = 0;
    public int totalWins = 0;
    public int totalKills = 0;
    public float kdRatio = 0.0f;     // Kill/Death Ratio
    public int mvpCount = 0;         // Kitni baar MVP bana
}

public class UserProfileManager : MonoBehaviour
{
    public BGMIProfileData myProfile = new BGMIProfileData();

    void Start()
    {
        LoadUserProfile();
    }

    // 1. Player ka BGMI Style Profile Data Load Karna
    public void LoadUserProfile()
    {
        myProfile.playerName = PlayerPrefs.GetString("PlayerName", "Guest_Player");
        myProfile.characterID = PlayerPrefs.GetString("CharacterID", GenerateRandomUID());
        myProfile.playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        myProfile.evoLevel = PlayerPrefs.GetInt("EvoLevel", 1);
        myProfile.achievementPoints = PlayerPrefs.GetInt("AchievementPoints", 150);
        myProfile.popularityCount = System.Convert.ToInt64(PlayerPrefs.GetString("Popularity", "0"));

        // Stats Load
        myProfile.matchesPlayed = PlayerPrefs.GetInt("MatchesPlayed", 0);
        myProfile.totalWins = PlayerPrefs.GetInt("TotalWins", 0);
        myProfile.totalKills = PlayerPrefs.GetInt("TotalKills", 0);
        myProfile.currentTier = PlayerPrefs.GetString("CurrentTier", "Bronze V");
        myProfile.mvpCount = PlayerPrefs.GetInt("MVPCount", 0);

        CalculateKDRatio();
        
        Debug.Log("=========================================");
        Debug.Log("BGMI STYLE PROFILE LOADED SUCCESSFULLY!");
        Debug.Log("Name: " + myProfile.playerName + " | ID: " + myProfile.characterID);
        Debug.Log("Tier: " + myProfile.currentTier + " | K/D Ratio: " + myProfile.kdRatio);
        Debug.Log("=========================================");
    }

    // 2. Match ke baad Stats Update karne ka Automatic Logic
    public void UpdateStatsAfterMatch(int kills, bool didWin, bool isMVP)
    {
        myProfile.matchesPlayed += 1;
        myProfile.totalKills += kills;
        
        if (didWin) myProfile.totalWins += 1;
        if (isMVP) myProfile.mvpCount += 1;

        // Level up simulation (Har 5 matches baad level up)
        if (myProfile.matchesPlayed % 5 == 0)
        {
            myProfile.playerLevel += 1;
            myProfile.achievementPoints += 20; // Achievement unlock reward
        }

        CalculateKDRatio();
        SaveProfileData();
    }

    // 3. K/D Ratio Calculate karne ka formula
    void CalculateKDRatio()
    {
        int deaths = myProfile.matchesPlayed - myProfile.totalWins;
        if (deaths <= 0) deaths = 1; // Agar zero death ho toh division error se bachne ke liye

        myProfile.kdRatio = (float)myProfile.totalKills / deaths;
        // Float ko 2 decimal tak round karne ke liye
        myProfile.kdRatio = Mathf.Round(myProfile.kdRatio * 100f) / 100f;
    }

    // 4. Data Save karne ka system
    void SaveProfileData()
    {
        PlayerPrefs.SetString("PlayerName", myProfile.playerName);
        PlayerPrefs.SetString("CharacterID", myProfile.characterID);
        PlayerPrefs.SetInt("PlayerLevel", myProfile.playerLevel);
        PlayerPrefs.SetInt("EvoLevel", myProfile.evoLevel);
        PlayerPrefs.SetInt("AchievementPoints", myProfile.achievementPoints);
        PlayerPrefs.SetString("Popularity", myProfile.popularityCount.ToString());
        PlayerPrefs.SetInt("MatchesPlayed", myProfile.matchesPlayed);
        PlayerPrefs.SetInt("TotalWins", myProfile.totalWins);
        PlayerPrefs.SetInt("TotalKills", myProfile.totalKills);
        PlayerPrefs.SetInt("MVPCount", myProfile.mvpCount);
        PlayerPrefs.Save();
    }

    // Pehli baar entry par random 10-digit ID banana
    string GenerateRandomUID()
    {
        string uid = "";
        for (int i = 0; i < 10; i++)
        {
            uid += Random.Range(0, 10).ToString();
        }
        PlayerPrefs.SetString("CharacterID", uid);
        return uid;
    }

    // Doston se Popularity milne par function trigger hona
    public void ReceivePopularity(int amount)
    {
        myProfile.popularityCount += amount;
        PlayerPrefs.SetString("Popularity", myProfile.popularityCount.ToString());
        Debug.Log("Awesome! Received " + amount + " Popularity points from a friend.");
    }
}
