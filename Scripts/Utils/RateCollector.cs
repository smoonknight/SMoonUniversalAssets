using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class RateCollector<T>
{
    public List<Rateproperty> rateProperties;

    float maximumProbabilityRate;

    public void Calculate()
    {
        float totalRate = 0;
        float currentRange = 0;
        rateProperties.ForEach(action => totalRate += action.rate);
        float adjustmentRateValue = 100.0f / totalRate;
        rateProperties.ForEach(action =>
        {
            action.minimumRange = currentRange;
            action.maximumRange = currentRange += adjustmentRateValue * action.rate;
        });

        maximumProbabilityRate = currentRange;
    }

    public T CalculateAndGetRandomData()
    {
        Calculate();
        return GetRandomData();
    }

    public T GetRandomData()
    {
        float randomizeValue = Random.Range(0, maximumProbabilityRate);
        return rateProperties.FirstOrDefault(rateProperty => randomizeValue.IsBetween(rateProperty.minimumRange, rateProperty.maximumRange)).data;
    }

    [System.Serializable]
    public class Rateproperty
    {
        public float rate = 1;
        [ReadOnly]
        public float minimumRange;
        [ReadOnly]
        public float maximumRange;
        public T data;
    }
}