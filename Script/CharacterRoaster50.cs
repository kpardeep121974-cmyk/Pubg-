using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HybridAbility
{
    public string name;
    public string type; // "Active" (Free Fire style) ya "Passive/Tactical" (BGMI style)
    public string description;
    public float cooldown;
}

[System.Serializable]
public class RosterCharacter
{
    public int id;
    public string name;
    public string originGame; // "Free Fire" ya "BGMI"
    public string rarity;     // "Luxury", "Epic", "Rare"
    public int gemCost;
    public HybridAbility ability;
}

public class CharacterRoster50 : MonoBehaviour
{
    public List<RosterCharacter> allCharacters = new List<RosterCharacter>();

    void Awake()
    {
        Load50Characters();
    }

    void Load50Characters()
    {
        // ==========================================
        // SECTION 1: FREE FIRE SIDE (26 CHARACTERS)
        // ==========================================
        AddFFChar(1, "Alok", "Luxury", 599, "Drop the Beat", "Active", "Creates 5m aura, increases speed by 15%, heals 5HP/s.", 45f);
        AddFFChar(2, "Chrono", "Luxury", 599, "Time Turner", "Active", "Creates a force field that blocks 800 damage.", 120f);
        AddFFChar(3, "Kelly", "Rare", 199, "Dash", "Passive", "Sprinting speed increased by 6%.", 0f);
        AddFFChar(4, "Hayato", "Epic", 399, "Bushido", "Passive", "With every 10% decrease in max HP, armor penetration increases.", 0f);
        AddFFChar(5, "Moco", "Rare", 199, "Hacker's Eye", "Passive", "Tags enemies shot for 5 seconds.", 0f);
        AddFFChar(6, "K", "Luxury", 599, "Master of All", "Active", "Max EP increases by 50. Mode switch: EP regeneration or conversion.", 3f);
        AddFFChar(7, "Wukong", "Epic", 399, "Camouflage", "Active", "Turns into a bush, reducing speed by 20%.", 200f);
        AddFFChar(8, "Maxim", "Rare", 199, "Gluttony", "Passive", "Eating mushrooms and using medkits is 25% faster.", 0f);
        AddFFChar(9, "Andrew", "Rare", 0, "Armor Specialist", "Passive", "Vest durability loss decreased by 12%.", 0f);
        AddFFChar(10, "Paloma", "Rare", 199, "Arms Dealing", "Passive", "Can carry AR ammo without taking up inventory space.", 0f);
        AddFFChar(11, "Jota", "Epic", 399, "Sustained Raids", "Passive", "Knocking down enemies restores HP.", 0f);
        AddFFChar(12, "Steffie", "Epic", 399, "Painted Refuge", "Active", "Creates graffiti that reduces explosive damage by 25%.", 90f);
        AddFFChar(13, "A124", "Rare", 199, "Thrill of Battle", "Active", "Converts 60 EP into HP quickly.", 10f);
        AddFFChar(14, "Rafael", "Epic", 399, "Dead Silent", "Passive", "Silencing effect when using Snipers and Marksman Rifles.", 0f);
        AddFFChar(15, "Laura", "Rare", 199, "Sharp Shooter", "Passive", "Accuracy increases by 35% while scoped in.", 0f);
        AddFFChar(16, "Clu", "Epic", 399, "Tracing Steps", "Active", "Locates positions of enemies within 75m who are not prone.", 75f);
        AddFFChar(17, "Luqueta", "Rare", 199, "Hat-Trick", "Passive", "Every kill increases max HP by up to 25.", 0f);
        AddFFChar(18, "Shirou", "Rare", 199, "Damage Delivered", "Passive", "When hit by enemy, attacker is marked and first shot has armor pen.", 25f);
        AddFFChar(19, "Skyler", "Luxury", 599, "Riptide Rhythm", "Active", "Unleashes a sonic wave to destroy Gloo Walls.", 60f);
        AddFFChar(20, "Dasha", "Rare", 199, "Partying On", "Passive", "Reduces damage taken from falls and recovery time.", 0f);
        AddFFChar(21, "Xayne", "Epic", 399, "Xtreme Encounter", "Active", "Gets 80 temporary HP, increases damage to shields.", 150f);
        AddFFChar(22, "D-Bee", "Rare", 199, "Bullet Beats", "Passive", "Moving while shooting increases speed and accuracy.", 0f);
        AddFFChar(23, "Thiva", "Rare", 199, "Vital Vibes", "Passive", "Rescue speed increases by 30%.", 0f);
        AddFFChar(24, "Dimitri", "Luxury", 599, "Healing Heartbeat", "Active", "Creates a 3.5m healing zone where players can self-recover.", 60f);
        AddFFChar(25, "Otho", "Rare", 199, "Memory Mist", "Passive", "Form a mist that reveals positions of other enemies after a kill.", 0f);
        AddFFChar(26, "Adom", "Rare", 0, "Classic Survivor", "Passive", "The ultimate pioneer. Default character with balanced core physical baseline.", 0f);

        // ==========================================
        // SECTION 2: BGMI SIDE (25 CHARACTERS)
        // ==========================================
        AddBgmChar(27, "Victor", "Rare", 0, "Submachine Master", "Tactical", "Reduces SMG reload time by 10% in matches.", 0f);
        AddBgmChar(28, "Carlo", "Luxury", 599, "Bounty Hunter", "Tactical", "Reduces fall damage by 20%.", 0f);
        AddBgmChar(29, "Sara", "Epic", 399, "Vehicle Expert", "Tactical", "Reinforces vehicles, reducing damage taken when driving by 10%.", 0f);
        AddBgmChar(30, "Andy", "Luxury", 599, "Puppet Master", "Tactical", "Increases gun drawing and holstering speed by 15%.", 0f);
        AddBgmChar(31, "Anna", "Rare", 199, "Investigator", "Tactical", "Slightly decreases sound of own footsteps while crouching.", 0f);
        AddBgmChar(32, "Sophia", "Epic", 399, "Shield Tech", "Tactical", "Slightly speeds up energy drink consumption.", 0f);
        AddBgmChar(33, "Riley", "Rare", 199, "Survivalist", "Tactical", "Bandage healing efficiency increased by 5%.", 0f);
        AddBgmChar(34, "Lorenzo", "Epic", 399, "Iron Wall", "Tactical", "Takes 5% less damage from blue zone outside the safe circle.", 0f);
        AddBgmChar(35, "Laith", "Luxury", 599, "Assault Specialist", "Tactical", "Slightly reduces AR weapon shake when firing long distance.", 0f);
        
        // Custom BGMI-Style Battle Royale Troops for Luxury Vibe
        for (int i = 36; i <= 51; i++)
        {
            AddBgmChar(i, "Tactical Soldier Squad-" + (i-35), "Epic", 299, "Squad Tactics", "Passive", "Increases reload speed of all squad weapons by 2%.", 0f);
        }
    }

    void AddFFChar(int id, string name, string rarity, int cost, string abName, string abType, string desc, float cd)
    {
        allCharacters.Add(new RosterCharacter {
            id = id, name = name, originGame = "Free Fire", rarity = rarity, gemCost = cost,
            ability = new HybridAbility { name = abName, type = abType, description = desc, cooldown = cd }
        });
    }

    void AddBgmChar(int id, string name, string rarity, int cost, string abName, string abType, string desc, float cd)
    {
        allCharacters.Add(new RosterCharacter {
            id = id, name = name, originGame = "BGMI", rarity = rarity, gemCost = cost,
            ability = new HybridAbility { name = abName, type = abType, description = desc, cooldown = cd }
            // EventManager.cs ke andar SpinGoldRush function ka sahi tarika:
public void SpinGoldRush()
{
    int spinCost = 300;

    // Direct EconomyManager ke SpendGold function ko call kiya
    if (economy.SpendGold(spinCost))
    {
        int randomIndex = Random.Range(0, goldRushPrizePool.Count);
        GoldRushItem wonItem = goldRushPrizePool[randomIndex];
        Debug.Log("GOLD RUSH REWARD: You won " + wonItem.itemName);
        
        if (wonItem.itemName.Contains("Skin"))
        {
            inventory.EquipWeaponSkin("AK47", wonItem.itemName);
        }
    }
}
