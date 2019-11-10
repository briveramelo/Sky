using System.Collections.Generic;
using UnityEngine;

public enum Tip
{
    Streaks,
    Combos,
    Crows,
    Ducks,
    Bats,
    BirdOfParadise,
    Tentacles,
    Pelicans,
    Shit
}

public static class Tips
{
    private static readonly Dictionary<Tip, string> _justTheTips = new Dictionary<Tip, string>()
    {
        {Tip.Streaks, "Go for big hit streaks to maximize your points"},
        {Tip.Combos, "Hit multiple birds with one spear for big point bonuses"},
        {Tip.Crows, "Crows protect their young, viciously"},
        {Tip.Ducks, "Ducks follow their leader, but they’re lost without one"},
        {Tip.Bats, "Don’t move too quickly with a swarm of bats around. They might pop your balloons"},
        {Tip.BirdOfParadise, "Kill a Bird of Paradise to gain an extra balloon"},
        {Tip.Tentacles, "Strike The Mighty Tentacles rapidly to free yourself from his grasp"},
        {Tip.Pelicans, "Move quickly sideways to avoid a Pelican dive bomb"},
        {Tip.Shit, "Wipe your screen to clean off birdshit"}
    };

    public static string GetTip(Tip requestedTip)
    {
        string tip = "";
        if (!_justTheTips.TryGetValue(requestedTip, out tip))
        {
            Debug.LogError("No tip message found for " + requestedTip);
        }

        return tip;
    }
}