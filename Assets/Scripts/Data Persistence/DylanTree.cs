using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DylanTree
{
    public static float EaseOutQuart(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4);
    }
}
