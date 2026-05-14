using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityData
{
    public string abilityName;
    public string type;        // "Active", "Passive", "Tactical", "Pet_Skill"
    public string description;
    public float cooldown;
}

[System.Serializable]
public class ProfileData
{
    public string bio;
    public string age;
    public string specialty; // e.g., "Assault", "Medic", "Survival", "Luxury Emotes"
}

[System.Serializable]
public class RosterItem
{
    public int id;
    public string name;
    public string category;   // "Character" ya "Pet"
    public string originGame; // "Free Fire" ya "BGMI"
    public string rarity;     // "Luxury", "Epic", "Rare"
    public int gemCost;
    public int goldCost;
    public AbilityData ability;
    public ProfileData profile;
}

public class CharacterRoster50 : MonoBehaviour
{
    public List<RosterItem> masterRoster = new List<RosterItem>();
    public EconomyManager economy;

    void Awake()
    {
        GenerateMegaRoster();
    }

    void GenerateMegaRoster()
    {
        // ==========================================
        // 1. FREE FIRE CHARACTERS (38 CHARACTERS)
        // ==========================================
        AddCharacter(1, "Alok", "Free Fire", "Luxury", 599, 0, "Drop the Beat", "Active", "Creates 5m aura, increases speed by 15%, heals 5HP/s.", 45f, "World famous DJ.", "28", "Support & Healing");
        AddCharacter(2, "Chrono", "Free Fire", "Luxury", 599, 0, "Time Turner", "Active", "Creates a force field that blocks 800 damage.", 120f, "Bounty hunter from another dimension.", "35", "Defense");
        AddCharacter(3, "Kelly", "Free Fire", "Rare", 199, 2000, "Dash", "Passive", "Sprinting speed increased by 6%.", 0f, "High school track star.", "17", "Sprinting");
        AddCharacter(4, "Hayato", "Free Fire", "Epic", 399, 8000, "Bushido", "Passive", "Armor penetration increases as HP decreases.", 0f, "Samurai family heir.", "20", "Armor Pierce");
        AddCharacter(5, "Moco", "Free Fire", "Rare", 199, 2000, "Hacker's Eye", "Passive", "Tags enemies shot for 5 seconds.", 0f, "Legendary cyber hacker.", "22", "Intel");
        AddCharacter(6, "K", "Free Fire", "Luxury", 599, 0, "Master of All", "Active", "Max EP increases by 50. Rapid EP recovery mode.", 3f, "Psychology professor and martial artist.", "32", "EP Conversion");
        AddCharacter(7, "Wukong", "Free Fire", "Epic", 399, 8000, "Camouflage", "Active", "Turns into a bush, reducing speed by 20%.", 200f, "Mysterious monkey king.", "Unknown", "Stealth");
        AddCharacter(8, "Maxim", "Free Fire", "Rare", 199, 2000, "Gluttony", "Passive", "Eating mushrooms and using medkits is 25% faster.", 0f, "Speed eating champion.", "17", "Fast Heal");
        AddCharacter(26, "Adom", "Free Fire", "Rare", 0, 0, "Classic Survivor", "Passive", "The ultimate pioneer survivor.", "Unknown", "Balanced Baseline");

        // Loop to generate remaining FF Characters up to 38
        for (int i = 9; i <= 38; i++)
        {
            if(i == 26) continue;
            AddCharacter(i, "FF Hero Squad-" + i, "Free Fire", "Epic", 299, 5000, "Survival Core", "Passive", "Slightly reduces recoil of all weapons.", 0f, "Elite special forces operator.", "26", "Assault");
        }

        // ==========================================
        // 2. BGMI CHARACTERS (37 CHARACTERS) -> TOTAL 75
        // ==========================================
        AddCharacter(39, "Victor", "BGMI", "Rare", 0, 0, "Submachine Master", "Tactical", "Reduces SMG reload time by 10%.", 0f, "Hardcore SMG enthusiast.", "30", "SMG Reload");
        AddCharacter(40, "Carlo", "BGMI", "Luxury", 599, 0, "Bounty Hunter", "Tactical", "Reduces fall damage by 20%.", 0f, "Highly skilled professional assassin.", "28", "Survival");
        AddCharacter(41, "Sara", "BGMI", "Epic", 399, 6000, "Vehicle Expert", "Tactical", "Reinforces vehicles, reducing damage taken by 10%.", 0f, "Talented mechanic and driver.", "23", "Driving");
        AddCharacter(42, "Andy", "BGMI", "Luxury", 599, 0, "Puppet Master", "Tactical", "Increases gun drawing and holstering speed by 15%.", 0f, "Ex-puppeteer turned marksman.", "35", "Fast Draw");

        for (int i = 43; i <= 75; i++)
        {
            AddCharacter(i, "Tactical Commando-" + i, "BGMI", "Epic", 299, 5000, "Squad Tactics", "Tactical", "Increases crawl speed when knocked down by 15%.", 0f, "Frontline specialized infantry.", "29", "Tactical");
        }

        // ==========================================
        // 3. FREE FIRE PETS (25 PETS)
        // ==========================================
        AddPet(101, "Detective Panda", "Free Fire", "Epic", 299, 5000, "Panda's Blessings", "Restores 10 HP upon every elimination.", "A cute panda with detective instincts.");
        AddPet(102, "Falco", "Free Fire", "Luxury", 499, 10000, "Skyline Spree", "Increases diving and gliding speed by 45%.", "Majestic eagle ruling the skies.");
        AddPet(103, "Robo", "Free Fire", "Epic", 299, 5000, "Iron Wall", "Adds a reinforcement shield to covers (+100 HP).", "Futuristic smart helper robot.");
        AddPet(104, "Ottero", "Free Fire", "Rare", 199, 3000, "Double Blubber", "Recover EP equal to 65% of HP restored from Medkits.", "Music loving adorable otter.");
        AddPet(105, "Mr. Waggor", "Free Fire", "Luxury", 499, 12000, "Smooth Gloo", "Generates utility items over time if inventory is empty.", "Dapper penguin with tactical gear.");

        for (int i = 106; i <= 125; i++)
        {
            AddPet(i, "FF Companion-" + i, "Free Fire", "Rare", 199, 3000, "Scout Aura", "Increases mushroom detection range by 5 meters.", "Wild trained wilderness companion.");
        }

        // ==========================================
        // 4. BGMI PETS / COMPANIONS (25 PETS) -> TOTAL 50
        // ==========================================
        AddPet(126, "The Falcon", "BGMI", "Luxury", 599, 15000, "Apex Emote", "Perches on shoulder. Triggers victory flight animation.", "The legendary premium BGMI avian companion.");
        AddPet(127, "Shiba", "BGMI", "Epic", 399, 7000, "Celebration Dance", "Triggers exclusive cute MVP celebration dance emotes.", "Loyal puppy with unmatched style.");
        AddPet(128, "Corgi", "BGMI", "Rare", 199, 4000, "Lobby Stroll", "Performs stylish walking and resting loops in main lobby.", "Fluffy short-legged royalty pup.");
        AddPet(129, "Snow Wolf", "BGMI", "Luxury", 599, 15000, "Alpha Howl", "Unlocks specialized arctic themes on match victory screens.", "Majestic winter beast.");

        for (int i = 130; i <= 150; i++)
        {
            AddPet(i, "Tactical Buddy-" + i, "BGMI", "Rare", 199, 4000, "Alert Look", "Performs a growling alert animation when squad is nearby.", "Domestic trained tactical support pet.");
        }
    }

    void AddCharacter(int id, string name, string origin, string rarity, int gems, int gold, string abName, string abType, string desc, float cd, string bio, string age, string spec)
    {
        masterRoster.Add(new RosterItem {
            id = id, name = name, category = "Character", originGame = origin, rarity = rarity, gemCost = gems, goldCost = gold,
            ability = new AbilityData { abilityName = abName, type = abType, description = desc, cooldown = cd },
            profile = new ProfileData { bio = bio, age = age, specialty = spec }
        });
    }

    void AddPet(int id, string name, string origin, string rarity, int gems, int gold, string abName, string desc, string bio)
    {
        masterRoster.Add(new RosterItem {
            id = id, name = name, category = "Pet", originGame = origin, rarity = rarity, gemCost = gems, goldCost = gold,
            ability = new AbilityData { abilityName = abName, type = "Pet_Skill", description = desc, cooldown = 0f },
            profile = new ProfileData { bio = bio, age = "Unknown", specialty = "Companion Mechanics" }
        });
    }

    public void PurchaseItem(int id, bool useGems)
    {
        foreach (var item in masterRoster)
        {
            if (item.id == id)
            {
                if (useGems && economy.PurchaseItem(item.gemCost, true))
                {
                    Debug.Log("Unlocked: " + item.name + " via Gems!");
                }
                else if (!useGems && economy.SpendGold(item.goldCost))
                {
                    Debug.Log("Unlocked: " + item.name + " via Gold!");
                }
                return;
            }
        }
    }
}
