using UnityEngine;
using System.Collections;

public static class GameStats
{

    //STATIC RECORD OF POINTS IN THE GAME
    //Can be called from any script
    public static int blueTeamPoints = 0;
    public static int redTeamPoints = 0;

    public static void addPointsTeam(int value, string teamColor)
    {
        if (teamColor == "red")
            redTeamPoints += value;
        else if (teamColor == "blue")
            blueTeamPoints += value;
    }
}
