using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum VoiceChannel { Mute, Team, All }

[System.Serializable]
public class VoicePeerStatus
{
    public string playerUID;
    public string playerName;
    public bool isSpeaking;
    public bool isMutedByMe;
    public float micVolume; // 0.0f se 1.0f
}

public class VoiceChatManager : MonoBehaviour
{
    [Header("Core Reference")]
    public UserProfileManager profileSystem;

    [Header("Voice Chat Settings")]
    public VoiceChannel currentMicChannel = VoiceChannel.Mute;
    public VoiceChannel currentSpeakerChannel = VoiceChannel.Mute;
    
    [Header("Network Status Simulation")]
    public bool isVoiceServerConnected = false;
    public int voicePingMs = 24; // Low ping means crystal clear high-end audio
    
    [Header("Squad Voice Registry")]
    public List<VoicePeerStatus> squadVoiceList = new List<VoicePeerStatus>();

    void Start()
    {
        // Match ya lobby enter karte hi voice engine connect hoga
        ConnectToVoiceServer();
    }

    // 1. BGMI Luxury Voice Server Connection Simulation
    public void ConnectToVoiceServer()
    {
        Debug.Log("Connecting to Realtime Cross-Platform Voice Server...");
        isVoiceServerConnected = true;
        voicePingMs = Random.Range(15, 45);
        Debug.Log("🎙️ VOICE SERVER CONNECTED! Protocol: High-Fidelity UDP | Ping: " + voicePingMs + "ms");
        
        SimulateSquadVoiceRegistry();
    }

    // 2. Mic State Toggle (Mute -> Team -> All)
    public void ToggleMicChannel(int channelIndex)
    {
        // 0 = Mute, 1 = Team, 2 = All
        currentMicChannel = (VoiceChannel)channelIndex;
        Debug.Log("🎤 MIC STATUS CHANGED TO: " + currentMicChannel.ToString());

        if (currentMicChannel != VoiceChannel.Mute)
        {
            Debug.Log("Your microphone is now transmitting live local audio stream.");
        }
    }

    // 3. Speaker State Toggle (Mute -> Team -> All)
    public void ToggleSpeakerChannel(int channelIndex)
    {
        currentSpeakerChannel = (VoiceChannel)channelIndex;
        Debug.Log("🔊 SPEAKER STATUS CHANGED TO: " + currentSpeakerChannel.ToString());
    }

    // 4. Teammate ko individual mute karne ka option
    public void ToggleMutePlayer(string targetUID)
    {
        foreach (var peer in squadVoiceList)
        {
            if (peer.playerUID == targetUID)
            {
                peer.isMutedByMe = !peer.isMutedByMe;
                Debug.Log("Voice status for " + peer.playerName + " toggled. Muted: " + peer.isMutedByMe);
                return;
            }
        }
    }

    // Fake squad members register karna simulation ke liye
    void SimulateSquadVoiceRegistry()
    {
        squadVoiceList.Clear();
        squadVoiceList.Add(new VoicePeerStatus { playerUID = "UID982736", playerName = "AWM_King_YT", isSpeaking = false, isMutedByMe = false, micVolume = 0.8f });
        squadVoiceList.Add(new VoicePeerStatus { playerUID = "UID119203", playerName = "Jonathan_True", isSpeaking = false, isMutedByMe = false, micVolume = 0.9f });
        
        // Loop simulation for dynamic speaker icons blinking
        StartCoroutine(SimulateTeammatesSpeaking());
    }

    IEnumerator SimulateTeammatesSpeaking()
    {
        while (isVoiceServerConnected)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            
            if (squadVoiceList.Count > 0 && currentSpeakerChannel == VoiceChannel.Team)
            {
                int randomTeammate = Random.Range(0, squadVoiceList.Count);
                if (!squadVoiceList[randomTeammate].isMutedByMe)
                {
                    squadVoiceList[randomTeammate].isSpeaking = true;
                    Debug.Log("🗣️ [TEAM VOICE]: " + squadVoiceList[randomTeammate].playerName + " is talking via mic...");
                    
                    yield return new WaitForSeconds(3f); // 3 seconds tak bolega
                    squadVoiceList[randomTeammate].isSpeaking = false;
                }
            }
        }
    }
}
