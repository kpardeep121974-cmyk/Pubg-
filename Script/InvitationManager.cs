using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatchInvite
{
    public string senderUID;
    public string senderName;
    public string gameMode; // "Squad Classic", "TDM", etc.
    public string roomID;   // Agar custom room se invite aaya ho
}

public class InvitationManager : MonoBehaviour
{
    [Header("Connected Core Systems")]
    public UserProfileManager profileSystem;
    public BGMILobbyManager lobbySystem;
    public CustomRoomManager roomSystem;

    [Header("UI Popups")]
    public GameObject inviteNotificationPopup; // Screen par aane wala pop-up
    public UnityEngine.UI.Text inviteDetailsText;

    private MatchInvite activeIncomingInvite;

    // 1. Apne kisi online dost ko lobby/room mein bulane ke liye invite bhejna
    public void SendMatchInvite(string targetFriendUID, string selectedMode)
    {
        if (profileSystem == null) return;

        string myUID = profileSystem.myProfile.characterID;
        string myName = profileSystem.myProfile.playerName;

        Debug.Log("=========================================");
        Debug.Log("📤 INVITE SENT: Sending lobby invitation to Friend ID: " + targetFriendUID);
        Debug.Log("From: " + myName + " | Mode: " + selectedMode);
        Debug.Log("=========================================");
        
        // Real game mein yahan Photon/Mirror Network server par event trigger hota hai
    }

    // 2. Jab koi dost aapko invite bhejega (Server callback simulation)
    public void ReceiveIncomingInvite(string sUID, string sName, string mode, string rID = "")
    {
        activeIncomingInvite = new MatchInvite {
            senderUID = sUID,
            senderName = sName,
            gameMode = mode,
            roomID = rID
        };

        // UI active karna aur details text fill karna
        if (inviteNotificationPopup != null)
        {
            inviteNotificationPopup.SetActive(true);
            if (string.IsNullOrEmpty(rID))
            {
                inviteDetailsText.text = sName + " has invited you to join their " + mode + " lobby.";
            }
            else
            {
                inviteDetailsText.text = sName + " has invited you to Custom Room: " + rID;
            }
        }

        Debug.Log("🔔 NEW INVITATION RECEIVED from: " + sName);
    }

    // 3. Invite ACCEPT (✔) Karne Ka Button Logic
    public void AcceptInvitation()
    {
        if (activeIncomingInvite == null) return;

        Debug.Log("✅ Invitation ACCEPTED! Joining " + activeIncomingInvite.senderName + "'s squad...");

        // Agar custom room ka invite tha toh room automatically join ho jaye
        if (!string.IsNullOrEmpty(activeIncomingInvite.roomID) && roomSystem != null)
        {
            roomSystem.JoinCustomRoom(activeIncomingInvite.roomID, "", profileSystem.myProfile.characterID, false);
        }
        else
        {
            // Normal lobby matchmaking squad network sync simulation
            Debug.Log("Successfully synchronized team lobby grids with team leader.");
        }

        CloseInvitationPopup();
    }

    // 4. Invite REJECT (❌) Karne Ka Button Logic
    public void RejectInvitation()
    {
        if (activeIncomingInvite == null) return;

        Debug.Log("❌ Invitation REJECTED by player.");
        // Server ko notify karne ka code yahan aayega (e.g., "Player is busy")
        
        CloseInvitationPopup();
    }

    void CloseInvitationPopup()
    {
        activeIncomingInvite = null;
        if (inviteNotificationPopup != null)
        {
            inviteNotificationPopup.SetActive(false);
        }
    }
}
