using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Currency Settings")]
    public int playerGems;      // Luxury Currency (like Diamonds)
    public int playerCoins;     // Standard Currency (Battle Points)

    [Header("Luxury Items")]
    public string[] ownedGunSkins;

    void Start()
    {
        // Load player data from local storage
        playerGems = PlayerPrefs.GetInt("PlayerGems", 0);
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 100);
    }

    public void AddGems(int amount)
    {
        playerGems += amount;
        PlayerPrefs.SetInt("PlayerGems", playerGems);
        Debug.Log("Gems Updated: " + playerGems);
    }

    public bool PurchaseItem(int cost, bool isGemPurchase)
    {
        if (isGemPurchase && playerGems >= cost)
        {
            playerGems -= cost;
            return true;
        }
        else if (!isGemPurchase && playerCoins >= cost)
        {
            playerCoins -= cost;
            return true;
        }
        
        Debug.Log("Insufficient Balance!");
        return false;
    }
}
