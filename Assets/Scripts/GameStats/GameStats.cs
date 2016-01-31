using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class GameStats
{

    //STATIC RECORD OF POINTS IN THE GAME
    //Can be called from any script
    public static int blueTeamPoints = 0;
    public static int redTeamPoints = 0;

    //Time Before Respawn after character dies
    public static float TimeBeforeRespawn = 5.0f;

    ///[PunRPC]
    public static void addPointsTeam(int value, string teamColor)
    {
        if (teamColor == "red")
        {
            redTeamPoints = redTeamPoints + value;
            GameObject.Find("Red Points").GetComponentInChildren<Text>().text = "" + redTeamPoints;
        }
        else if (teamColor == "blue")
        {
            blueTeamPoints = blueTeamPoints + value;
            GameObject.Find("Blue Points").GetComponentInChildren<Text>().text = "" + blueTeamPoints;
        }
        Debug.Log("Red team points: " + redTeamPoints);
        Debug.Log("Blue team points: " + blueTeamPoints);
    }
}
