using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class RandomUtils
{
    public static T RandomValue<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}