using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private CharacterController moveScript;

    [Header("Weapon Shooting Reference")]
    // Agar aapke paas weapon ki script hai toh use yahan link karein
    public bool isShooting = false;
    public bool isReloading = false;

    void Start()
    {
        // GameObject se Animator aur Movement scripts ko auto-get karna
        animator = GetComponent<Animator>();
        moveScript = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovementAnimations();
        HandleCombatAnimations();
    }

    // 1. Movement Animations Logic (Idle, Walk, Run)
    void HandleMovementAnimations()
    {
        if (moveScript == null || animator == null) return;

        // Input check karna
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Kya player move kar raha hai?
        bool isMoving = (horizontal != 0 || vertical != 0);
        
        // Kya player Left Shift daba kar aage daud raha hai?
        bool isSprinting = isMoving && Input.GetKey(KeyCode.LeftShift) && vertical > 0;

        // Animator Parameters ko Update karna
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isSprinting", isSprinting);

        // Advance: Blend Tree ke liye Speed pass karna (Agar aap 1D/2D Blend tree use kar rahe hain)
        float speed = isSprinting ? 2f : (isMoving ? 1f : 0f);
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    // 2. Combat Animations Logic (Shooting, Reloading)
    void HandleCombatAnimations()
    {
        if (animator == null) return;

        // Mouse Left-Click dabane par Shooting animation trigger hoga
        if (Input.GetButton("Fire1") && !isReloading)
        {
            isShooting = true;
            animator.SetBool("isShooting", true);
        }
        else
        {
            isShooting = false;
            animator.SetBool("isShooting", false);
        }

        // 'R' Key dabane par Reloading start hogi
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            TriggerReload();
        }
    }

    void TriggerReload()
    {
        isReloading = true;
        animator.SetTrigger("ReloadTrigger");
        
        // Note: Apne animation ki length ke hisab se reload time set karein (jaise 2.5 seconds)
        Invoke("ResetReloadStatus", 2.5f); 
    }

    void ResetReloadStatus()
    {
        isReloading = false;
    }
}
