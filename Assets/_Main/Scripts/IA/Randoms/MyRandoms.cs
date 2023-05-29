using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRandoms
{
    public static float Range(float min, float max)
    {
        return (Random.value * (max - min)) + min;
    }
    public static T Roulette<T>(Dictionary<T, float> items)
    {
        float total = 0;
        foreach (var item in items)
        {
            total += item.Value;
        }
        var random = Range(0, total);

        foreach (var item in items)
        {
            if (random < item.Value)
            {
                return item.Key;
            }
            else
            {
                random -= item.Value;
            }
        }
        return default(T);
    }
    public static void Shuffle<T>(T[] items, System.Action<T, T> onSwap = null)
    {
        //Curr=0
        //ranmdom=7

        //aux=7
        //random=curr
        //curr=aux
        for (int i = 0; i < items.Length; i++)
        {
            int random = Random.Range(0, items.Length);
            if (onSwap != null)
                onSwap(items[i], items[random]);
            (items[random], items[i]) = (items[i], items[random]);
        }
    }
    public static void Shuffle<T>(List<T> items, System.Action<T, T> onSwap)
    {
        for (int i = 0; i < items.Count; i++)
        {
            int random = Random.Range(0, items.Count);
            if (onSwap != null)
                onSwap(items[i], items[random]);
            (items[random], items[i]) = (items[i], items[random]);
        }
    }
}
