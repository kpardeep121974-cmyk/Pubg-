using UnityEngine;

public class RankSystem : MonoBehaviour
{
    public int rankPoints = 0;
    public string currentRank = "Bronze I";

    public void AddPoints(int points)
    {
        rankPoints += points;
        UpdateRankName();
        Debug.Log("Points Added: " + points + " | Total Points: " + rankPoints);
    }

    void UpdateRankName()
    {
        if (rankPoints < 1000) currentRank = "Bronze";
        else if (rankPoints < 2000) currentRank = "Silver";
        else if (rankPoints < 3000) currentRank = "Gold";
        else if (rankPoints < 4000) currentRank = "Platinum";
        else if (rankPoints < 5000) currentRank = "Diamond";
        else if (rankPoints < 6000) currentRank = "Crown";
        else if (rankPoints < 7000) currentRank = "Ace";
        else if (rankPoints < 8000) currentRank = "Conqueror";
        else currentRank = "Grandmaster"; // Luxury Rank
    }
}
