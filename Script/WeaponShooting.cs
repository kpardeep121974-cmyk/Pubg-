using System.Collections;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("Current Weapon Equip")]
    public string activeWeaponName = "MP40 - Predatory Cobra";
    public int currentAmmo = 30;
    public int maxMagazineSize = 30;

    [Header("Connected Systems")]
    public WeapensSkinManager weaponDatabase;
    public HealthSystem enemyHealth; 
    public CharacterRoster50 petSystem;

    private bool isFiring = false;

    // 1. Shoot Trigger Function
    public void PressFireButton()
    {
        if (currentAmmo > 0 && !isFiring)
        {
            StartCoroutine(FireWeaponLoop());
        }
        else if (currentAmmo <= 0)
        {
            Debug.Log("OUT OF AMMO! Need to reload.");
            CheckMrWaggorPetSkill();
        }
    }

    IEnumerator FireWeaponLoop()
    {
        isFiring = true;
        
        // Weapon stats database se uthane ka simulation
        int damagePerShot = 35; 
        float fireRateCooldown = 0.1f; // Fast SMG style

        currentAmmo--;
        Debug.Log("Fired 1 Bullet from " + activeWeaponName + " | Ammo Left: " + currentAmmo);

        // Raycast ya hit detection logic se enemy ko damage dena
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damagePerShot);
        }

        yield return new WaitForSeconds(fireRateCooldown);
        isFiring = false;
    }

    public void ReloadWeapon()
    {
        currentAmmo = maxMagazineSize;
        Debug.Log("Weapon Reloaded Successfully!");
    }

    // [PET SKILL TRIGGER]: Mr. Waggor Smooth Gloo/Utility Supply
    void CheckMrWaggorPetSkill()
    {
        if (currentAmmo == 0)
        {
            Debug.Log("Mr. Waggor Skill Check: Player inventory is empty. Generating tactical ammo supply pack!");
            currentAmmo = 10; // Bonus emergency ammo supply
        }
    }
}
