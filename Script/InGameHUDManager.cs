using UnityEngine;
using UnityEngine.UI;

public class InGameHUDManager : MonoBehaviour
{
    [Header("Health UI")]
    public Slider healthBar;
    public Text healthText;

    [Header("Weapon & Ammo UI")]
    public Text weaponNameText;
    public Text ammoText;

    [Header("Match Stats")]
    public Text alivePlayersText;
    public Text killCountText;
    
    [Header("Kill Feed Panel")]
    public GameObject killFeedPrefab;
    public Transform killFeedContainer;

    private void Awake()
    {
        // Register this UI Manager to the central Service Locator
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<InGameHUDManager>(this);
        }
    }

    // Call this whenever the player's health changes
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthBar != null) healthBar.value = currentHealth / maxHealth;
        if (healthText != null) healthText.text = $"{Mathf.RoundToInt(currentHealth)} / {maxHealth}";
    }

    // Call this whenever the player switches weapons or shoots
    public void UpdateAmmoUI(string weaponName, int currentAmmo, int reservedAmmo)
    {
        if (weaponNameText != null) weaponNameText.text = weaponName;
        if (ammoText != null) ammoText.text = $"{currentAmmo} / {reservedAmmo}";
    }

    // Call this to update general match stats
    public void UpdateMatchStats(int aliveCount, int localKills)
    {
        if (alivePlayersText != null) alivePlayersText.text = $"ALIVE: {aliveCount}";
        if (killCountText != null) killCountText.text = $"KILLS: {localKills}";
    }

    // Call this to push a new message to the top-right kill feed (e.g., "Player1 knocked Player2")
    public void AddToKillFeed(string killer, string victim, string weaponUsed)
    {
        if (killFeedPrefab == null || killFeedContainer == null) return;

        GameObject feedItem = Instantiate(killFeedPrefab, killFeedContainer);
        Text feedText = feedItem.GetComponentInChildren<Text>();
        
        if (feedText != null)
        {
            feedText.text = $"{killer} [{weaponUsed}] {victim}";
        }

        // Auto destroy the notification after 4 seconds to keep the screen clean
        Destroy(feedItem, 4f);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class InGameHUDManager : MonoBehaviour
{
    [Header("Health UI")]
    public Slider healthBar;
    public Text healthText;

    [Header("Weapon & Ammo UI")]
    public Text weaponNameText;
    public Text ammoText;

    [Header("Match Stats")]
    public Text alivePlayersText;
    public Text killCountText;
    
    [Header("Kill Feed Panel")]
    public GameObject killFeedPrefab;
    public Transform killFeedContainer;

    private void Awake()
    {
        // Register this UI Manager to the central Service Locator
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<InGameHUDManager>(this);
        }
    }

    // Call this whenever the player's health changes
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthBar != null) healthBar.value = currentHealth / maxHealth;
        if (healthText != null) healthText.text = $"{Mathf.RoundToInt(currentHealth)} / {maxHealth}";
    }

    // Call this whenever the player switches weapons or shoots
    public void UpdateAmmoUI(string weaponName, int currentAmmo, int reservedAmmo)
    {
        if (weaponNameText != null) weaponNameText.text = weaponName;
        if (ammoText != null) ammoText.text = $"{currentAmmo} / {reservedAmmo}";
    }

    // Call this to update general match stats
    public void UpdateMatchStats(int aliveCount, int localKills)
    {
        if (alivePlayersText != null) alivePlayersText.text = $"ALIVE: {aliveCount}";
        if (killCountText != null) killCountText.text = $"KILLS: {localKills}";
    }

    // Call this to push a new message to the top-right kill feed (e.g., "Player1 knocked Player2")
    public void AddToKillFeed(string killer, string victim, string weaponUsed)
    {
        if (killFeedPrefab == null || killFeedContainer == null) return;

        GameObject feedItem = Instantiate(killFeedPrefab, killFeedContainer);
        Text feedText = feedItem.GetComponentInChildren<Text>();
        
        if (feedText != null)
        {
            feedText.text = $"{killer} [{weaponUsed}] {victim}";
        }

        // Auto destroy the notification after 4 seconds to keep the screen clean
        Destroy(feedItem, 4f);
    }
}
