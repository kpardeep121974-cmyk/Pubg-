using System;
using System.Collections;
using UnityEngine;

public class FlashSaleFOMOManager : MonoBehaviour
{
    [System.Serializable]
    public class FlashOffer
    {
        public string offerID;
        public string itemDescription;
        public float originalPrice;
        public float discountedPrice;
        public float durationInSeconds = 3600f; // 1 घंटा
        public bool isActive = false;
    }

    public FlashOffer currentPersonalOffer;
    private DateTime offerEndTime;

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<FlashSaleFOMOManager>(this);
        }
    }

    // मैच खत्म होने पर या लॉबी में आने पर इस मेथड को ट्रिगर करें
    public void TriggerRandomFlashSale()
    {
        if (currentPersonalOffer.isActive) return; // अगर पहले से ऑफर चल रहा है तो नया न दें

        // 🚀 अल्टीमेट रेवेन्यू लॉजिक: प्लेयर को 80% से 90% का भारी डिस्काउंट दिखाएं
        currentPersonalOffer.offerID = "FOMO_BUNDLE_" + UnityEngine.Random.Range(100, 999);
        currentPersonalOffer.itemDescription = "अल्ट्रा रेयर क्रिमसन बंडल + 500 डायमंड्स";
        currentPersonalOffer.originalPrice = 1999f;
        currentPersonalOffer.discountedPrice = 199f; // सिर्फ ₹199 में (FOMO ट्रिगर)
        currentPersonalOffer.isActive = true;

        offerEndTime = DateTime.UtcNow.AddSeconds(currentPersonalOffer.durationInSeconds);
        
        Debug.LogWarning($"🔥 [FOMO SALE] स्पेशल फ्लैश सेल शुरू! {currentPersonalOffer.itemDescription} केवल ₹{currentPersonalOffer.discountedPrice} में! समय सीमित है!");
        
        StartCoroutine(FlashSaleCountdownRoutine());
    }

    IEnumerator FlashSaleCountdownRoutine()
    {
        while (DateTime.UtcNow < offerEndTime)
        {
            TimeSpan remainingTime = offerEndTime - DateTime.UtcNow;
            // यहाँ आप अपने लॉबी UI के टाइमर टेक्स्ट को अपडेट करेंगे (जैसे: 59m 54s बचे हैं)
            // Debug.Log($"[FOMO Timer] समय शेष: {remainingTime.Minutes}m {remainingTime.Seconds}s");
            yield return new WaitForSeconds(1f);
        }

        // समय समाप्त, ऑफर खत्म! अब प्लेयर इसे इस दाम में कभी नहीं खरीद पाएगा
        EndFlashSale();
    }

    private void EndFlashSale()
    {
        currentPersonalOffer.isActive = false;
        Debug.LogError("[FOMO SALE] ऑफर का समय समाप्त हो गया! स्टोर से आइटम हटा दिया गया है।");
        // UI से बैनर छुपाएं
    }
}
