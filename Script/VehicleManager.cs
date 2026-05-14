using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGMIVehicle
{
    public string vehicleName;
    public string type;         // "Sedan", "Offroad", "Bike", "Sports"
    public float maxSpeed;      // KM/H mein
    public float currentHealth;
    public float maxHealth;
    public float fuelLevel;     // 0% se 100%
    public int seatingCapacity;
}

public class VehicleManager : MonoBehaviour
{
    public List<BGMIVehicle> spawnableVehicles = new List<BGMIVehicle>();
    
    [Header("Active Vehicle Status")]
    public BGMIVehicle currentDrivenVehicle;
    public bool isPlayerInsideVehicle = false;

    void Start()
    {
        InitializeBGMIVehicles();
    }

    // 1. BGMI Ke Saare Iconic Vehicles Copy Karna
    void InitializeBGMIVehicles()
    {
        // UAZ (The King of Offroad)
        spawnableVehicles.Add(new BGMIVehicle { 
            vehicleName = "UAZ (Closed Top)", type = "Offroad", maxSpeed = 115f, 
            maxHealth = 1820f, currentHealth = 1820f, fuelLevel = 100f, seatingCapacity = 4 
        });

        // Dacia (The Classic Squad Sedan)
        spawnableVehicles.Add(new BGMIVehicle { 
            vehicleName = "Dacia 1300", type = "Sedan", maxSpeed = 120f, 
            maxHealth = 1200f, currentHealth = 1200f, fuelLevel = 100f, seatingCapacity = 4 
        });

        // Coupe RB (Super Fast 2-Seater)
        spawnableVehicles.Add(new BGMIVehicle { 
            vehicleName = "Coupe RB", type = "Sports", maxSpeed = 150f, 
            maxHealth = 1000f, currentHealth = 1000f, fuelLevel = 100f, seatingCapacity = 2 
        });

        // Buggy (Excellent Suspension)
        spawnableVehicles.Add(new BGMIVehicle { 
            vehicleName = "Buggy", type = "Offroad", maxSpeed = 100f, 
            maxHealth = 1100f, currentHealth = 1100f, fuelLevel = 100f, seatingCapacity = 2 
        });

        // Motorcycle (The Fastest Rotation Vehicle)
        spawnableVehicles.Add(new BGMIVehicle { 
            vehicleName = "Motorcycle (2-Seater)", type = "Bike", maxSpeed = 152f, 
            maxHealth = 1000f, currentHealth = 1000f, fuelLevel = 100f, seatingCapacity = 2 
        });
    }

    // 2. Gaadi Mein Bethne Ka Logic (Sara Character Buff Connected)
    public void EnterVehicle(string vehicleName, string activeCharacterName)
    {
        foreach (var v in spawnableVehicles)
        {
            if (v.vehicleName == vehicleName)
            {
                currentDrivenVehicle = v;
                isPlayerInsideVehicle = true;
                Debug.Log("Entered Vehicle: " + v.vehicleName + " | Max Speed: " + v.maxSpeed + " KM/H");

                // [BGMI CHARACTER SKILL TRIGGER]: Sara's Vehicle Expert Ability
                if (activeCharacterName == "Sara")
                {
                    Debug.Log("Sara Skill Active! Vehicle structural reinforcement applied. Damage vulnerability reduced by 10%.");
                }
                return;
            }
        }
    }

    // 3. Gaadi Par Enemy Fire / Blast Logic
    public void DamageVehicle(float damageAmount, string activeCharacterName)
    {
        if (!isPlayerInsideVehicle || currentDrivenVehicle == null) return;

        // Agar character Sara hai, toh damage 10% kam ho jayega
        if (activeCharacterName == "Sara")
        {
            damageAmount *= 0.90f; 
        }

        currentDrivenVehicle.currentHealth -= damageAmount;
        Debug.Log(currentDrivenVehicle.vehicleName + " took damage! Remaining Health: " + currentDrivenVehicle.currentHealth);

        if (currentDrivenVehicle.currentHealth <= 0)
        {
            currentDrivenVehicle.currentHealth = 0;
            VehicleExplode();
        }
    }

    // 4. Fuel Consuming Simulation (Chalte chalte fuel kam hona)
    public void UseFuel(float amount)
    {
        if (currentDrivenVehicle != null && isPlayerInsideVehicle)
        {
            currentDrivenVehicle.fuelLevel = Mathf.Max(0f, currentDrivenVehicle.fuelLevel - amount);
            if (currentDrivenVehicle.fuelLevel <= 0)
            {
                Debug.Log("Vehicle is out of fuel! Need a Gas Can.");
            }
        }
    }

    void VehicleExplode()
    {
        Debug.Log("💥 VEHICLE EXPLODED! Immediate squad elimination risk!");
        isPlayerInsideVehicle = false;
        currentDrivenVehicle = null;
        // Yahan par particle effect aur player damage handler call hoga
    }
}
