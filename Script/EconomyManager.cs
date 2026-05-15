using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Currency Vault")]
    public int playerGems;      // Premium Currency (Diamonds)
    public int playerCoins;     // Free Fire Style Gold Coins (Earnable)

    void Start()
    {
        // Save kiya hua data load karna
        playerGems = PlayerPrefs.GetInt("PlayerGems", 0);
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 1000); // New players ko 1000 Gold Free
    }

    // ==========================================
    // 1. GOLD EARNING METHODS (Gold Kamane Ke Tarike)
    // ==========================================
    
    // Match khatam hone par gold milna (Based on Kills & Survival)
    public void EarnGoldFromMatch(int kills, int survivalMinutes)
    {
        int goldEarned = (kills * 50) + (survivalMinutes * 10);
        
        // Free Fire limit: Ek match me max 300 gold mil sakta hai
        if (goldEarned > 300) goldEarned = 300;

        playerCoins += goldEarned;
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
        Debug.Log("MATCH REWARD: You earned " + goldEarned + " Gold Coins!");
    }

    // Daily Login par free gold milna
    public void ClaimDailyLoginGold()
    {
        int dailyReward = 150;
        playerCoins += dailyReward;
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
        Debug.Log("DAILY REWARD: 150 Gold Coins Added!");
    }

    // ==========================================
    // 2. GOLD SPENDING METHODS (Gold Kharch Karne Ke Tarike)
    // ==========================================
    
    // Gold use karke buy karne ka core logic
    public bool SpendGold(int amount)
    {
        if (playerCoins >= amount)
        {
            playerCoins -= amount;
            PlayerPrefs.SetInt("PlayerCoins", playerCoins);
            Debug.Log("Purchase Successful! Remaining Gold: " + playerCoins);
            return true;
        }
        
        Debug.Log("Not enough Gold Coins! Khelte raho aur Gold kamao.");
        return false;
    }

    // Gems/Diamonds add karne ke liye function
    public void AddGems(int amount)
    {
        playerGems += amount;
        PlayerPrefs.SetInt("PlayerGems", playerGems);
    }

    // Premium Purchase handler
    public bool PurchaseItem(int cost, bool isGemPurchase)
    {
        if (isGemPurchase && playerGems >= cost)
        {
            playerGems -= cost;
            PlayerPrefs.SetInt("PlayerGems", playerGems);
            return true;
        }
        else if (!isGemPurchase && playerCoins >= cost)
        {
            playerCoins -= cost;
            PlayerPrefs.SetInt("PlayerCoins", playerCoins);
            return true;
        }
        return false;
    }
    EconomyManager economy = GameServiceLocator.Instance.GetService<EconomyManager>();
// Do your logic with economy here
