using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreItem
{
    public int itemID;
    public string itemName;
    public string itemCategory; // "RoomCard", "PetFood", "WeaponCrate"
    public int gemCost;          // UC / Diamonds Price
    public int coinCost;         // BP / Gold Coins Price
    public string description;
}

public class InventoryStoreManager : MonoBehaviour
{
    [Header("Connected Core Systems")]
    public EconomyManager economySystem;
    public WeapensSkinManager weaponSkinSystem;

    [Header("Shop Inventory Database")]
    public List<StoreItem> shopItemsList = new List<StoreItem>();

    void Start()
    {
        InitializeShopProducts();
    }

    // 1. Store ke saare products register karna (BGMI + FF Mix Items)
    void InitializeShopProducts()
    {
        // Custom Room Cards (Most Important for CustomRoomManager)
        shopItemsList.Add(new StoreItem { itemID = 501, itemName = "1-Day Custom Room Card", itemCategory = "RoomCard", gemCost = 100, coinCost = 0, description = "Unlocks unlimited custom rooms for 24 hours." });
        shopItemsList.Add(new StoreItem { itemID = 502, itemName = "7-Day Custom Room Card", itemCategory = "RoomCard", gemCost = 500, coinCost = 0, description = "Unlocks unlimited custom rooms for 7 days." });

        // Pet Foods (For upgrading your 50 Pets)
        shopItemsList.Add(new StoreItem { itemID = 601, itemName = "Premium Pet Food", itemCategory = "PetFood", gemCost = 20, coinCost = 2000, description = "Increases pet XP and unlocks new animations." });

        // Tactical Weapon Crates (For EVO Gun Skins)
        shopItemsList.Add(new StoreItem { itemID = 701, itemName = "M416 Glacier Crate", itemCategory = "WeaponCrate", gemCost = 60, coinCost = 0, description = "Chance to get the legendary upgrading M416 skin." });
        shopItemsList.Add(new StoreItem { itemID = 702, itemName = "AK47 Blue Flame Crate", itemCategory = "WeaponCrate", gemCost = 60, coinCost = 0, description = "Contains materials to upgrade Free Fire EVO guns." });
    }

    // 2. Item Purchase Logic (Gold ya Gems validation ke saath)
    public void BuyStoreItem(int itemID, bool buyWithGems)
    {
        if (economySystem == null) return;

        foreach (var item in shopItemsList)
        {
            if (item.itemID == itemID)
            {
                if (buyWithGems)
                {
                    // Premium Currency Check (UC/Diamonds)
                    if (economySystem.PurchaseItem(item.gemCost, true))
                    {
                        DeliverPurchasedAsset(item);
                    }
                }
                else
                {
                    // Free Currency Check (BP/Gold)
                    if (economySystem.SpendGold(item.coinCost))
                    {
                        DeliverPurchasedAsset(item);
                    }
                }
                return;
            }
        }
        Debug.Log("❌ Item ID not found in Store database.");
    }

    // 3. Purchase ke baad item ko Player ke data mein add karna
    void DeliverPurchasedAsset(StoreItem item)
    {
        Debug.Log("🛍️ PURCHASE SUCCESS: You bought " + item.itemName);

        if (item.itemCategory == "RoomCard")
        {
            // Room card ka count local storage me badhana taaki CustomRoomManager check kar sake
            int currentCards = PlayerPrefs.GetInt("CustomRoomCards", 0);
            currentCards++;
            PlayerPrefs.SetInt("CustomRoomCards", currentCards);
            PlayerPrefs.Save();
            Debug.Log("🏠 Custom Room Card added to inventory. Total owned: " + currentCards);
        }
        else if (item.itemCategory == "PetFood")
        {
            int currentFood = PlayerPrefs.GetInt("PetFoodCount", 0);
            currentFood++;
            PlayerPrefs.SetInt("PetFoodCount", currentFood);
            PlayerPrefs.Save();
            Debug.Log("🍖 Pet Food added. Ready to feed your companions.");
        }
        else if (item.itemCategory == "WeaponCrate" && weaponSkinSystem != null)
        {
            // Loot crate open karne ka simulation
            Debug.Log("📦 Opening weapon crate... Syncing skin rewards with database.");
            weaponSkinSystem.UnlockNewSkin(item.itemName);
        }
    }
}
