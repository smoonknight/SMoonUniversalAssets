using System;
using SMoonUniversalAsset;
using UnityEngine;

namespace SMoonUniversalAsset
{
    public class ParticleSpawnerManager : MultiSpawnerManagerWithDDOL<ParticleSpawner, ParticleSystem, ParticleType>
    {

    }

    [System.Serializable]
    public class ParticleSpawner : MultiSpawnerBase<ParticleSystem, ParticleType>
    {
        public override void OnSpawn(ParticleSystem component, ParticleType type, Func<Vector3> onSetDeactiveOnDurationUpdate)
        {
            SetDeactiveOnDuration(component, component.main.duration + component.main.startLifetime.constantMax, onSetDeactiveOnDurationUpdate);
        }
    }
}

public enum ParticleType
{
    DoubleJump, Explosion, Nakama
}