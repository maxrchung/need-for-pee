using System.Collections.Generic;
using UnityEngine;

public enum GameFlag
{
    // PLEASE ONLY ADD TO BOTTOM OR BREAK EVERYTHING OK :(
    NullFlag,
    ClubCardKnown,
    RoundOneClubCard,
    HasKey,
    MikuLeft,
    KnowsCodeGiver,
    HasCode,
}

public static  class FlagManager
{
    private static readonly HashSet<GameFlag> Flags = new();

    public static void Set(GameFlag flag)
    {
        Flags.Add(flag);
    }

    public static void Unset(GameFlag flag)
    {
        Flags.Remove(flag);
    }

    public static bool Check(GameFlag flag)
    {
        return Flags.Contains(flag);
    }
}