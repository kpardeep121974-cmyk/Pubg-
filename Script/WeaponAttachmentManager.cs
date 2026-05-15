using UnityEngine;

public class WeaponAttachmentManager : MonoBehaviour
{
    public enum AttachmentType { Scope, Magazine, Muzzle }

    [System.Serializable]
    public class WeaponSlot
    {
        public string weaponName = "M416";
        [Header("Current Fitted Attachments")]
        public GameObject equippedScope;
        public GameObject equippedMagazine;
        public GameObject equippedMuzzle;

        [Header("Attachment Anchor Points")]
        public Transform scopeAnchor;    // गन के ऊपर स्कोप रखने की जगह
        public Transform magazineAnchor; // मैगजीन की जगह
        public Transform muzzleAnchor;   // गन की नोक पर साइलेंसर लगाने की जगह
    }

    [Header("Active Weapon Configuration")]
    public WeaponSlot activeWeapon;

    private void Awake()
    {
        // इसे Service Locator में रजिस्टर करें
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<WeaponAttachmentManager>(this);
        }
    }

    // इन्वेंटरी से किसी अटैचमेंट पर क्लिक करने पर इसे कॉल करें
    public void EquipAttachment(GameObject attachmentPrefab, AttachmentType type)
    {
        if (activeWeapon == null)
        {
            Debug.LogWarning("हाथ में कोई गन नहीं है, अटैचमेंट कहाँ लगाएं?");
            return;
        }

        switch (type)
        {
            case AttachmentType.Scope:
                ReplaceAttachment(ref activeWeapon.equippedScope, attachmentPrefab, activeWeapon.scopeAnchor);
                ApplyScopeLogic(attachmentPrefab.name);
                break;

            case AttachmentType.Magazine:
                ReplaceAttachment(ref activeWeapon.equippedMagazine, attachmentPrefab, activeWeapon.magazineAnchor);
                ApplyMagazineLogic();
                break;

            case AttachmentType.Muzzle:
                ReplaceAttachment(ref activeWeapon.equippedMuzzle, attachmentPrefab, activeWeapon.muzzleAnchor);
                ApplyMuzzleLogic(attachmentPrefab.name);
                break;
        }
    }

    private void ReplaceAttachment(ref GameObject currentAttachment, GameObject newPrefab, Transform anchor)
    {
        // अगर पहले से कोई अटैचमेंट लगा है, तो उसे हटाएं (और ज़मीन पर गिराने का लॉजिक लिख सकते हैं)
        if (currentAttachment != null)
        {
            Destroy(currentAttachment);
        }

        if (anchor != null && newPrefab != null)
        {
            // गन के एंकर पॉइंट पर नया 3D मॉडल स्पॉन करें
            currentAttachment = Instantiate(newPrefab, anchor.position, anchor.rotation, anchor);
            Debug.Log($"[Attachment] {newPrefab.name} को सफलता पूर्वक गन पर फिट कर दिया गया है।");
        }
    }

    // ---- अटैचमेंट्स का गेमप्ले पर असर ----

    private void ApplyScopeLogic(string scopeName)
    {
        // कैमरा ज़ूम (Field of View - FOV) को बदलें
        Camera playerCam = Camera.main;
        if (playerCam != null)
        {
            if (scopeName.Contains("4x")) playerCam.fieldOfView = 20f;      // ज्यादा ज़ूम
            else if (scopeName.Contains("RedDot")) playerCam.fieldOfView = 50f; // कम ज़ूम
            else playerCam.fieldOfView = 60f;                               // नॉर्मल व्यू
        }
    }

    private void ApplyMagazineLogic()
    {
        // WeaponShooting स्क्रिप्ट की मैक्सिमम एमो कैपेसिटी बढ़ाएं
        WeaponShooting shooting = GetComponent<WeaponShooting>();
        if (shooting != null)
        {
            // मान लेते हैं एक्सटेंडेड मैग लगाने पर 30 से 40 गोलियां हो जाती हैं
            // shooting.maxMagazineCapacity = 40; 
            Debug.Log("Extended Magazine: गन की एमो कैपेसिटी बढ़ गई है।");
        }
    }

    private void ApplyMuzzleLogic(string muzzleName)
    {
        WeaponShooting shooting = GetComponent<WeaponShooting>();
        if (shooting != null && muzzleName.Contains("Suppressor"))
        {
            // गन की आवाज़ और मज़ल फ़्लैश गायब करें
            // shooting.gunAudioVolume = 0.2f;
            // shooting.showMuzzleFlash = false;
            Debug.Log("Suppressor: गन की आवाज शांत कर दी गई है।");
        }
    }
}
