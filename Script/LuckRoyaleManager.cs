using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoyaleReward
{
    public string rewardName;
    public string rarity;       // "Legendary", "Epic", "Rare", "Common"
    public string rewardType;   // "WeaponSkin", "OutfitBundle"
}

public class LuckRoyaleManager : MonoBehaviour
{
    [Header("Connected Core Systems")]
    public EconomyManager economySystem;
    public WeapensSkinManager weaponSkinSystem; // To unlock weapon skins directly

    [Header("Royale Prize Pools")]
    public List<RoyaleReward> weaponRoyalePool = new List<RoyaleReward>();
    public List<RoyaleReward> goldRoyalePool = new List<RoyaleReward>();

    [Header("Player Luck Counter (Free Fire Style)")]
    public int weaponLuckPoints = 0;
    public int goldLuckPoints = 0;

    void Start()
    {
        InitializeRoyaleRewards();
    }

    // Prizes register karna (FF Systems + BGMI Best Bundles)
    void InitializeRoyaleRewards()
    {
        // 1. WEAPON ROYALE POOL (Free Fire Copy + Premium Weapons)
        weaponRoyalePool.Clear();
        weaponRoyalePool.Add(new RoyaleReward { rewardName = "M416 Glacier (Level 1)", rarity = "Legendary", rewardType = "WeaponSkin" });
        weaponRoyalePool.Add(new RoyaleReward { rewardName = "AK47 Blue Flame Draco", rarity = "Legendary", rewardType = "WeaponSkin" });
        weaponRoyalePool.Add(new RoyaleReward { rewardName = "AWM Mauve Avenger", rarity = "Epic", rewardType = "WeaponSkin" });
        weaponRoyalePool.Add(new RoyaleReward { rewardName = "SCAR-L Cupid", rarity = "Epic", rewardType = "WeaponSkin" });
        weaponRoyalePool.Add(new RoyaleReward { rewardName = "UMP45 Arctic Blue", rarity = "Rare", rewardType = "WeaponSkin" });

        // 2. GOLD ROYALE POOL (BGMI Best Outfits Combo)
        goldRoyalePool.Clear();
        goldRoyalePool.Add(new RoyaleReward { rewardName = "Pharaoh X-Suit Bundle", rarity = "Legendary", rewardType = "OutfitBundle" });
        goldRoyalePool.Add(new RoyaleReward { rewardName = "Poseidon X-Suit Set", rarity = "Legendary", rewardType = "OutfitBundle" });
        goldRoyalePool.Add(new RoyaleReward { rewardName = "Blood Raven Set", rarity = "Epic", rewardType = "OutfitBundle" });
        goldRoyalePool.Add(new RoyaleReward { rewardName = "Street Boy Bundle", rarity = "Rare", rewardType = "OutfitBundle" });
        goldRoyalePool.Add(new RoyaleReward { rewardName = "Imperial Corps Set", rarity = "Common", rewardType = "OutfitBundle" });
    }

    // ==========================================
    // MODULE 1: FREE FIRE STYLE WEAPON ROYALE
    // ==========================================
    public void SpinWeaponRoyale(bool spin10Times)
    {
        if (economySystem == null) return;

        int cost = spin10Times ? 540 : 60; // 10 spins par 1 spin free discount
        
        // Premium currency (Gems/UC) check
        if (economySystem.PurchaseItem(cost, true))
        {
            int spinCount = spin10Times ? 10 : 1;
            for (int i = 0; i < spinCount; i++)
            {
                weaponLuckPoints++;
                CalculateWeaponSpinResult();
            }
        }
    }

    void CalculateWeaponSpinResult()
    {
        RoyaleReward wonReward;
        
        // Free Fire Rule: Agar luck 100 ho jaye toh guaranteed Legendary item milega
        if (weaponLuckPoints >= 100 || Random.Range(0, 100) < 5)
        {
            wonReward = weaponRoyalePool[Random.Range(0, 2)]; // Pehle do legendary items hain
            weaponLuckPoints = 0; // Luck reset
        }
        else
        {
            wonReward = weaponRoyalePool[Random.Range(2, weaponRoyalePool.Count)];
        }

        Debug.Log("💥 WEAPON ROYALE SPIN RESULT: You won [" + wonReward.rewardName + "] (Rarity: " + wonReward.rarity + ")");
        
        // Database connection: Direct skin unlock handler trigger karna
        if (weaponSkinSystem != null && wonReward.rewardType == "WeaponSkin")
        {
            weaponSkinSystem.UnlockNewSkin(wonReward.rewardName);
        }
    }

    // ==========================================
    // MODULE 2: GOLD ROYALE (BGMI BEST BUNDLES)
    // ==========================================
    public void SpinGoldRoyale(bool spin10Times)
    {
        if (economySystem == null) return;

        int coinCost = spin10Times ? 2700 : 300; // Gold Coins/BP Price

        if (economySystem.SpendGold(coinCost))
        {
            int spinCount = spin10Times ? 10 : 1;
            for (int i = 0; i < spinCount; i++)
            {
                goldLuckPoints++;
                CalculateGoldSpinResult();
            }
        }
    }

    void CalculateGoldSpinResult()
    {
        RoyaleReward wonReward;

        // Luck percentage rule for BGMI Pharaoh/Poseidon X-Suits
        if (goldLuckPoints >= 150 || Random.Range(0, 100) < 3)
        {
            wonReward = goldRoyalePool[Random.Range(0, 2)]; // Top Pharaoh / Poseidon
            goldLuckPoints = 0;
        }
        else
        {
            wonReward = goldRoyalePool[Random.Range(2, goldRoyalePool.Count)];
        }

        Debug.Log("🪙 GOLD ROYALE SPIN RESULT: You unlocked [" + wonReward.rewardName + "]!");
        // Inventory system save trigger
        PlayerPrefs.SetInt("Unlocked_" + wonReward.rewardName, 1);
        PlayerPrefs.Save();
    }
}
