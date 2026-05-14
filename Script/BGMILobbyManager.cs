using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BGMILobbyManager : MonoBehaviour
{
    [Header("Core Core Systems Reference")]
    public UserProfileManager profileSystem;
    public VehicleManager vehicleSystem;
    public MatchmakingServer matchmakingSystem;

    [Header("BGMI Top-Left Profile Panel")]
    public Text playerLobbyNameText;
    public Text playerLevelText;
    public Text currentTierTitleText; // e.g., "Conqueror" ya "Ace"
    public Image avatarIcon;

    [Header("BGMI Top-Right Currency & Social")]
    public Text ucPremiumText;        // Premium currency (UC/Gems)
    public Text bpFreeText;           // Free currency (BP/Coins)
    public Text popularityScoreText;  // Total popularity points

    [Header("BGMI Bottom-Left Match Selection")]
    public Text selectedMapNameText;  // e.g., "Erangel", "Sanhok"
    public Text teamModeText;          // "Solo", "Duo", "Squad"
    public Button startMatchButton;
    public GameObject matchmakingPopup; // Center matchmaking timer ticker
    public Text serverTimerText;

    [Header("BGMI 3D Lobby Scene References")]
    public Transform characterSpawnPoint; // Jahan active character khada hoga
    public Transform vehicleShowcasePoint; // Jahan piche UAZ ya Dacia khadi hogi

    private float lobbyTimer = 0f;

    void Start()
    {
        RenderBGMILobbyUI();
        SpawnLobby3DModels();
    }

    void Update()
    {
        // BGMI Style Center Matching Ticker
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

    // 1. Pure BGMI Layout Parameters ko Screen par Load karna
    public void RenderBGMILobbyUI()
    {
        if (profileSystem != null)
        {
            // BGMI Identity Sync
            playerLobbyNameText.text = profileSystem.myProfile.playerName;
            playerLevelText.text = "Lv." + profileSystem.myProfile.playerLevel;
            currentTierTitleText.text = profileSystem.myProfile.currentTier;
            popularityScoreText.text = "🔥 " + profileSystem.myProfile.popularityCount.ToString();

            // BGMI Currency Nomenclature (UC aur BP setup)
            ucPremiumText.text = PlayerPrefs.GetInt("PlayerGems", 0).ToString();
            bpFreeText.text = PlayerPrefs.GetInt("PlayerCoins", 1000).ToString();
            
            // Map Setup Display
            selectedMapNameText.text = "Erangel"; 
            teamModeText.text = "TPP - SQUAD";
            
            Debug.Log("BGMI Cinematic Lobby UI Canvas Rendered Successfully!");
        }
    }

    // 2. 3D Environment Showcase (Character + Gaadi Backdrop)
    void SpawnLobby3DModels()
    {
        Debug.Log("Spawning active Character Model at main center stage...");
        
        // Checking if vehicle system has an active vehicle to flex in background
        if (vehicleSystem != null && vehicleSystem.spawnableVehicles.Count > 0)
        {
            string luxuryCar = vehicleSystem.spawnableVehicles[0].vehicleName; // Default: UAZ
            Debug.Log("BGMI Showcase Active: Displaying " + luxuryCar + " in the background parking slot.");
        }
    }

    // 3. BGMI Yellow 'START' Button Trigger
    public void OnBGMIStartButtonPressed()
    {
        if (matchmakingSystem != null && profileSystem != null)
        {
            Debug.Log("BGMI Matchmaking Initiated: Connecting to regional matchmaking grids...");
            matchmakingSystem.JoinMatchmakingQueue(
                profileSystem.myProfile.characterID,
                profileSystem.myProfile.playerName,
                1500, // Simulated matchmaking rank rating
                "Carlo", // Active tactical character
                "Default Luxury"
            );
        }
    }
}
