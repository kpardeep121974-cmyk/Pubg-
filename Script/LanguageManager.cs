using UnityEngine;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
    public enum Language { English, Hindi, Spanish, Portuguese }
    public Language currentLanguage = Language.English;

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<LanguageManager>(this);
        }
    }

    void Start()
    {
        // प्लेयर के डिवाइस की भाषा को ऑटो-डिटेक्ट करें
        AutoDetectDeviceLanguage();
    }

    void AutoDetectDeviceLanguage()
    {
        SystemLanguage systemLang = Application.systemLanguage;

        switch (systemLang)
        {
            case SystemLanguage.Hindi:
                currentLanguage = Language.Hindi;
                break;
            case SystemLanguage.Spanish:
                currentLanguage = Language.Spanish;
                break;
            case SystemLanguage.Portuguese:
                currentLanguage = Language.Portuguese;
                break;
            default:
                currentLanguage = Language.English; // डिफ़ॉल्ट ग्लोबल भाषा
                break;
        }

        Debug.Log($"[Localization] ग्लोबल प्लेयर के लिए भाषा चुनी गई: {currentLanguage}");
    }

    // UI टेक्स्ट को बदलने के लिए इस मेथड का इस्तेमाल करें
    public string GetLocalizedString(string key)
    {
        // यहाँ आप एक डिक्शनरी बना सकते हैं जो 'Play Button' को हिंदी में 'खेलें' और स्पेनिश में 'Jugar' दिखाए
        return key; 
    }
}
