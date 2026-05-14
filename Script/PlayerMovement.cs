using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8.5f;
    public float gravity = -19.62f; // Realistic gravity for quick drops
    public float jumpHeight = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Ground Check (Check karna ki player zameen par hai ya nahi)
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset velocity when on ground
        }

        // 2. Input Lena (WASD ya Joystick keys)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // 3. Sprint (Shift dabane par tez daudna)
        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && z > 0) // Sirf aage badhte waqt sprint hoga
        {
            currentSpeed = sprintSpeed;
        }

        // Move Player
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 4. Jump Logic (Spacebar dabane par jump)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Physics Formula: v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. Gravity Apply Karna
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
