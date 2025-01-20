using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameFlag
{
    // PLEASE ONLY ADD TO BOTTOM OR BREAK EVERYTHING OK :(
    NullFlag,

    BathroomFound,
    NeedClubCard,
    HasClubCard,
    KnowsCodeGiver,
    HasCode,
    DoorUnlocked,
}

public static class FlagManager
{
    private static readonly BitArray Flags = new(Enum.GetNames(typeof(GameFlag)).Length, false);

    public static void Set(GameFlag flag)
    {
        Flags.Set((int)flag, true);
    }

    public static void Unset(GameFlag flag)
    {
        Flags.Set((int)flag, false);
    }

    public static bool Check(GameFlag flag)
    {
        return Flags.Get((int)flag);
    }

    public static bool CheckAll(params GameFlag[] flags)
    {
        return flags.All(Check);
    }
}