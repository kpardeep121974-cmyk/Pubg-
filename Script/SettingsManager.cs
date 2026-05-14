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
using UnityEngine;
using System;

public class SettingsManager : MonoBehaviour
{
    // BGMI Graphics Enums
    public enum GraphicsQuality { Smooth, Balanced, HD, HDR, UltraHD }
    public enum FrameRate { Low_30FPS, Medium_40FPS, High_60FPS, Extreme_90FPS }
    public enum SensitivityPreset { Low, Medium, High, Custom }

    [System.Serializable]
    public class BGMISettings
    {
        [Header("Graphics & FPS")]
        public GraphicsQuality graphics = GraphicsQuality.Smooth;
        public FrameRate frameRate = FrameRate.High_60FPS;
        public bool aimAssist = true;

        [Header("Sensitivity Settings")]
        public SensitivityPreset preset = SensitivityPreset.Medium;
        public float cameraSensitivity = 100f;  // Free Look (Eye button)
        public float adsSensitivity = 90f;      // Aim Down Sight (Scope)
        public float gyroscopeSensitivity = 150f;

        [Header("Audio Settings")]
        public float masterVolume = 1f;
        public float sfxVolume = 1f;            // Gunshots & Footsteps
        public float voiceChatVolume = 0.8f;
    }

    public BGMISettings currentSettings = new BGMISettings();

    void Start()
    {
        LoadSettings();
        ApplyGraphicsAndFPS();
    }

    // 1. Settings Apply Karne Ka Function
    public void ApplyGraphicsAndFPS()
    {
        // FPS Set Karna (BGMI ki tarah smooth chalne ke liye)
        int targetFPS = 60;
        switch (currentSettings.frameRate)
        {
            case FrameRate.Low_30FPS: targetFPS = 30; break;
            case FrameRate.Medium_40FPS: targetFPS = 40; break;
            case FrameRate.High_60FPS: targetFPS = 60; break;
            case FrameRate.Extreme_90FPS: targetFPS = 90; break;
        }
        Application.targetFrameRate = targetFPS;

        // Unity Quality Settings (Graphics Level)
        int qualityLevel = (int)currentSettings.graphics;
        QualitySettings.SetQualityLevel(qualityLevel, true);

        Debug.Log($"BGMI Settings Applied: Graphics={currentSettings.graphics}, Target FPS={targetFPS}");
    }

    // 2. Sensitivity Preset Change Karna
    public void ChangeSensitivityPreset(SensitivityPreset newPreset)
    {
        currentSettings.preset = newPreset;
        if (newPreset == SensitivityPreset.Low)
        {
            currentSettings.cameraSensitivity = 60f;
            currentSettings.adsSensitivity = 50f;
        }
        else if (newPreset == SensitivityPreset.Medium)
        {
            currentSettings.cameraSensitivity = 100f;
            currentSettings.adsSensitivity = 90f;
        }
        else if (newPreset == SensitivityPreset.High)
        {
            currentSettings.cameraSensitivity = 140f;
            currentSettings.adsSensitivity = 120f;
        }
        
        SaveSettings();
    }

    // 3. Settings Save Karna (Local Storage mein)
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(currentSettings);
        PlayerPrefs.SetString("BGMISettingsData", json);
        PlayerPrefs.Save();
    }

    // 4. Settings Load Karna
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("BGMISettingsData"))
        {
            string json = PlayerPrefs.GetString("BGMISettingsData");
            currentSettings = JsonUtility.FromJson<BGMISettings>(json);
        }
        else
        {
            // Default BGMI settings agar pehli baar game khula hai
            ChangeSensitivityPreset(SensitivityPreset.Medium);
        }
    }
}
