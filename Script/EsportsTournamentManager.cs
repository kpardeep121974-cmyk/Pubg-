using System.Collections.Generic;
using UnityEngine;

public class EsportsTournamentManager : MonoBehaviour
{
    [System.Serializable]
    public class TournamentInfo
    {
        public string tournamentID;
        public string tournamentName; // जैसे: "India National Championship 2026"
        public int currentRegisteredTeams = 0;
        public int maxTeamsLimit = 10000;
        public bool isRegistrationOpen = false;
    }

    [System.Serializable]
    public class TeamRegistration
    {
        public string teamName;
        public string leaderUID;
        public string member2UID;
        public string member3UID;
        public string member4UID;
        public int totalTournamentPoints = 0; // मैच जीतने और किल्स के पॉइंट्स
    }

    [Header("Active Tournament Status")]
    public TournamentInfo activeTournament;
    
    [Header("Your Registered Squad")]
    public TeamRegistration mySquad;

    [Header("Esports Leaderboard (Top Teams Simulation)")]
    public List<TeamRegistration> esportsLeaderboard = new List<TeamRegistration>();

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<EsportsTournamentManager>(this);
        }
    }

    void Start()
    {
        // डमी टूर्नामेंट डेटा सेटअप
        activeTournament.tournamentID = "INC_2026";
        activeTournament.tournamentName = "Global Battle Royale Masters 2026";
        activeTournament.isRegistrationOpen = true;
    }

    // लॉबी में 'Esports Register' बटन दबाने पर स्क्वाड को रजिस्टर करने का लॉजिक
    public void RegisterSquadInTournament(string tName, string p1, string p2, string p3, string p4)
    {
        if (!activeTournament.isRegistrationOpen)
        {
            Debug.LogError("[Esports] अभी किसी भी टूर्नामेंट के रजिस्ट्रेशन खुले नहीं हैं!");
            return;
        }

        if (activeTournament.currentRegisteredTeams >= activeTournament.maxTeamsLimit)
        {
            Debug.LogError("[Esports] टूर्नामेंट के स्लॉट्स फुल हो चुके हैं!");
            return;
        }

        mySquad.teamName = tName;
        mySquad.leaderUID = p1;
        mySquad.member2UID = p2;
        mySquad.member3UID = p3;
        mySquad.member4UID = p4;
        mySquad.totalTournamentPoints = 0;

        activeTournament.currentRegisteredTeams++;
        Debug.LogWarning($"🏆 [ESPORTS SUCCESS] आपकी स्क्वाड '{tName}' सफलता पूर्वक ऑफिशल टूर्नामेंट '{activeTournament.tournamentName}' के लिए रजिस्टर हो गई है!");
        
        SaveEsportsData();
    }

    // एस्पोर्ट्स मैच खत्म होने पर पॉइंट्स कैलकुलेट करने का लॉजिक (Placement Points + Kill Points)
    public void UpdateTournamentPoints(int matchRank, int totalKills)
    {
        if (string.IsNullOrEmpty(mySquad.teamName)) return; // अगर प्लेयर किसी टूर्नामेंट टीम में नहीं है तो रिटर्न करें

        int placementPoints = 0;
        if (matchRank == 1) placementPoints = 12;      // चिकन डिनर के 12 पॉइंट्स
        else if (matchRank == 2) placementPoints = 9;  // सेकंड रैंक के 9 पॉइंट्स
        else if (matchRank <= 4) placementPoints = 6;

        int killPoints = totalKills * 1; // हर 1 किल का 1 पॉइंट (जैसे ऑफिशल एस्पोर्ट्स में होता है)
        int matchTotal = placementPoints + killPoints;

        mySquad.totalTournamentPoints += matchTotal;

        Debug.LogWarning($"🎮 [ESPORTS MATCH END] आपकी टीम को मिले: {placementPoints} प्लेसमेंट पॉइंट्स और {killPoints} किल पॉइंट्स। टोटल टीम स्कोर: {mySquad.totalTournamentPoints}");
        
        SaveEsportsData();
    }

    private void SaveEsportsData()
    {
        string json = JsonUtility.ToJson(mySquad);
        PlayerPrefs.SetString("RegisteredEsportsSquad", json);
        PlayerPrefs.Save();
    }
}
