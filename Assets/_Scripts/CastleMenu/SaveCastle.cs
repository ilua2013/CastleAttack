using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveCastle
{
    public static int TypeCastle
    {
        get => PlayerPrefs.GetInt("CastleType", -1);
        set => PlayerPrefs.SetInt("CastleType", value);
    }

    public static int Health
    {
        get => PlayerPrefs.GetInt("CastleHealth", 0);
        set => PlayerPrefs.SetInt("CastleHealth", value);
    }

    public static bool AttackCastle
    {
        get => PlayerPrefs.GetInt("AttackCastle", 0) == 1;
        set => PlayerPrefs.SetInt("AttackCastle", value ? 1 : 0);
    }
}
