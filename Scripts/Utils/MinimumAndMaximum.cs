
using UnityEngine;

[System.Serializable]
public class MinimumAndMaximum
{
    public float minimum;
    public float maximum;

    public float GetRandom() => Random.Range(minimum, maximum);
}