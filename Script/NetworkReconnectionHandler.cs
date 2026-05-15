using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkReconnectionHandler : MonoBehaviour
{
    [Header("Reconnection Settings")]
    public float maxReconnectionTimeout = 30f; // री-कनेक्ट करने के लिए प्लेयर को 30 सेकंड का समय मिलेगा
    public bool isAttemptingReconnection = false;

    private float reconnectionTimer = 0f;
    private Coroutine timeoutCoroutine;

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<NetworkReconnectionHandler>(this);
        }
    }

    // यह मेथड आपका नेटवर्किंग आर्किटेक्चर (Photon/Mirror/Netcode) तब कॉल करेगा जब कनेक्शन लूज़ होगा
    public void OnNetworkConnectionLost()
    {
        if (isAttemptingReconnection) return;

        Debug.LogWarning("[Network] कनेक्शन टूट गया! री-कनेक्ट करने की कोशिश की जा रही है...");
        isAttemptingReconnection = true;
        reconnectionTimer = 0f;

        // 1. गेमप्ले को क्लाइंट-साइड पर होल्ड करें और UI पर "Connecting..." दिखाएं
        ShowReconnectionUI(true);

        // 2. प्लेयर की इनपुट्स को फ्रीज करें ताकि वह हवा में न भागता रहे
        TogglePlayerControls(false);

        // 3. टाइमआउट काउंटडाउन शुरू करें
        timeoutCoroutine = StartCoroutine(ReconnectionTimeoutRoutine());
    }

    IEnumerator ReconnectionTimeoutRoutine()
    {
        while (reconnectionTimer < maxReconnectionTimeout)
        {
            yield return new WaitForSeconds(1f);
            reconnectionTimer++;
            Debug.Log($"[Network] री-कनेक्टिंग... समय बचा है: {maxReconnectionTimeout - reconnectionTimer}s");

            // यहाँ बैकएंड या मास्टर सर्वर को पिंग करने की कोशिश करें
            if (CheckServerPing())
            {
                OnReconnectionSuccess();
                yield break;
            }
        }

        // अगर 30 सेकंड में कनेक्ट नहीं हुआ, तो प्लेयर को बाहर निकालें
        OnReconnectionFailed();
    }

    private bool CheckServerPing()
    {
        // नेटवर्किंग फ्रेमवर्क का पिंग चेक लॉजिक यहाँ आएगा
        // उदाहरण के लिए: return PhotonNetwork.IsConnectedAndReady;
        // अभी टेस्टिंग के लिए हम इसे डमी मान लेते हैं:
        return false; 
    }

    public void OnReconnectionSuccess()
    {
        isAttemptingReconnection = false;
        if (timeoutCoroutine != null) StopCoroutine(timeoutCoroutine);

        ShowReconnectionUI(false);
        TogglePlayerControls(true);

        Debug.LogWarning("[Network] री-कनेक्शन सफल! आप वापस मैच में हैं।");
    }

    private void OnReconnectionFailed()
    {
        isAttemptingReconnection = false;
        Debug.LogError("[Network] री-कनेक्शन फेल! प्लेयर को मुख्य लॉबी में भेजा जा रहा है।");
        
        // स्क्रीन साफ करें और लॉबी सीन लोड करें
        ShowReconnectionUI(false);
        SceneManager.LoadScene("BGMILobby");
    }

    private void ShowReconnectionUI(bool show)
    {
        // HUD UI मैनेजर को कॉल करके "Connection Lost. Retrying..." का पैनल दिखाएं
        HUDManager hud = GameServiceLocator.Instance?.GetService<HUDManager>();
        if (hud != null)
        {
            // hud.ToggleReconnectionPanel(show);
        }
    }

    private void TogglePlayerControls(bool enable)
    {
        PlayerMovement movement = GetComponent<PlayerMovement>();
        WeaponShooting shooting = GetComponent<WeaponShooting>();

        if (movement != null) movement.enabled = enable;
        if (shooting != null) shooting.enabled = enable;
    }
}
