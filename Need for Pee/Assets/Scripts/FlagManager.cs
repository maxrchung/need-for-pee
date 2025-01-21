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
    IsStupid,
    SecondDoorFound,
    NeedCode,
    CanPickUpKey,
    HasKey,
    GirlKeyQuest,
    HasShrek2,
    DiscoveredKeyWrong,
    HasKeyGirl,
    HadKeyGirl,
    HasKeyBathroomGuy,
    HadKeyBathroomGuy,
    HasKeyRound1Guy,
    HadKeyRound1Guy,
    SecondDoorUnlocked,
    PissGuyNumberFound,
    RequestCatCommand,
    CatRequestedCake,
    CakeObtained,
    CatSoldShares,
    BusinessDenied,
    PeeShy,
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

    public static void ClearAll()
    {
        Flags.SetAll(false);
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