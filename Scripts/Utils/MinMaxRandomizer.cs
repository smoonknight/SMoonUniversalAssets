using UnityEngine;

[System.Serializable]
public class MinMaxRandomizer
{
    public MinMaxRandomizer(int minimum, int maximum)
    {
        this.minimum = minimum;
        this.maximum = maximum;
    }
    public MinMaxRandomizer()
    {

    }
    public int minimum;
    public int maximum;

    public int GetRandomize() => Random.Range(minimum, maximum);
}