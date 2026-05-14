using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Currently Equipped (Active Items)")]
    public string equippedCharacter = "Adom"; // Default Free Fire character
    public string equippedM4A1Skin = "Default";
    public string equippedMP40Skin = "Default";

    [Header("References to Game Systems")]
    public CharacterRoster50 characterSystem;
    public WeaponSkinManager weaponSystem;

    // 1. Character Equip Karne Ka Function
    public void EquipCharacter(string characterName)
    {
        // Pehle check karenge ki kya character roster mein hai aur unlocked hai
        foreach (var character in characterSystem.allCharacters)
        {
            if (character.name == characterName)
            {
                equippedCharacter = characterName;
                Debug.Log("Luxury Vault Update: Equipped character is now " + characterName);
                ApplyCharacterLobbyModel(characterName);
                return;
            }
        }
        Debug.Log("Cannot equip! Character might be locked.");
    }

    // 2. Weapon Skin Equip Karne Ka Function (Free Fire EVO Style)
    public void EquipWeaponSkin(string weaponName, string skinName)
    {
        if (weaponName == "M4A1")
        {
            equippedM4A1Skin = skinName;
            Debug.Log("M4A1 Skin Changed to: " + skinName);
        }
        else if (weaponName == "MP40")
        {
            equippedMP40Skin = skinName;
            Debug.Log("MP40 Skin Changed to: " + skinName);
        }
        
        UpdateLobbyWeaponVisuals();
    }

    // Visual Updates for Lobby (UI Render Logic)
    void ApplyCharacterLobbyModel(string name)
    {
        // Yahan aapka 3D Character model load karne ka logic aayega
        Debug.Log("Loading 3D Model for " + name + " in Main Lobby...");
    }

    void UpdateLobbyWeaponVisuals()
    {
        // Lobby mein khade character ke haath mein chamkti hui EVO skin dikhane ke liye
        Debug.Log("Refreshing weapon skin visuals in character's hand.");
    }
}
