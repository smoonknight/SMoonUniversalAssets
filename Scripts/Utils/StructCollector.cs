using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct PointData
{
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class PointDataCollector
{
    public Transform pointParent;
    public List<PointData> pointDatas { get; private set; } = new();

    public void UpdatePointData(List<PointData> pointDatas) => this.pointDatas = pointDatas;

    public void UpdatePointData()
    {
        var randomWalkPoints = pointParent.GetComponentsInChildren<Transform>();
        pointDatas.Clear();
        foreach (var randomWalkPoint in randomWalkPoints)
        {
            if (randomWalkPoint != pointParent.transform)
            {
                pointDatas.Add(new PointData() { position = randomWalkPoint.position, rotation = randomWalkPoint.rotation });
            }
        }
    }

    public PointData FindNearestPoint(Vector3 targetPoint)
    {
        PointData nearestPoint = pointDatas[0];
        float shortestDistance = Vector3.Distance(targetPoint, nearestPoint.position);

        foreach (PointData point in pointDatas)
        {
            float distance = Vector3.Distance(targetPoint, point.position);
            if (distance < shortestDistance)
            {
                nearestPoint = point;
                shortestDistance = distance;
            }
        }

        return nearestPoint;
    }

    public bool TryGetSampleSources(out List<PointData> pointDatas, int totalPath = 5)
    {
        pointDatas = RandomHelper.RandomList(this.pointDatas).Take(totalPath).ToList();
        if (pointDatas.Count == 0)
        {
            return false;
        }
        return true;
    }

    public PointData GetSampleSource() => RandomHelper.GetRandomElement(pointDatas);
    public bool IsPointDataAvailable() => pointDatas.Count != 0;
}