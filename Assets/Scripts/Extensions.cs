using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Extensions
{
    
    public static T PickRandom<T>(this IList<T> source)
    {
        return source[Random.Range(0, source.Count)];
    }
}
