using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Friend
{
    public string friendName;
    public string status; // Online, In-Match, Offline
    public int level;
}

public class FriendManager : MonoBehaviour
{
    public List<Friend> friendList = new List<Friend>();
    public List<string> pendingRequests = new List<string>();

    public void SendFriendRequest(string name)
    {
        Debug.Log("Friend request sent to: " + name);
        // Logic to send request to server
    }

    public void AcceptRequest(string name)
    {
        Friend newFriend = new Friend { friendName = name, status = "Online", level = 1 };
        friendList.Add(newFriend);
        Debug.Log(name + " is now your friend!");
    }
}
