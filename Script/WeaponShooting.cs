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
using UnityEngine;

public class ServerAuthoritativeShooting : MonoBehaviour
{
    public Transform cameraTransform;
    public float weaponRange = 200f;
    public float weaponDamage = 35f;

    // 1. Called on the Client when they press the left mouse button
    public void TryToFireWeapon()
    {
        // Play local muzzle flash and sound instantly for responsive game feel (Client Prediction)
        PlayLocalMuzzleFlash();

        // Send a request to the server with the camera's position and direction
        Vector3 shootOrigin = cameraTransform.position;
        Vector3 shootDirection = cameraTransform.forward;

        // NOTE: If using Photon/Mirror/Netcode, replace this with your framework's actual RPC attribute
        // e.g., [ServerRpc] or [Command]
        SendShootRequestToServer(shootOrigin, shootDirection);
    }

    // 2. This method executes ONLY on the secure Server Host
    private void SendShootRequestToServer(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        
        // The server performs the actual raycast physics calculation
        if (Physics.Raycast(origin, direction, out hit, weaponRange))
        {
            // Verify if we hit a valid player target
            HealthSystem enemyHealth = hit.collider.GetComponent<HealthSystem>();
            
            if (enemyHealth != null)
            {
                // Server calculates distance to prevent "silent aim/teleport hacks"
                float distance = Vector3.Distance(origin, hit.point);
                if (distance <= weaponRange)
                {
                    // Securely deduct health on the server version of the enemy
                    enemyHealth.TakeDamageFromServer(weaponDamage);
                    Debug.Log($"Server verified hit! Inflicted {weaponDamage} damage.");
                }
            }
        }
    }

    private void PlayLocalMuzzleFlash() { /* VFX Logic */ }
}
