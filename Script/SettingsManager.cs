using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [Header("BGMI Style Graphics Settings")]
    public string graphicsQuality = "Smooth"; // Options: Smooth, Balanced, HD, UltraHD
    public int frameRateTarget = 60;          // Options: 30, 40, 60, 90 FPS

    [Header("Free Fire Style Sensitivity (0 to 100)")]
    public float generalSensitivity = 95f;   // Look around speed
    public float redDotSensitivity = 80f;
    public float scope2xSensitivity = 70f;
    public float scope4xSensitivity = 50f;
    public float awmSniperSensitivity = 40f;

    [Header("Audio Settings")]
    public float masterVolume = 1.0f;
    public float soundEffectsVolume = 1.0f; // Gunshots and footsteps

    void Start()
    {
        LoadPlayerSettings();
    }

    // 1. Settings ko Load karne ka system
    public void LoadPlayerSettings()
    {
        graphicsQuality = PlayerPrefs.GetString("GraphicsQuality", "Smooth");
        frameRateTarget = PlayerPrefs.GetInt("FrameRateTarget", 60);

        generalSensitivity = PlayerPrefs.GetFloat("SensGeneral", 95f);
        redDotSensitivity = PlayerPrefs.GetFloat("SensRedDot", 80f);
        scope4xSensitivity = PlayerPrefs.GetFloat("Sens4x", 50f);
        awmSniperSensitivity = PlayerPrefs.GetFloat("SensAWM", 40f);

        Debug.Log("Luxury Settings Loaded! Current Graphics: " + graphicsQuality + " | General Sens: " + generalSensitivity);
        ApplyGraphicsSettings();
    }

    // 2. Sensitivity Update karne ka function (Sliders ke liye)
    public void UpdateSensitivity(string type, float newValue)
    {
        newValue = Mathf.Clamp(newValue, 0f, 100f);

        if (type == "General") { generalSensitivity = newValue; PlayerPrefs.SetFloat("SensGeneral", newValue); }
        else if (type == "RedDot") { redDotSensitivity = newValue; PlayerPrefs.SetFloat("SensRedDot", newValue); }
        else if (type == "Scope4x") { scope4xSensitivity = newValue; PlayerPrefs.SetFloat("Sens4x", newValue); }
        else if (type == "AWM") { awmSniperSensitivity = newValue; PlayerPrefs.SetFloat("SensAWM", newValue); }

        Debug.Log(type + " Sensitivity Updated to: " + newValue);
    }

    // 3. Graphics Change karne ka function
    public void SetGraphicsQuality(string quality, int fps)
    {
        graphicsQuality = quality;
        frameRateTarget = fps;

        PlayerPrefs.SetString("GraphicsQuality", quality);
        PlayerPrefs.SetInt("FrameRateTarget", fps);

        ApplyGraphicsSettings();
    }

    void ApplyGraphicsSettings()
    {
        // Target FPS set karna mobile device ke liye
        Application.targetFrameRate = frameRateTarget;
        Debug.Log("Graphics Engine Applied: " + graphicsQuality + " Mode at " + frameRateTarget + " FPS.");
    }
}
