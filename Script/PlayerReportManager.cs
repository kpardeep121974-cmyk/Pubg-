using System.Collections.Generic;
using UnityEngine;

public class PlayerReportManager : MonoBehaviour
{
    public enum ReportReason { Cheating, ToxicBehavior, TeamKilling, VerbalAbuse }

    [System.Serializable]
    public class ReportTicket
    {
        public string reportedPlayerID;
        public string reportedPlayerName;
        public ReportReason reason;
        public string matchID;
        public System.DateTime reportTime;
    }

    [Header("Server Report Log (Simulated)")]
    public List<ReportTicket> globalReportsLog = new List<ReportTicket>();

    [Header("Ban Configuration")]
    public int reportsThresholdForBan = 5; // अगर एक प्लेयर को 5 बार रिपोर्ट किया गया, तो एक्शन लिया जाएगा

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<PlayerReportManager>(this);
        }
    }

    // मैच एंड स्क्रीन पर जब कोई प्लेयर 'Report' बटन दबाएगा
    public void SubmitReport(string targetID, string targetName, ReportReason reason, string currentMatchID)
    {
        ReportTicket newTicket = new ReportTicket
        {
            reportedPlayerID = targetID,
            reportedPlayerName = targetName,
            reason = reason,
            matchID = currentMatchID,
            reportTime = System.DateTime.UtcNow
        };

        // 🚀 बैकएंड नोट: यहाँ यह डेटा सीधे आपके डेटाबेस (जैसे Firebase/PlayFab) पर जाएगा
        globalReportsLog.Add(newTicket);
        Debug.LogWarning($"[Report System] प्लेयर {targetName} ({targetID}) को '{reason}' के लिए रिपोर्ट किया गया है।");

        // चेक करें कि इस प्लेयर के खिलाफ कुल कितनी रिपोर्ट्स आ चुकी हैं
        CheckPlayerReportCount(targetID);
    }

    private void CheckPlayerReportCount(string playerID)
    {
        // इस प्लेयर की सभी रिपोर्ट्स को ढूंढें
        List<ReportTicket> playerTickets = globalReportsLog.FindAll(t => t.reportedPlayerID == playerID);

        if (playerTickets.Count >= reportsThresholdForBan)
        {
            AutoBanPlayer(playerID);
        }
    }

    private void AutoBanPlayer(string playerID)
    {
        Debug.LogError($"[ANTI-TOXICITY] प्लेयर ID: {playerID} को लगातार आ रही रिपोर्ट्स के कारण 24 घंटे के लिए ऑटो-बैन किया जाता है!");

        // 🚀 सर्वर साइड लॉजिक:
        // 1. प्लेयर के अकाउंट स्टेटस को डेटाबेस में 'Banned = true' करें।
        // 2. अगर वह इस समय ऑनलाइन है, तो उसे तुरंत सर्वर से किक (Disconnect) कर दें।
        
        // डमी नोटिफिकेशन भेजने के लिए:
        MailBoxManager mailbox = GameServiceLocator.Instance?.GetService<MailBoxManager>();
        if (mailbox != null)
        {
            // सिस्टम को पता चल जाएगा कि यह हैकर या टॉक्सिक प्लेयर है
        }
    }
}
