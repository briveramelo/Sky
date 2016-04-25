using UnityEngine;
using System.Collections.Generic;

public enum Tip {
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

public static class Tips {

    static readonly Dictionary<Tip, string> JustTheTips = new Dictionary<Tip, string>() {
        {Tip.Streaks, "Go for big hit streaks to maximize your points" },
        {Tip.Combos, "Hit multiple birds with one spear for big point bonuses" },
        {Tip.Crows, "Crows protect their young, viciously" },
        {Tip.Ducks, "Ducks follow their leader, but they’re lost without one" },
        {Tip.Bats, "Don’t move too quickly with a swarm of bats around- they might pop your balloons" },
        {Tip.BirdOfParadise, "Killing a Bird of Paradise brings an extra balloon" },
        {Tip.Tentacles, "Strike The Might Tentacles rapidly to free yourself from his grasp" },
        {Tip.Pelicans, "Move quickly sideways to avoid a Pelican dive bomb" },
        {Tip.Shit, "Wipe your screen to clean off birdshit" }
    };

    public static string GetTip(Tip requestedTip) {
        return JustTheTips[requestedTip];
    }
}
