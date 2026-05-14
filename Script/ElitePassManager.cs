using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassTierReward
{
    public int tierLevel;
    public string freeRewardName;
    public string eliteRewardName; // Sirf unhe milega jinhone pass purchase kiya hai
    public bool isEliteClaimed;
    public bool isFreeClaimed;
}

public class ElitePassManager : MonoBehaviour
{
    [Header("Connected Core Systems")]
    public EconomyManager economySystem;
    public UserProfileManager profileSystem;

    [Header("Pass Status")]
    public bool isElitePassPurchased = false;
    public int currentPassTier = 1;
    public int currentPassPoints = 0; // 100 points = 1 Tier Level Up
    public const int maxTierLimit = 50;

    [Header("Pass Rewards Data Grid")]
    public List<PassTierReward> passSeasonRewards = new List<PassTierReward>();

    void Start()
    {
        InitializeSeasonPassRewards();
        LoadPassProgress();
    }

    // 1. Rewards Pool Setup (BGMI Mythics + FF Materials Copy)
    void InitializeSeasonPassRewards()
    {
        passSeasonRewards.Clear();
        
        // Tier 1 to 50 sample data skeleton setup
        passSeasonRewards.Add(new PassTierReward { tierLevel = 1, freeRewardName = "500 Gold Coins", eliteRewardName = "Season 1 Premium Weapon Skin", isFreeClaimed = false, isEliteClaimed = false });
        passSeasonRewards.Add(new PassTierReward { tierLevel = 10, freeRewardName = "Supply Crate Coupon", eliteRewardName = "Street Dance Emote", isFreeClaimed = false, isEliteClaimed = false });
        passSeasonRewards.Add(new PassTierReward { tierLevel = 25, freeRewardName = "100 Gems Bundle", eliteRewardName = "Custom Room Card (1-Day)", isFreeClaimed = false, isEliteClaimed = false });
        passSeasonRewards.Add(new PassTierReward { tierLevel = 50, freeRewardName = "Elite Warrior Parachute", eliteRewardName = "Mythic Overlord Outfit Set (Max Reward)", isFreeClaimed = false, isEliteClaimed = false });
    }

    // 2. Elite Pass Unlock Logic (Free Fire Style Diamond Spending)
    public void PurchaseElitePass Upgrade()
    {
        if (isElitePassPurchased)
        {
            Debug.Log("⚠️ Elite Pass is already active for this season!");
            return;
        }

        int elitePassCost = 499; // 499 Gems / Diamonds Cost

        if (economySystem != null && economySystem.PurchaseItem(elitePassCost, true))
        {
            isElitePassPurchased = true;
            PlayerPrefs.SetInt("ElitePassActive", 1);
            PlayerPrefs.Save();
            Debug.Log("👑 ELITE PASS ACTIVATED! Premium rewards track unlocked.");
        }
    }

    // 3. Match ke baad Points badhane aur Level Up karne ka Logic
    public void AddPassPointsAfterMatch(int pointsGained)
    {
        if (currentPassTier >= maxTierLimit) return;

        currentPassPoints += pointsGained;
        Debug.Log("🎯 Gained " + pointsGained + " Pass Points. Total: " + currentPassPoints + "/100");

        // Level up verification loop
        while (currentPassPoints >= 100 && currentPassTier < maxTierLimit)
        {
            currentPassPoints -= 100;
            currentPassTier++;
            Debug.Log("⭐ LEVEL UP! Royal/Elite Pass reached Tier: " + currentPassTier);
        }

        SavePassProgress();
    }

    // 4. Rewards Claim karne ka Engine (Free aur Elite separation)
    public void ClaimTierReward(int tier)
    {
        foreach (var reward in passSeasonRewards)
        {
            if (reward.tierLevel == tier)
            {
                if (currentPassTier >= tier)
                {
                    // Free Track Reward Claiming
                    if (!reward.isFreeClaimed)
                    {
                        reward.isFreeClaimed = true;
                        Debug.Log("🎁 Claimed Free Reward: " + reward.freeRewardName);
                    }

                    // Elite Track Reward Claiming
                    if (isElitePassPurchased && !reward.isEliteClaimed)
                    {
                        reward.isEliteClaimed = true;
                        Debug.Log("🔥 Claimed Premium Elite Reward: " + reward.eliteRewardName);
                    }
                }
                else
                {
                    Debug.Log("❌ Lock! Aapka Elite Pass Tier abhi chota hai.");
                }
                return;
            }
        }
    }

    void SavePassProgress()
    {
        PlayerPrefs.SetInt("PassTier", currentPassTier);
        PlayerPrefs.SetInt("PassPoints", currentPassPoints);
        PlayerPrefs.Save();
    }

    void LoadPassProgress()
    {
        currentPassTier = PlayerPrefs.GetInt("PassTier", 1);
        currentPassPoints = PlayerPrefs.GetInt("PassPoints", 0);
        isElitePassPurchased = PlayerPrefs.GetInt("ElitePassActive", 0) == 1;
    }
}
