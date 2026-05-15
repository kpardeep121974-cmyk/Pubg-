using UnityEngine;

public class PlayerParachute : MonoBehaviour
{
    [Header("Physics Settings")]
    public float freeFallSpeed = 60f;
    public float parachuteSpeed = 12f;
    public float gravityMultiplier = 9.81f;

    private CharacterController controller;
    private bool isParachuteOpen = false;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
    }

    void Update()
    {
        // 1. Check if the player has hit the ground
        if (controller.isGrounded)
        {
            LandPlayer();
            return;
        }

        // 2. Press 'F' to open parachute while falling
        if (Input.GetKeyDown(KeyCode.F) && !isParachuteOpen)
        {
            isParachuteOpen = true;
            Debug.Log("Parachute Deployed!");
        }

        // 3. Apply custom terminal velocities depending on state
        float targetTerminalVelocity = isParachuteOpen ? parachuteSpeed : freeFallSpeed;
        
        // Simulating downward gravity acceleration
        velocity.y -= gravityMultiplier * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -targetTerminalVelocity, 0f);

        // Simple forward movement while gliding based on keyboard inputs
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveInput = (transform.right * moveX + transform.forward * moveZ).normalized;

        Vector3 finalMovement = (moveInput * (targetTerminalVelocity * 0.5f)) + velocity;
        controller.Move(finalMovement * Time.deltaTime);
    }

    void LandPlayer()
    {
        Debug.Log("Player landed safely on the battlefield.");
        
        // Enable normal walking movement scripts
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = true;

        // Self destruct this falling script since the jump is finished
        Destroy(this);
    }
}
