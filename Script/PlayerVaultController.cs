using UnityEngine;

public class PlayerVaultController : MonoBehaviour
{
    [Header("Vault Settings")]
    public float vaultCheckDistance = 1.5f;
    public float vaultSpeed = 6f;
    public LayerMask obstacleLayer; // जिस लेयर पर दीवारें/बाधाएं हैं (उदा. "Obstacle")

    [Header("Raycast Origins")]
    public Transform chestHeightTransform; // प्लेयर की छाती की ऊंचाई पर एक एम्प्टी ऑब्जेक्ट
    public Transform eyeHeightTransform;   // प्लेयर की आंख की ऊंचाई पर एक एम्प्टी ऑब्जेक्ट

    private CharacterController characterController;
    private bool isVaulting = false;
    private Vector3 vaultTargetPosition;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isVaulting)
        {
            PerformVaultMovement();
            return;
        }

        // जब प्लेयर 'Space' (Jump) दबाए और आगे बढ़ रहा हो, तब चेक करें
        if (Input.GetButtonDown("Jump") && Input.GetAxis("Vertical") > 0)
        {
            CheckForVault();
        }
    }

    void CheckForVault()
    {
        RaycastHit chestHit;
        // 1. चेक करें कि क्या छाती की ऊंचाई पर सामने कोई दीवार है
        if (Physics.Raycast(chestHeightTransform.position, transform.forward, out chestHit, vaultCheckDistance, obstacleLayer))
        {
            // 2. चेक करें कि क्या आंख की ऊंचाई पर सामने का रास्ता साफ है (यानी दीवार छोटी है)
            if (!Physics.Raycast(eyeHeightTransform.position, transform.forward, vaultCheckDistance, obstacleLayer))
            {
                // दीवार के ऊपर की सतह पर थोड़ा आगे का पॉइंट कैलकुलेट करें जहाँ प्लेयर को पहुंचना है
                Vector3 vaultDir = transform.forward;
                vaultTargetPosition = chestHit.point + (Vector3.up * 1.5f) + (vaultDir * 1f);
                
                StartVault();
            }
        }
    }

    void StartVault()
    {
        isVaulting = true;
        
        // वाल्टिंग के दौरान ग्रेविटी और नॉर्मल मूवमेंट रोकने के लिए प्लेयर स्क्रिप्ट को डिसेबल करें
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;

        Debug.Log("[Vault] दीवार पर चढ़ना शुरू!");
    }

    void PerformVaultMovement()
    {
        // प्लेयर को स्मूथली टारगेट पोजीशन की तरफ ले जाएं (Lerp या MoveTowards)
        transform.position = Vector3.MoveTowards(transform.position, vaultTargetPosition, vaultSpeed * Time.deltaTime);

        // जब प्लेयर टारगेट पोजीशन पर पहुंच जाए, तो वाल्टिंग खत्म करें
        if (Vector3.Distance(transform.position, vaultTargetPosition) < 0.1f)
        {
            EndVault();
        }
    }

    void EndVault()
    {
        isVaulting = false;

        // प्लेयर मूवमेंट को वापस ऑन करें
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = true;

        Debug.Log("[Vault] चढ़ना पूरा हुआ।");
    }
}
