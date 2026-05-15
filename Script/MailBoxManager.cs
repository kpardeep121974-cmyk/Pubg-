using System.Collections.Generic;
using UnityEngine;

public class MailBoxManager : MonoBehaviour
{
    [System.Serializable]
    public class MailMessage
    {
        public string mailID;
        public string title;
        public string messageContent;
        public int rewardCoins;
        public bool isRead = false;
        public bool isClaimed = false;
    }

    [Header("Player Inbox")]
    public List<MailMessage> inbox = new List<MailMessage>();

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<MailBoxManager>(this);
        }
    }

    void Start()
    {
        // गेम शुरू होने पर सर्वर से मेल्स लोड करें (अभी हम डमी मेल्स जोड़ रहे हैं)
        FetchMailsFromServer();
    }

    void FetchMailsFromServer()
    {
        // उदाहरण के लिए: सर्वर से आया हुआ एक गिफ्ट मेल
        MailMessage systemMail = new MailMessage
        {
            mailID = "MAIL_001",
            title = "सर्वर मेंटेनेंस रिवॉर्ड!",
            messageContent = "असुविधा के लिए खेद है। कृपया अपनी फ्री करेंसी क्लेम करें।",
            rewardCoins = 1000,
            isRead = false,
            isClaimed = false
        };

        inbox.Add(systemMail);
        Debug.Log($"[Mailbox] नए मेल्स लोड हुए। कुल मेल्स: {inbox.Count}");
    }

    // जब प्लेयर लिस्ट में से किसी मेल पर क्लिक करेगा
    public void ReadMail(string id)
    {
        MailMessage mail = inbox.Find(m => m.mailID == id);
        if (mail != null)
        {
            mail.isRead = true;
            Debug.Log($"[Mail Open] {mail.title}: {mail.messageContent}");
        }
    }

    // 'Claim Reward' बटन दबाने पर
    public void ClaimMailReward(string id)
    {
        MailMessage mail = inbox.Find(m => m.mailID == id);
        
        if (mail == null) return;
        
        if (mail.isClaimed)
        {
            Debug.LogWarning("यह रिवॉर्ड पहले ही क्लेम किया जा चुका है!");
            return;
        }

        if (mail.rewardCoins > 0)
        {
            // हमारे EconomyManager को ढूंढकर प्लेयर के वॉलेट में कॉइन्स जोड़ें
            EconomyManager economy = GameServiceLocator.Instance?.GetService<EconomyManager>();
            if (economy != null)
            {
                economy.EarnGold(mail.rewardCoins);
                mail.isClaimed = true;
                Debug.LogWarning($"[REWARD CLAIMED] सफलता पूर्वक {mail.rewardCoins} कॉइन्स क्लेम कर लिए गए हैं।");
            }
        }

        // अपडेटेड इनबॉक्स स्टेट को लोकल या क्लाउड पर सेव करें
        SaveMailState();
    }

    private void SaveMailState()
    {
        // आप इस लिस्ट को JSON में बदलकर सुरक्षित रूप से क्लाउड या प्लेयर प्रिफ्स में रख सकते हैं
        string jsonInbox = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PlayerInbox", jsonInbox);
        PlayerPrefs.Save();
    }
}
