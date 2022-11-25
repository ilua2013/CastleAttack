using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamesStatistics
{
    public static int KilledEnemy { get; private set; }
    public static int KilledFriend { get; private set; }

    public static void RegisterEnemyKill()
    {
        Debug.Log("Kill enemy");
        KilledEnemy++;
    }

    public static void RegisterFriendKill()
    {
        KilledFriend++;
    }
}
