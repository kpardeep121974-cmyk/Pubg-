using System.Collections.Generic;
using UnityEngine;

public class AirdropManager : MonoBehaviour
{
    [Header("Airdrop Status")]
    public bool isDropSpawned = false;
    public Vector3 dropLocation;

    [Header("Premium Luxury Loot Pool")]
    public List<string> airdropWeapons = new List<string>();
    public List<string> airdropGear = new List<string>();

    void Start()
    {
        InitializeAirdropLoot();
    }

    void InitializeAirdropLoot()
    {
        // Sirf rare aur powerful items jo normal ground par nahi milte
        airdropWeapons.Add("AWM Sniper");
        airdropWeapons.Add("GROZA");
        airdropWeapons.Add("M249");

        airdropGear.Add("Level 3 Helmet");
        airdropGear.Add("Level 3 Vest");
        airdropGear.Add("Ghillie Suit (Luxury Camouflage)");
    }

    // 2. Plane se drop girne ka function (Triggered by MatchmakingServer)
    public void SpawnAirdropInMatch()
    {
        isDropSpawned = true;
        dropLocation = new Vector3(Random.Range(-500f, 500f), 0f, Random.Range(-500f, 500f));
        
        Debug.Log("=========================================");
        Debug.Log("AIRDROP INCOMING! A cargo plane is flying over the map.");
        Debug.Log("Airdrop successfully dropped at coordinates: " + dropLocation);
        Debug.Log("Red Smoke Signal Activated! Go hunt for the luxury loot.");
        Debug.Log("=========================================");
    }

    // 3. Player jab drop loot karega
    public void LootAirdrop(InventoryManager playerInventory)
    {
        if (isDropSpawned)
        {
            string weaponReward = airdropWeapons[Random.Range(0, airdropWeapons.Count)];
            string gearReward = airdropGear[Random.Range(0, airdropGear.Count)];

            Debug.Log("LOOT SUCCESSFUL! You equipped: " + weaponReward + " and " + gearReward);
            
            // Player ke inventory controller ko skin aur weapon data bhejna
            playerInventory.EquipWeaponSkin(weaponReward, "Airdrop Exclusive Edition");
            
            isDropSpawned = false; // Drop empty ho gaya
        }
    }
}
