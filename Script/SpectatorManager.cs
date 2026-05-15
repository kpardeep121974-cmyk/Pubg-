using System.Collections.Generic;
using UnityEngine;

public class SpectatorManager : MonoBehaviour
{
    [Header("Spectator Settings")]
    public List<Transform> targetTeammates = new List<Transform>(); // जीवित टीममेट्स की लिस्ट
    public float cameraSmoothSpeed = 5f;
    public Vector3 cameraOffset = new Vector3(0, 3f, -5f); // प्लेयर के पीछे कैमरे की पोजीशन

    private int currentTargetIndex = 0;
    private bool isSpectating = false;
    private Camera spectatorCamera;

    private void Awake()
    {
        // इसे Service Locator में रजिस्टर करें ताकि HealthSystem इसे एक्सेस कर सके
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<SpectatorManager>(this);
        }

        spectatorCamera = GetComponent<Camera>();
        if (spectatorCamera != null)
        {
            spectatorCamera.enabled = false; // गेम की शुरुआत में यह कैमरा बंद रहेगा
        }
    }

    // जब प्लेयर एलिमिनेट होगा, तब HealthSystem इस मेथड को कॉल करेगा
    public void StartSpectating(List<Transform> teammates)
    {
        targetTeammates = teammates;

        if (targetTeammates == null || targetTeammates.Count == 0)
        {
            Debug.Log("कोई टीममेट जीवित नहीं है। लॉबी में वापस जा रहे हैं...");
            // यहाँ आप अपने LobbyManager को कॉल करके प्लेयर को बाहर भेज सकते हैं
            return;
        }

        isSpectating = true;
        if (spectatorCamera != null) spectatorCamera.enabled = true;
        
        currentTargetIndex = 0;
        Debug.WriteLine("स्पेक्टेटर मोड एक्टिवेट हो गया है।");
    }

    void LateUpdate()
    {
        if (!isSpectating || targetTeammates.Count == 0) return;

        // अगर करंट टारगेट प्लेयर बीच में डिस्कनेक्ट या मर जाए, तो लिस्ट साफ करें
        if (targetTeammates[currentTargetIndex] == null)
        {
            SwitchToNextTarget();
            return;
        }

        // 1. कैमरे को टारगेट प्लेयर के पीछे स्मूथली मूव कराना (Third Person View)
        Transform target = targetTeammates[currentTargetIndex];
        Vector3 targetPosition = target.position + target.TransformDirection(cameraOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSmoothSpeed * Time.deltaTime);

        // 2. कैमरे का रोटेशन प्लेयर की तरफ रखना
        transform.LookAt(target.position + Vector3.up * 1.5f);

        // 3. माउस क्लिक या Left/Right Arrow से टीममेट्स स्विच करना
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchToNextTarget();
        }
    }

    private void SwitchToNextTarget()
    {
        // लिस्ट में से मरे हुए या नल प्लेयर्स को हटाना
        targetTeammates.RemoveAll(item => item == null);

        if (targetTeammates.Count == 0)
        {
            Debug.Log("अब कोई भी टीममेट नहीं बचा।");
            isSpectating = false;
            return;
        }

        currentTargetIndex = (currentTargetIndex + 1) % targetTeammates.Count;
        Debug.Log($"अब आप देख रहे हैं: {targetTeammates[currentTargetIndex].name}");
    }
}
