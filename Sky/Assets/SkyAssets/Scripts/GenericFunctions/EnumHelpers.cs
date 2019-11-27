using System;
using System.Collections.Generic;
using System.Linq;
using BRM.DebugAdapter;
using BRM.DebugAdapter.Interfaces;

public static class EnumHelpers
{
    private static IDebug _debugger = new UnityDebugger();

    public static List<TEnum> Invert<TEnum>(this TEnum[] birdTypes) where TEnum : Enum
    {
        var birdsToWaitFor = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        foreach (var birdType in birdTypes)
        {
            birdsToWaitFor.Remove(birdType);
        }

        return birdsToWaitFor;
    }

    public static List<TEnum> GetAll<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
    }

    public static TVal SafeGet<TKey, TVal>(this Dictionary<TKey, TVal> dic, TKey key) where TKey : Enum
    {
        if (dic.TryGetValue(key, out var val))
        {
            return val;
        }

        _debugger.LogErrorFormat("No value found in dictionary for keyed enum {0}", key);
        return default;
    }

    public static TEnum GetRandom<TEnum>() where TEnum : Enum
    {
        var all = GetAll<TEnum>();
        var index = UnityEngine.Random.Range(0, all.Count);
        return all[index];
    }

    public static TEnum ToEnum<TEnum>(this string unparsedString) where TEnum : Enum
    {
        return (TEnum) Enum.Parse(typeof(TEnum), unparsedString);
    }
}