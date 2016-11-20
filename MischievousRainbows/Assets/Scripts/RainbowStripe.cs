using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RainbowStripe {
    public Color Color;
    [Range(0.0f, 1.0f)]
    public float WidthFraction;
    public LineRenderer Rend;
}
