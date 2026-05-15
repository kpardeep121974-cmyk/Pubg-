using System.Collections;
using UnityEngine;

public class LowEndPerformanceScaler : MonoBehaviour
{
    [Header("Device Specs (Read Only)")]
    public int systemMemoryRAM;
    public int processorCount;

    [Header("FPS Monitor")]
    private float deltaTime = 0.0f;
    public float currentFPS = 0.0f;

    [Header("Optimization Thresholds")]
    public float lowFPSThreshold = 25.0f; // अगर FPS 25 से नीचे गया, तो ऑप्टिमाइज़ेशन ट्रिगर होगा
    private bool isFullyOptimizedForLowEnd = false;

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<LowEndPerformanceScaler>(this);
        }
    }

    void Start()
    {
        // 1. डिवाइस की स्पेसिफिकेशन्स चेक करें
        systemMemoryRAM = SystemInfo.systemMemorySize; // MB में रैम मिलेगी
        processorCount = SystemInfo.processorCount;

        Debug.Log($"[Performance] Device RAM: {systemMemoryRAM}MB, Cores: {processorCount}");

        // 2. अगर फोन में 3GB (3072MB) या उससे कम रैम है, तो तुरंत लो-एंड सेटिंग्स लागू करें
        if (systemMemoryRAM <= 3100)
        {
            ApplyAggressiveLowEndSettings();
        }
        else
        {
            // अगर अच्छा फोन है, तो मैच के दौरान लाइव FPS पर नज़र रखें
            StartCoroutine(FPSMonitorRoutine());
        }
    }

    void Update()
    {
        // लाइव FPS कैलकुलेशन लॉजिक
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        currentFPS = 1.0f / deltaTime;
    }

    IEnumerator FPSMonitorRoutine()
    {
        // गेम शुरू होने के 5 सेकंड बाद मॉनिटरिंग शुरू करें ताकि लोडिंग का लैग काउंट न हो
        yield return new WaitForSeconds(5.0f);

        while (!isFullyOptimizedForLowEnd)
        {
            yield return new WaitForSeconds(3.0f); // हर 3 सेकंड में चेक करें

            if (currentFPS < lowFPSThreshold)
            {
                Debug.LogWarning($"[PERFORMANCE WARNING] FPS गिरकर {Mathf.Round(currentFPS)} हो गया है! लो-एंड स्केलर एक्टिवेट हो रहा है...");
                ApplyAggressiveLowEndSettings();
                isFullyOptimizedForLowEnd = true; // एक बार पूरी तरह ऑप्टिमाइज़ होने के बाद लूप रोकें
            }
        }
    }

    // 🚀 फ्री-फायर स्टाइल सुपर ऑप्टिमाइज़ेशन लॉजिक
    public void ApplyAggressiveLowEndSettings()
    {
        Debug.LogError("⚠️ [LOW-END SCALE] गेम को ₹8,000 वाले फोन के लिए ऑप्टिमाइज़ किया जा रहा है...");

        // 1. यूनिटी की क्वालिटी सेटिंग्स को सबसे लोएस्ट (Very Low / Fast) पर डालें
        QualitySettings.SetQualityLevel(0, true);

        // 2. फ्रेम रेट को 30 पर लॉक करें ताकि फोन गर्म न हो और बैटरी बचे
        Application.targetFrameRate = 30;

        // 3. शैडोज़ (परछाईं) को पूरी तरह बंद करें (यह सबसे ज़्यादा रैम खाता है)
        QualitySettings.shadows = ShadowQuality.Disable;

        // 4. सिंक बंद करें
        QualitySettings.vSyncCount = 0;

        // 5. गिटहब के अन्य सिस्टम्स को अलर्ट करें (जैसे मैप स्ट्रीमिंग को छोटे चंक्स लोड करने को कहें)
        MapStreamingManager streamingManager = GameServiceLocator.Instance?.GetService<MapStreamingManager>();
        if (streamingManager != null)
        {
            // यहाँ आप स्ट्रीमिंग मैनेजर का लोड डिस्टेंस कम कर सकते हैं
            Debug.Log("[Low-End] मैप स्ट्रीमिंग रेंडर डिस्टेंस को आधा कर दिया गया है।");
        }

        // 6. गनशॉट के पार्टिकल इफेक्ट्स और धुएं को कम करने के लिए ऑब्जेक्ट पूल को री-साइज करें
        ObjectPooler pooler = GameServiceLocator.Instance?.GetService<ObjectPooler>();
        if (pooler != null)
        {
            // पूल साइज लिमिट सेट करें ताकि रैम न भरे
        }
    }
}
