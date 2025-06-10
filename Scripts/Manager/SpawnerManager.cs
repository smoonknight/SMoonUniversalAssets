using System;
using SMoonUniversalAsset;
using UnityEngine;

public class SpawnerManager<T, G> : Singleton<SpawnerManager<T, G>> where T : SingleSpawnerBase<G> where G : Component
{
    [SerializeField]
    protected T spawner;

    public G GetSpawned(Vector3? position = null, Quaternion? rotation = null, Func<Vector3> onSetDeactiveOnDurationUpdate = null) => spawner.GetSpawned(position, rotation, onSetDeactiveOnDurationUpdate);

    protected override void OnAwake()
    {
        base.OnAwake();
        spawner.Initialize();
    }

    public int GetActiveSpawn() => spawner.GetActiveSpawn();
}

public class SpawnerManagerWithDDOL<T, G> : SingletonWithDontDestroyOnLoad<SpawnerManagerWithDDOL<T, G>> where T : SingleSpawnerBase<G> where G : Component
{
    [SerializeField]
    protected T spawner;

    public G GetSpawned(Vector3? position = null, Quaternion? rotation = null, Func<Vector3> onSetDeactiveOnDurationUpdate = null) => spawner.GetSpawned(position, rotation, onSetDeactiveOnDurationUpdate);

    protected override void OnAwake()
    {
        base.OnAwake();
        spawner.Initialize();
    }

    public int GetActiveSpawn() => spawner.GetActiveSpawn();
}

public class MultiSpawnerManager<T, G, C> : Singleton<MultiSpawnerManager<T, G, C>> where T : MultiSpawnerBase<G, C> where G : Component where C : Enum
{
    [SerializeField]
    protected T spawner;

    public G GetSpawned(C type, Vector3? position = null, Quaternion? rotation = null, Func<Vector3> onSetDeactiveOnDurationUpdate = null) => spawner.GetSpawned(type, position, rotation, onSetDeactiveOnDurationUpdate);

    protected override void OnAwake()
    {
        base.OnAwake();
        spawner.Initialize();
    }

    public int GetActiveSpawn() => spawner.GetActiveSpawn();
}

public class MultiSpawnerManagerWithDDOL<T, G, C> : SingletonWithDontDestroyOnLoad<MultiSpawnerManagerWithDDOL<T, G, C>> where T : MultiSpawnerBase<G, C> where G : Component where C : Enum
{
    [SerializeField]
    protected T spawner;

    public G GetSpawned(C type, Vector3? position = null, Quaternion? rotation = null, Func<Vector3> onSetDeactiveOnDurationUpdate = null) => spawner.GetSpawned(type, position, rotation, onSetDeactiveOnDurationUpdate);

    protected override void OnAwake()
    {
        base.OnAwake();
        spawner.Initialize();
    }

    public int GetActiveSpawn() => spawner.GetActiveSpawn();
}