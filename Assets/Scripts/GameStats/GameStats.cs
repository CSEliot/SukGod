using UnityEngine;
using System.Collections;

public static class GameStats
{

    //STATIC RECORD OF POINTS IN THE GAME
    //Can be called from any script
    public static int blueTeamPoints = 0;
    public static int redTeamPoints = 0;

    //Time Before Respawn after character dies
    public static float TimeBeforeRespawn = 3.0f;

    public static void addPointsTeam(int value, string teamColor)
    {
        if (teamColor == "red")
            redTeamPoints = redTeamPoints + value;
        else if (teamColor == "blue")
            blueTeamPoints = blueTeamPoints + value;
        Debug.Log("Red team points: " + redTeamPoints);
        Debug.Log("Blue team points: " + blueTeamPoints);
    }
}
