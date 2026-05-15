using UnityEngine;

public class SpeedHackDetector : MonoBehaviour
{
    [Header("Movement Limits")]
    public float maxAllowedSpeed = 12f; // एक सेकंड में प्लेयर अधिकतम 12 मीटर भाग सकता है (बिना गाड़ी के)
    public float checkInterval = 1.0f;  // हर 1 सेकंड में चेक करें

    [Header("Anti-Cheat Punishment")]
    public int maxViolationsAllowed = 3;
    private int currentViolations = 0;

    private Vector3 lastPosition;
    private float nextCheckTime;
    private CharacterController characterController;
    private BattleRoyaleVehicleController vehicleController;

    void Start()
    {
        lastPosition = transform.position;
        nextCheckTime = Time.time + checkInterval;
        characterController = GetComponent<CharacterController>();
        vehicleController = GetComponentInParent<BattleRoyaleVehicleController>();
    }

    void Update()
    {
        // परफॉर्मेंस बचाने के लिए हर सेकंड चेक करें
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            ValidatePlayerMovement();
        }
    }

    void ValidatePlayerMovement()
    {
        // अपवाद (Exception): अगर प्लेयर गाड़ी के अंदर बैठा है, तो स्पीड हैक चेक न करें
        // क्योंकि गाड़ी की स्पीड प्लेयर की पैदल स्पीड से ज़्यादा होगी
        if (transform.parent != null && transform.parent.GetComponent<BattleRoyaleVehicleController>() != null)
        {
            // गाड़ी में होने पर पोजीशन अपडेट करते रहें ताकि बाहर आते ही गलत फ्लैग न हो
            lastPosition = transform.position;
            return;
        }

        // 1. पिछले 1 सेकंड में तय की गई दूरी नापें (Y एक्सिस यानी गिरने/कूदने को छोड़कर)
        Vector3 currentPos = transform.position;
        currentPos.y = 0;
        Vector3 oldPos = lastPosition;
        oldPos.y = 0;

        float distanceMoved = Vector3.Distance(currentPos, oldPos);

        // 2. अगर दूरी तय सीमा से ज़्यादा है, तो यह स्पीड हैक हो सकता है
        if (distanceMoved > maxAllowedSpeed)
        {
            currentViolations++;
            Debug.LogWarning($"[ANTI-CHEAT] संदिग्ध मूवमेंट डिटेक्ट हुई! तय दूरी: {distanceMoved}m. Violations: {currentViolations}");

            // प्लेयर को वापस उसकी पुरानी सही पोजीशन पर टेलीपोर्ट (Rubberband) कर दें
            RollbackPosition();

            if (currentViolations >= maxViolationsAllowed)
            {
                PunishPlayer();
            }
        }
        else
        {
            // अगर सब ठीक है, तो वियोलेशन धीरे-धीरे कम करें और नई पोजीशन सेव करें
            if (currentViolations > 0) currentViolations--;
            lastPosition = transform.position;
        }
    }

    void RollbackPosition()
    {
        // हैकर को आगे भागने से रोककर पीछे खींचें
        if (characterController != null) characterController.enabled = false;
        transform.position = lastPosition;
        if (characterController != null) characterController.enabled = true;
    }

    void PunishPlayer()
    {
        Debug.LogError("[ANTI-CHEAT] प्लेयर को स्पीड हैक के कारण गेम से किक किया जा रहा है!");
        
        // नेटवर्क से डिस्कनेक्ट करने का लॉजिक (जैसे: PhotonNetwork.Disconnect() या Server Kick RPC)
        // यहाँ आप प्लेयर को लॉबी सीन में वापस भेज सकते हैं
        UnityEngine.SceneManagement.SceneManager.LoadScene("BGMILobby");
    }
}
