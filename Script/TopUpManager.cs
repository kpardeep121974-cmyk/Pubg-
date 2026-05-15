using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GemPack
{
    public string packName;
    public int gemAmount;
    public float priceInINR; // Luxury price e.g., 80, 400, 800
}

public class TopUpManager : MonoBehaviour
{
    public List<GemPack> shopPacks = new List<GemPack>();
    public EconomyManager economy; // Reference to your Economy script

    void Start()
    {
        InitializeStore();
    }

    void InitializeStore()
    {
        // Free Fire style Top-up packs
        shopPacks.Add(new GemPack { packName = "Starter Pack", gemAmount = 100, priceInINR = 80f });
        shopPacks.Add(new GemPack { packName = "Luxury Bundle", gemAmount = 520, priceInINR = 400f });
        shopPacks.Add(new GemPack { packName = "Elite Stash", gemAmount = 1060, priceInINR = 800f });
    }

    public void BuyGems(int packIndex)
    {
        if (packIndex < shopPacks.Count)
        {
            GemPack selected = shopPacks[packIndex];
            
            // Yahan real payment gateway (Google Pay/Apple Pay) ka logic integrate hota hai
            Debug.Log("Processing payment of ₹" + selected.priceInINR);
            
            // Payment success hone ke baad:
            CompletePurchase(selected.gemAmount);
        }
    }

    void CompletePurchase(int amount)
    {
        economy.AddGems(amount);
        Debug.Log("Successfully added " + amount + " Gems to your luxury vault!");
    }
}
