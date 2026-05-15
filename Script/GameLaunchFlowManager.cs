using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLaunchFlowManager : MonoBehaviour
{
    [Header("UI Panels (Canvas Groups for Fading)")]
    public CanvasGroup splashScreenGroup;
    public CanvasGroup loginScreenGroup;
    public CanvasGroup loadingScreenGroup;

    [Header("Lobby Camera Animation")]
    public Camera mainMenuCamera;
    public Transform cameraLoginPosition; // लॉगिन के समय कैमरे की पोजीशन (थोड़ी दूर)
    public Transform cameraLobbyPosition; // लॉबी में आने के बाद कैमरे की पोजीशन (कैरेक्टर के पास सिनेमैटिक ज़ूम)
    public float cameraZoomSpeed = 2.0f;

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<GameLaunchFlowManager>(this);
        }
    }

    void Start()
    {
        // गेम स्टार्ट होते ही सबसे पहले स्प्लैश स्क्रीन और लॉगिन फ्लो शुरू करें
        StartCoroutine(ExecuteGameLaunchFlow());
    }

    IEnumerator ExecuteGameLaunchFlow()
    {
        // 1. शुरुआत में सिर्फ स्प्लैश स्क्रीन दिखेगी, बाकी सब छुपे रहेंगे
        splashScreenGroup.alpha = 1;
        loginScreenGroup.alpha = 0;
        loadingScreenGroup.alpha = 0;
        if (mainMenuCamera != null && cameraLoginPosition != null)
        {
            mainMenuCamera.transform.position = cameraLoginPosition.position;
            mainMenuCamera.transform.rotation = cameraLoginPosition.rotation;
        }

        // स्प्लैश स्क्रीन को 2 सेकंड तक रोकें (लोगो दिखाने के लिए)
        yield return new WaitForSeconds(2.0f);

        // 2. स्प्लैश स्क्रीन को फेड-आउट करें और लॉगिन स्क्रीन को फेड-इन करें
        yield return StartCoroutine(FadeCanvasGroup(splashScreenGroup, 1, 0, 1.0f));
        yield return StartCoroutine(FadeCanvasGroup(loginScreenGroup, 0, 1, 1.0f));
    }

    // लॉगिन बटन (FB/Google) दबाने पर इस मेथड को UI से कॉल करें
    public void OnLoginButtonPressed()
    {
        StartCoroutine(ProcessLoginAndTransition());
    }

    IEnumerator ProcessLoginAndTransition()
    {
        // 1. लॉगिन स्क्रीन पर लोडिंग दिखाएं
        yield return StartCoroutine(FadeCanvasGroup(loadingScreenGroup, 0, 1, 0.5f));

        // 2. यहाँ आपका CloudSaveManager या Auth सर्वर चेक करेगा (हम 2 सेकंड का डमी होल्ड ले रहे हैं)
        yield return new WaitForSeconds(2.0f); 

        // 3. लॉगिन सफल! अब लॉगिन और लोडिंग स्क्रीन दोनों को गायब करें
        StartCoroutine(FadeCanvasGroup(loginScreenGroup, 1, 0, 0.8f));
        yield return StartCoroutine(FadeCanvasGroup(loadingScreenGroup, 1, 0, 0.8f));

        // 4. 🚀 सिनेमैटिक कैमरा एनीमेशन (कैमरे को लॉगिन व्यू से लॉबी व्यू पर स्मूथली ले जाएं)
        float elapsedTime = 0;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * cameraZoomSpeed;
            mainMenuCamera.transform.position = Vector3.Lerp(cameraLoginPosition.position, cameraLobbyPosition.position, elapsedTime);
            mainMenuCamera.transform.rotation = Quaternion.Lerp(cameraLoginPosition.rotation, cameraLobbyPosition.rotation, elapsedTime);
            yield return null;
        }

        Debug.LogWarning("🎉 [Flow Manager] प्लेयर सफलता पूर्वक मेन लॉबी में आ चुका है। एनीमेशन कम्प्लीट!");
        
        // यहाँ आप लॉबी के बाकी UI (जैसे फ्रेंड्स लिस्ट, स्टोर बटन, मैचमेकिंग बटन) को एक्टिवेट कर सकते हैं
    }

    // यूनिटी में किसी भी UI को स्मूथली गायब या प्रकट करने का हेल्पर फंक्शन
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        float timeMax = 0;
        while (timeMax < 1.0f)
        {
            timeMax += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, timeMax);
            yield return null;
        }
        cg.alpha = endAlpha;
    }
}
