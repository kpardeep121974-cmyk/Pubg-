using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FreeFireWeapon
{
    public string weaponName;
    public string weaponCategory; // "AR", "SMG", "Shotgun", "Sniper", "Pistol"
    public int baseDamage;
    public int rateOfFire;
    public int range;
    public int reloadSpeed;
    public bool isEvoGun;         // Kya ye EVO gun hai? (True/False)
    public int currentEvoLevel = 1; // Default level 1 (Max level 7)
}

public class WeapensSkinManager : MonoBehaviour
{
    // Do alag options/tabs jaisa Free Fire mein hota hai
    public List<FreeFireWeapon> normalWeaponsTab = new List<FreeFireWeapon>();
    public List<FreeFireWeapon> evoGunsTab = new List<FreeFireWeapon>();

    void Start()
    {
        LoadAllFreeFireWeapons();
    }

    void LoadAllFreeFireWeapons()
    {
        // ==========================================
        // 1. EVO GUNS TAB (Free Fire Famous EVO Guns)
        // ==========================================
        AddEvoWeapon("AK47 - Blue Flame Draco", "AR", 61, 56, 72, 47);
        AddEvoWeapon("M4A1 - Infernal Draco", "AR", 60, 56, 78, 48);
        AddEvoWeapon("MP40 - Predatory Cobra", "SMG", 48, 83, 22, 48);
        AddEvoWeapon("SCAR - Megalodon Alpha", "AR", 53, 61, 60, 41);
        AddEvoWeapon("M1014 - Green Flame Draco", "Shotgun", 94, 38, 10, 20);
        AddEvoWeapon("UMP40 - Booyah Day", "SMG", 49, 75, 24, 43);
        AddEvoWeapon("FAMAS - Demonic Grin", "AR", 53, 67, 70, 47);

        // ==========================================
        // 2. NORMAL WEAPONS TAB (Baaki Saare FF Weapons)
        // ==========================================
        // Assault Rifles (AR)
        AddNormalWeapon("GROZA", "AR", 61, 56, 75, 48);
        AddNormalWeapon("M14", "AR", 71, 43, 76, 41);
        AddNormalWeapon("SKS", "AR", 82, 35, 82, 41);
        AddNormalWeapon("SVD", "AR", 89, 34, 80, 41);
        AddNormalWeapon("AUG", "AR", 56, 61, 56, 48);
        AddNormalWeapon("Woodpecker", "AR", 85, 38, 63, 48);

        // Submachine Guns (SMG)
        AddNormalWeapon("P90", "SMG", 48, 75, 27, 48);
        AddNormalWeapon("Vector", "SMG", 47, 81, 20, 62);
        AddNormalWeapon("MAC10", "SMG", 49, 75, 25, 43);
        AddNormalWeapon("Thompson", "SMG", 50, 77, 33, 48);

        // Shotguns
        AddNormalWeapon("M1887", "Shotgun", 100, 42, 15, 34); // Double Barrel 100 Damage!
        AddNormalWeapon("MAG-7", "Shotgun", 89, 53, 11, 55);
        AddNormalWeapon("Charge Buster", "Shotgun", 95, 35, 18, 40);

        // Snipers (AWM & MARKSMAN)
        AddNormalWeapon("AWM", "Sniper", 90, 27, 91, 34);
        AddNormalWeapon("M82B", "Sniper", 90, 27, 85, 41); // Gloo wall piercer
        AddNormalWeapon("KAR98K", "Sniper", 90, 27, 84, 27);
    }

    void AddNormalWeapon(string name, string cat, int dmg, int rof, int rng, int reload)
    {
        normalWeaponsTab.Add(new FreeFireWeapon {
            weaponName = name, weaponCategory = cat, baseDamage = dmg, 
            rateOfFire = rof, range = rng, reloadSpeed = reload, isEvoGun = false
        });
    }

    void AddEvoWeapon(string name, string cat, int dmg, int rof, int rng, int reload)
    {
        evoGunsTab.Add(new FreeFireWeapon {
            weaponName = name, weaponCategory = cat, baseDamage = dmg, 
            rateOfFire = rof, range = rng, reloadSpeed = reload, isEvoGun = true, currentEvoLevel = 1
        });
    }

    // EVO Gun Level Up karne ka function (Max Level 7)
    public void UpgradeEvoGun(string name, EconomyManager economy)
    {
        foreach (var evoGun in evoGunsTab)
        {
            if (evoGun.weaponName == name && evoGun.currentEvoLevel < 7)
            {
                int tokenCost = evoGun.currentEvoLevel * 100; // Har level par cost badhegi
                
                if (economy.PurchaseItem(tokenCost, true)) // Gems se tokens kharid kar upgrade
                {
                    evoGun.currentEvoLevel++;
                    evoGun.baseDamage += 2;      // Extra Luxury stats unlock
                    evoGun.rateOfFire += 1;
                    Debug.Log(name + " Upgraded to Level " + evoGun.currentEvoLevel + "! Visual Exclusive Effect Unlocked.");
                }
                return;
            }
        }
    }
}
