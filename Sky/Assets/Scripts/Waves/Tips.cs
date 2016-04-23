using UnityEngine;
using System.Collections.Generic;

public enum Tip {
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K
}

public static class Tips {

    static readonly Dictionary<Tip, string> JustTheTips = new Dictionary<Tip, string>() {
        {Tip.A, "Go for big hit streaks to maximize your points" },
        {Tip.B, "Hit multiple birds with one spear for big point bonuses" },
        {Tip.C, "Seagulls and The Mighty Tentacles give big point bonuses for other birds on screen" },
        {Tip.D, "Crows protect their young, viciously" },
        {Tip.E, "Ducks follow their leader, but they’re lost without one" },
        {Tip.F, "Don’t move too quickly with a swarm of bats around- they might pop your balloons" },
        {Tip.G, "Killing a Bird of Paradise brings an extra balloon" },
        {Tip.H, "When you see The Mighty Tentacles lurking, don’t get too close to the water" },
        {Tip.I, "Strike The Might Tentacles rapidly to free yourself from his grasp" },
        {Tip.J, "Move quickly sideways to avoid a Pelican dive bomb" },
        {Tip.K, "Wipe your screen to clean off birdshit" }
    };

    public static string GetTip(Tip requestedTip) {
        return JustTheTips[requestedTip];
    }
}
