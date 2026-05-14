using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BGMILobbyManager : MonoBehaviour
{
    [Header("Core Systems Reference")]
    public UserProfileManager profileSystem;
    public VehicleManager vehicleSystem;
    public MatchmakingServer matchmakingSystem;
    public LeaderboardManager leaderboardSystem; // Connected Leaderboard

    [Header("BGMI Top-Left Profile Panel")]
    public Text playerLobbyNameText;
    public Text playerLevelText;
    public Text currentTierTitleText; 
    public Image avatarIcon;

    [Header("BGMI Top-Right Currency & Social")]
    public Text ucPremiumText;        
    public Text bpFreeText;           
    public Text popularityScoreText;  

    [Header("BGMI Bottom-Left Match Selection")]
    public Text selectedMapNameText;  
    public Text teamModeText;          
    public Button startMatchButton;
    public GameObject matchmakingPopup; 
    public Text serverTimerText;

    [Header("BGMI Interactive UI Panels (Popups)")]
    public GameObject profilePanelWindow;     // BGMI Profile Popup Screen
    public GameObject leaderboardPanelWindow; // BGMI Leaderboard Popup Screen

    [Header("Profile Panel Inside Text Fields")]
    public Text profIDText;
    public Text profLevelText;
    public Text profMatchesText;
    public Text profKDText;
    public Text profWinsText;

    [Header("3D Lobby Scene References")]
    public Transform characterSpawnPoint; 
    public Transform vehicleShowcasePoint; 

    private float lobbyTimer = 0f;

    void Start()
    {
        RenderBGMILobbyUI();
        SpawnLobby3DModels();
        
        // Shuruat mein saare extra panels closed rahenge
        if (profilePanelWindow != null) profilePanelWindow.SetActive(false);
        if (leaderboardPanelWindow != null) leaderboardPanelWindow.SetActive(false);
    }

    void Update()
    {
        if (matchmakingSystem != null && matchmakingSystem.isServerSearching)
        {
            if (!matchmakingPopup.activeSelf) matchmakingPopup.SetActive(true);
            lobbyTimer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(lobbyTimer / 60f);
            int seconds = Mathf.FloorToInt(lobbyTimer % 60f);
            serverTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            if (matchmakingPopup.activeSelf) matchmakingPopup.SetActive(false);
            lobbyTimer = 0f;
        }
    }

    public void RenderBGMILobbyUI()
    {
        if (profileSystem != null)
        {
            playerLobbyNameText.text = profileSystem.myProfile.playerName;
            playerLevelText.text = "Lv." + profileSystem.myProfile.playerLevel;
            currentTierTitleText.text = profileSystem.myProfile.currentTier;
            popularityScoreText.text = "🔥 " + profileSystem.myProfile.popularityCount.ToString();

            ucPremiumText.text = PlayerPrefs.GetInt("PlayerGems", 0).ToString();
            bpFreeText.text = PlayerPrefs.GetInt("PlayerCoins", 1000).ToString();
            
            selectedMapNameText.text = "Erangel"; 
            teamModeText.text = "TPP - SQUAD";
        }
    }

    void SpawnLobby3DModels()
    {
        Debug.Log("Spawning active Character Model at main center stage...");
        if (vehicleSystem != null && vehicleSystem.spawnableVehicles.Count > 0)
        {
            string luxuryCar = vehicleSystem.spawnableVehicles[0].vehicleName;
            Debug.Log("BGMI Showcase Active: Displaying " + luxuryCar + " in background.");
        }
    }

    // ==========================================
    // NEW: PROFILE & LEADERBOARD CONNECTION LOGIC
    // ==========================================

    // 1. Profile Avatar par click karne par ye chalega
    public void OpenProfilePanel()
    {
        if (profileSystem != null && profilePanelWindow != null)
        {
            profilePanelWindow.SetActive(true);
            
            // Profile data ko popup window ke andar transfer karna
            profIDText.text = "Character ID: " + profileSystem.myProfile.characterID;
            profLevelText.text = "Evo Level: " + profileSystem.myProfile.evoLevel;
            profMatchesText.text = "Matches: " + profileSystem.myProfile.matchesPlayed;
            profKDText.text = "K/D Ratio: " + profileSystem.myProfile.kdRatio;
            profWinsText.text = "Wins: " + profileSystem.myProfile.totalWins;

            Debug.Log("BGMI Personal Statistics Card Opened.");
        }
    }

    public void CloseProfilePanel()
    {
        if (profilePanelWindow != null) profilePanelWindow.SetActive(false);
    }

    // 2. Leaderboard Button par click karne par ye chalega
    public void OpenLeaderboardPanel()
    {
        if (leaderboardPanelWindow != null)
        {
            leaderboardPanelWindow.SetActive(true);
            
            if (leaderboardSystem != null)
            {
                leaderboardSystem.RefreshLeaderboardData(); // Console aur UI mein data refresh
            }
            Debug.Log("Global Leaderboard Rank List Opened.");
        }
    }

    public void CloseLeaderboardPanel()
    {
        if (leaderboardPanelWindow != null) leaderboardPanelWindow.SetActive(false);
    }

    public void OnBGMIStartButtonPressed()
    {
        if (matchmakingSystem != null && profileSystem != null)
        {
            matchmakingSystem.JoinMatchmakingQueue(
                profileSystem.myProfile.characterID,
                profileSystem.myProfile.playerName,
                1500, 
                "Carlo", 
                "Default Luxury"
            );
        }
    }
}
