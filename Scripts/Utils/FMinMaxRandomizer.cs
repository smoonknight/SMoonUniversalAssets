using UnityEngine;

[System.Serializable]
public class FMinMaxRandomizer
{
    public FMinMaxRandomizer(float minimum, float maximum)
    {
        this.minimum = minimum;
        this.maximum = maximum;
    }
    public FMinMaxRandomizer() 
    {
        
    }
    public float minimum;
    public float maximum;

    public float GetRandomize() => Random.Range(minimum, maximum);
}