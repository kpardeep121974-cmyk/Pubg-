using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InGameEvent
{
    public string eventName;
    public string description;
    public int requiredKills;
    public int currentKills;
    public int rewardCoins;
    public bool isCompleted;
}

[System.Serializable]
public class GoldRushItem
{
    public string itemName;
    public string itemRarity; // "Grand Prize", "Rare", "Common"
    public bool isPermanent;
}

public class EventManager : MonoBehaviour
{
    public List<InGameEvent> activeEvents = new List<InGameEvent>();
    public List<GoldRushItem> goldRushPrizePool = new List<GoldRushItem>();
    
    public EconomyManager economy;      // Gems aur Coins ke liye reference
    public InventoryManager inventory;  // Unlocked items ko vault me bhejne ke liye

    void Start()
    {
        InitializeEvents();
        InitializeGoldRush();
    }

    // 1. Free Fire Style Copy Events (Daily Missions)
    void InitializeEvents()
    {
        activeEvents.Add(new InGameEvent { 
            eventName = "Booyah Challenge", 
            description = "Get 5 kills in Erangel map using AR weapons.", 
            requiredKills = 5, 
            currentKills = 0, 
            rewardCoins = 200, 
            isCompleted = false 
        });

        activeEvents.Add(new InGameEvent { 
            eventName = "Weekend Warrior", 
            description = "Defeat 10 enemies with any SMG in custom rooms.", 
            requiredKills = 10, 
            currentKills = 0, 
            rewardCoins = 500, 
            isCompleted = false 
        });
    }

    // 2. Gold Rush (Gold Royale Spin System)
    void InitializeGoldRush()
    {
        // Free Fire style luck royale prize pool
        goldRushPrizePool.Add(new GoldRushItem { itemName = "Imperial Gold AK47 Skin", itemRarity = "Grand Prize", isPermanent = true });
        goldRushPrizePool.Add(new GoldRushItem { itemName = "Luxury Streetwear Jacket", itemRarity = "Rare", isPermanent = true });
        goldRushPrizePool.Add(new GoldRushItem { itemName = "Gold Royale Shoes", itemRarity = "Common", isPermanent = false });
        goldRushPrizePool.Add(new GoldRushItem { itemName = "100 Bonus Coins", itemRarity = "Common", isPermanent = false });
    }

    // Player jab 1 Spin karega (Cost: 300 Gold Coins)
    public void SpinGoldRush()
    {
        int spinCost = 300;

        if (economy.playerCoins >= spinCost)
        {
            economy.playerCoins -= spinCost; // Gold coins deduct honge
            PlayerPrefs.SetInt("PlayerCoins", economy.playerCoins);

            // Random prize select karna pool se
            int randomIndex = Random.Range(0, goldRushPrizePool.Count);
            GoldRushItem wonItem = goldRushPrizePool[randomIndex];

            Debug.Log("GOLD RUSH SPIN SUCCESS! You won: " + wonItem.itemName + " [" + wonItem.itemRarity + "]");

            // Agar gun skin nikli toh weapon inventory me add ho jayegi
            if (wonItem.itemName.Contains("Skin"))
            {
                inventory.EquipWeaponSkin("AK47", wonItem.itemName);
            }
        }
        else
        {
            Debug.Log("Not enough Gold Coins to spin Gold Rush Royale!");
        }
    }

    // Mission progress update karne ka function
    public void TrackEventProgress(int killsAdded)
    {
        foreach (var ev in activeEvents)
        {
            if (!ev.isCompleted)
            {
                ev.currentKills += killsAdded;
                if (ev.currentKills >= ev.requiredKills)
                {
                    ev.isCompleted = true;
                    economy.AddGems(0); // Reward mein coins dene ke liye system trigger
                    economy.playerCoins += ev.rewardCoins;
                    PlayerPrefs.SetInt("PlayerCoins", economy.playerCoins);
                    Debug.Log("Event Completed: " + ev.eventName + "! Claimed " + ev.rewardCoins + " Coins.");
                }
            }
        }
    }
}
