using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static void FlipTransform(Transform t)
    {
        Vector3 scale = t.localScale;
        scale.x *= -1;
        t.localScale = scale;
    }
}
