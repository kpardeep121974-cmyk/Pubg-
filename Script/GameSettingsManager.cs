using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    [Header("Sensitivity Settings")]
    public float mouseSensitivity = 2.0f;

    [Header("Graphics Settings")]
    public int graphicsQualityIndex = 2; // 0 = Low, 1 = Medium, 2 = High/Ultra

    [Header("Audio Settings")]
    public float masterVolume = 1.0f; // 0.0f से 1.0f के बीच

    private void Awake()
    {
        // इसे central Service Locator में रजिस्टर करें
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<GameSettingsManager>(this);
        }
    }

    void Start()
    {
        // गेम शुरू होते ही प्लेयर की पुरानी सेव की हुई सेटिंग्स लोड करें
        LoadSettings();
    }

    // ---- सेंसिटिविटी कंट्रोल ----
    public void SetSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        
        // कैमरा स्क्रिप्ट को तुरंत सूचित करें (अगर आपके पास MouseLook या CameraController है)
        UpdateCameraSensitivity();
    }

    private void UpdateCameraSensitivity()
    {
        // मान लेते हैं आपके कैमरा कंट्रोलर का नाम CameraController है
        // CameraController cam = FindObjectOfType<CameraController>();
        // if (cam != null) cam.sensitivity = mouseSensitivity;
        Debug.Log($"[Settings] माउस सेंसिटिविटी अपडेट हुई: {mouseSensitivity}");
    }

    // ---- ग्राफिक्स कंट्रोल ----
    public void SetGraphicsQuality(int qualityIndex)
    {
        graphicsQualityIndex = qualityIndex;
        // Unity के इन-बिल्ट Quality Settings का इस्तेमाल करके ग्राफिक्स बदलें
        QualitySettings.SetQualityLevel(qualityIndex, true);
        
        PlayerPrefs.SetInt("GraphicsQuality", graphicsQualityIndex);
        Debug.Log($"[Settings] ग्राफिक्स क्वालिटी सेट की गई: {QualitySettings.names[qualityIndex]}");
    }

    // ---- ऑडियो कंट्रोल ----
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        // पूरे गेम की आवाज़ को कंट्रोल करें
        AudioListener.volume = masterVolume;
        
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        Debug.Log($"[Settings] मास्टर वॉल्यूम सेट किया गया: {masterVolume * 100}%");
    }

    // ---- डेटा सेव और लोड ----
    private void SaveAllSettings()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.SetInt("GraphicsQuality", graphicsQualityIndex);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2.0f);
        graphicsQualityIndex = PlayerPrefs.GetInt("GraphicsQuality", 2);
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);

        // लोड किए गए डेटा को गेम पर अप्लाई करें
        QualitySettings.SetQualityLevel(graphicsQualityIndex, true);
        AudioListener.volume = masterVolume;
        UpdateCameraSensitivity();
    }
}
