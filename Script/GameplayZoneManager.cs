using System.Collections;
using UnityEngine;

public class GameplayZoneManager : MonoBehaviour
{
    [Header("Connected Core Systems")]
    public HealthSystem playerHealth; // Connection to player's HP/EP

    [Header("BGMI Style Safe Zone (Playzone)")]
    public float safeZoneRadius = 1000f;
    public float minSafeZoneRadius = 50f;
    public bool isZoneShrinking = false;
    public float blueZoneDamage = 2f; // Har second kitni HP kategi

    [Header("Free Fire Style Danger Zone (Red Zone)")]
    public bool isDangerZoneActive = false;
    public Vector3 dangerZoneCenter;
    public float dangerZoneRadius = 150f;

    void Start()
    {
        // Game start hote hi zone ka loop shuru ho jayega
        StartCoroutine(MainGameplayLoop());
    }

    // Both Games Combo: Main Match Timing Loop
    IEnumerator MainGameplayLoop()
    {
        while (safeZoneRadius > minSafeZoneRadius)
        {
            // 1. Waiting for next zone circle
            yield return new WaitForSeconds(60f); // 1 minute ka hold

            // 2. Free Fire Style Danger Zone Trigger (Randomly anywhere on map)
            TriggerFreeFireDangerZone();

            // 3. BGMI Style Playzone Shrinking Starts
            yield return StartCoroutine(ShrinkPlayzone());

            // Danger zone automatic clear ho jayega playzone shrink hone ke baad
            ClearDangerZone();
        }
    }

    // ==========================================
    // MODULE 1: BGMI BLUE ZONE LOGIC
    // ==========================================
    IEnumerator ShrinkPlayzone()
    {
        isZoneShrinking = true;
        Debug.Log("⚠️ BGMI ALERT: The Playzone is shrinking! Get to the safe zone as soon as possible.");
        
        float targetRadius = safeZoneRadius * 0.6f; // Har baar zone 40% chota hoga
        float shrinkDuration = 30f; // 30 seconds tak circle chota hota rahega
        float elapsed = 0f;

        while (elapsed < shrinkDuration)
        {
            safeZoneRadius = Mathf.Lerp(safeZoneRadius, targetRadius, elapsed / shrinkDuration);
            elapsed += Time.deltaTime;

            // Check karenge ki player zone ke andar hai ya bahar
            CheckPlayerOutsideZonePosition();

            yield return null;
        }

        safeZoneRadius = targetRadius;
        isZoneShrinking = false;
        Debug.Log("Playzone has stopped shrinking.");
    }

    void CheckPlayerOutsideZonePosition()
    {
        // Simulation: Real game mein hum player aur circle center ka distance nikalenge
        bool isOutside = Random.value > 0.85f; // 15% chance simulation debug ke liye

        if (isOutside && playerHealth != null)
        {
            Debug.Log("💔 Player is taking damage from the BGMI Blue Zone!");
            playerHealth.TakeDamage(blueZoneDamage);
        }
    }

    // ==========================================
    // MODULE 2: FREE FIRE DANGER ZONE LOGIC
    // ==========================================
    void TriggerFreeFireDangerZone()
    {
        isDangerZoneActive = true;
        dangerZoneCenter = new Vector3(Random.Range(-300f, 300f), 0f, Random.Range(-300f, 300f));
        
        Debug.Log("=========================================");
        Debug.Log("🔴 FREE FIRE WARNING: Danger Zone has appeared on the map!");
        Debug.Log("Location: " + dangerZoneCenter + " | Take cover inside buildings immediately!");
        Debug.Log("=========================================");

        // Bombardment simulation
        InvokeRepeating("DropDangerZoneBomb", 2f, 3f);
    }

    void DropDangerZoneBomb()
    {
        if (!isDangerZoneActive) return;

        Debug.Log("💥 BOOM! A missile struck inside the Danger Zone.");
        
        // Agar player open ground me coordinates ke paas hai toh instant heavy damage
        bool playerCaughtInOpen = Random.value > 0.90f; 
        if (playerCaughtInOpen && playerHealth != null)
        {
            Debug.Log("❌ Direct Hit! Player knocked out by Danger Zone explosion.");
            playerHealth.TakeDamage(150f); // Massive structure damage
        }
    }

    void ClearDangerZone()
    {
        isDangerZoneActive = false;
        CancelInvoke("DropDangerZoneBomb");
        Debug.Log("The Danger Zone has disappeared. Area is now safe.");
    }
}

