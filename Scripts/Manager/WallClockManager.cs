using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallClockManager : Singleton<WallClockManager>
{
    private WallClockController[] wallClockControllers;

    protected override void OnAwake()
    {
        base.OnAwake();

        InitializeWallClocks();
    }

    private void InitializeWallClocks()
    {
        wallClockControllers = FindObjectsByType<WallClockController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    public void UpdateClocks(int hours, int minutes)
    {
        foreach (var wallClockController in wallClockControllers)
        {
            wallClockController.UpdateClock(hours, minutes);
        }
    }
}