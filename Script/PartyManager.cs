using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [System.Serializable]
    public class PartyMember
    {
        public string playerID;
        public string playerName;
        public bool isLeader;
    }

    [Header("Party Settings")]
    public int maxPartySize = 4;
    public List<PartyMember> currentParty = new List<PartyMember>();
    
    private string localPlayerID = "Player_123"; // टेस्टिंग के लिए लोकल ID
    private string localPlayerName = "AbhiPro";

    private void Awake()
    {
        // इसे Service Locator में रजिस्टर करें ताकि Lobby UI इसे ढूंढ सके
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<PartyManager>(this);
        }
    }

    void Start()
    {
        // गेम शुरू होने पर प्लेयर अपनी खुद की अकेली पार्टी (Solo) का लीडर होता है
        CreateParty();
    }

    public void CreateParty()
    {
        currentParty.Clear();
        PartyMember leader = new PartyMember
        {
            playerID = localPlayerID,
            playerName = localPlayerName,
            isLeader = true
        };
        currentParty.Add(leader);
        Debug.Log("[PartyManager] नई पार्टी बनाई गई! आप लीडर हैं।");
    }

    // जब आप किसी दोस्त को इनवाइट भेजते हैं
    public void SendInvite(string targetFriendID)
    {
        if (currentParty.Count >= maxPartySize)
        {
            Debug.LogWarning("पार्टी पहले से ही फुल है!");
            return;
        }

        // नेटवर्क के ज़रिए इनवाइट भेजें (जैसे Photon RPC, Mirror, या Backend Webhook)
        Debug.Log($"[PartyManager] Player {targetFriendID} को इनवाइट भेजा गया।");
        
        // यहाँ आप अपने नेटवर्क मैनेजर को कॉल करेंगे:
        // NetworkManager.Instance.SendInviteRPC(targetFriendID, localPlayerID);
    }

    // जब आपके पास कोई इनवाइट आता है और आप 'Accept' दबाते हैं
    public void ReceiveAndAcceptInvite(string hostPlayerID, string hostName)
    {
        Debug.Log($"{hostName} का इनवाइट एक्सेप्ट किया गया। पार्टी जॉइन कर रहे हैं...");
        
        // पुरानी सोलो पार्टी छोड़ें और नई पार्टी के डेटा को सिंक करें
        JoinPartyOnServer(hostPlayerID);
    }

    private void JoinPartyOnServer(string partyID)
    {
        // सर्वर से पार्टी के बाकी मेंबर्स का डेटा मंगाकर लिस्ट अपडेट करें
        // अभी के लिए डमी डेटा जोड़ रहे हैं:
        PartyMember newMember = new PartyMember
        {
            playerID = localPlayerID,
            playerName = localPlayerName,
            isLeader = false
        };
        currentParty.Add(newMember);
        
        UpdateLobbyUI();
    }

    public void KickMember(string memberID)
    {
        // सिर्फ लीडर ही किसी को किक कर सकता है
        if (!AmILeader()) return;

        currentParty.RemoveAll(m => m.playerID == memberID);
        Debug.Log($"[PartyManager] Player {memberID} को टीम से निकाल दिया गया।");
        UpdateLobbyUI();
    }

    private bool AmILeader()
    {
        PartyMember me = currentParty.Find(m => m.playerID == localPlayerID);
        return me != null && me.isLeader;
    }

    private void UpdateLobbyUI()
    {
        // आपके BGMILobbyManager या UI स्क्रिप्ट को सूचित करेगा कि स्क्रीन पर 3D मॉडल्स अपडेट करें
        Debug.Log($"[PartyManager] UI अपडेटेड! पार्टी में अभी {currentParty.Count} प्लेयर्स हैं।");
    }
}
