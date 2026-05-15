using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Player Vitals")]
    public float maxHP = 200f;
    public float currentHP = 200f;
    public float maxEP = 200f; // Free Fire Style Energy Points
    public float currentEP = 100f;

    [Header("Pet Buffs Connection")]
    public string equippedPetName = "Ottero"; // Detective Panda, Ottero, etc.
    
    void Start()
    {
        // Background mein EP se HP convert karne ka loop chalega (Free Fire style)
        StartCoroutine(EnergyToHealthConversion());
    }

    // 1. Damage Lene Ka Function
    public void TakeDamage(float damageAmount)
    {
        currentHP -= damageAmount;
        if (currentHP <= 0)
        {
            currentHP = 0;
            PlayerEliminated();
        }
        Debug.Log("Player Took Damage! Current HP: " + currentHP);
    }

    // 2. Free Fire Medkit System (Ottero Pet Connected)
    public void UseMedkit()
    {
        if (currentHP >= maxHP)
        {
            Debug.Log("HP is already full!");
            return;
        }

        float hpRegenAmount = 75f; // Ek medkit 75 HP deta hai
        currentHP = Mathf.Min(currentHP + hpRegenAmount, maxHP);
        Debug.Log("Used Medkit! HP Restored to: " + currentHP);

        // [PET SKILL TRIGGER]: Ottero Double Blubber Ability
        if (equippedPetName == "Ottero")
        {
            float epBonus = hpRegenAmount * 0.65f; // 65% of HP restored goes to EP
            currentEP = Mathf.Min(currentEP + epBonus, maxEP);
            Debug.Log("Ottero Pet Skill Activated! Bonus EP Added: " + epBonus);
        }
    }

    // 3. Free Fire Style EP to HP Logic (Har second 1 HP badhegi agar EP hai)
    IEnumerator EnergyToHealthConversion()
    {
        while (true)
        {
            if (currentEP > 0 && currentHP < maxHP)
            {
                currentEP -= 1f;
                currentHP = Mathf.Min(currentHP + 1f, maxHP);
                // Refresh UI Here
            }
            yield return new WaitForSeconds(1.0f); // Har 1 second mein convert hoga
        }
    }

    // 4. Kill/Elimination Buff (Detective Panda Pet Connected)
    public void OnEnemyEliminated()
    {
        Debug.Log("Enemy Eliminated!");

        // [PET SKILL TRIGGER]: Detective Panda's Blessings
        if (equippedPetName == "Detective Panda")
        {
            currentHP = Mathf.Min(currentHP + 10f, maxHP);
            Debug.Log("Panda's Blessings Active! Restored 10 HP on Kill. Current HP: " + currentHP);
        }
    }

    void PlayerEliminated()
    {
        Debug.Log("Player has been knocked out/eliminated!");
        // Game Over ya Knockdown logic yahan aayega
    }
}
// Inside your TakeDamage/ApplyDamage method:
currentHealth -= damageAmount;

// Add this part to update the screen automatically:
InGameHUDManager hud = GameServiceLocator.Instance?.GetService<InGameHUDManager>();
if (hud != null)
{
    // Assuming this script runs on the local player
    hud.UpdateHealth(currentHealth, maxHealth);
}
// HealthSystem.cs के अंदर जहां प्लेयर मरता है:
void Die()
{
    Debug.Log("प्लेयर मारा गया!");

    // 1. लोकल प्लेयर का कैमरा और मूवमेंट स्क्रिप्ट्स डिसेबल करें
    GetComponent<PlayerMovement>().enabled = false;
    
    // 2. मैच में बचे हुए टीममेट्स की लिस्ट बनाएं (यह डेटा आपके Matchmaking/Room मैनेजर से आ सकता है)
    List<Transform> aliveTeammates = new List<Transform>();
    
    // (मान लेते हैं कि आपके पास जिंदा साथियों की लिस्ट ढूंढने का लॉजिक है)

    // 3. SpectatorManager को कॉल करें
    SpectatorManager spectator = GameServiceLocator.Instance?.GetService<SpectatorManager>();
    if (spectator != null)
    {
        spectator.StartSpectating(aliveTeammates);
    }

    // 4. अंत में इस मरे हुए प्लेयर के कैरेक्टर मॉडल को हाइड या डिस्ट्रॉय करें
    gameObject.SetActive(false);
}
// Inside your updated HealthSystem.cs
public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // CRITICAL: Only allow the server to call this method
    public void TakeDamageFromServer(float amount)
    {
        // 1. Deduct health securely on the server instance
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // 2. Sync this new health value back down to all player clients (State Synchronization)
        SyncHealthToClients(currentHealth);

        if (currentHealth <= 0)
            {
            // Trigger the Spectator mode we built in Phase 5
            Die();
        }
    }

    private void SyncHealthToClients(float newHealth)
    {
        // Use your Networking Framework (Netcode, Photon, Mirror) to update the client's HUD
        HUDManager hud = GameServiceLocator.Instance?.GetService<HUDManager>();
        if (hud != null)
        {
            hud.UpdateHealthBar(newHealth); // Updates slider smoothly across network
        }
    }

    private void Die() { /* Spectator activation logic */ }
}
