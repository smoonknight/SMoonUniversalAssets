using System;
using UnityEngine;

[System.Serializable]
public class ExperienceStat
{
    /// <summary>
    /// Level with startLevel
    /// </summary>
    public int Level => GetLevel(exp);
    /// <summary>
    /// Level without startLevel
    /// </summary>
    public int NormalizedLevel => GetLevel(exp, 0);
    public int exp;
    public int startLevel = 1;
    public int maximumLevel = 4;
    public float factor = 1000;

    public int GetLevel(int exp) => GetLevel(exp, startLevel);
    public int GetLevel(int exp, int startLevel) => Math.Min(maximumLevel, (int)Math.Floor(startLevel + (Math.Sqrt(1 + 8 * exp / factor) / 2)));

    public int ExpRequireOnLevel(int level) => Mathf.FloorToInt((4 * (level + 1) * (level + 1) - 1) * factor / 8);

    public bool IsNextExpRequire(out int expRequire)
    {
        int level = Level;
        int netralLevel = level - startLevel;
        expRequire = ExpRequireOnLevel(netralLevel);
        return level < maximumLevel;
    }

    public bool IsNextExpRequireAndRemainingExp(out int expRequire, out int remainingExp)
    {
        bool isNextExpRequire = IsNextExpRequire(out expRequire);
        remainingExp = expRequire - exp;
        return isNextExpRequire;
    }

    public void AddExperience(int amount)
    {
        if (!IsNextExpRequire(out int expRequire))
        {
            exp = expRequire;
        }
        else
        {
            exp += amount;
        }
    }

    public void AddExperience(int amount, out bool isLevelUp, out int currentLevel, out int levelUpCount)
    {
        int previousLevel = Level;
        AddExperience(amount);
        currentLevel = Level;

        isLevelUp = currentLevel > previousLevel;
        levelUpCount = currentLevel - previousLevel;
    }

    public float AddExperienceAndGetRemainingExpPercentage(int amount, out bool isLevelUp, out int currentLevel, out int levelUpCount)
    {
        AddExperience(amount, out isLevelUp, out currentLevel, out levelUpCount);

        int normalizedLevel = NormalizedLevel;
        int expRequire = ExpRequireOnLevel(normalizedLevel);
        int prevExpRequire = ExpRequireOnLevel(normalizedLevel - 1);
        return (float)(exp - prevExpRequire) / (expRequire - prevExpRequire);
    }
}