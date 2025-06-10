using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace SMoonUniversalAsset
{
    public abstract class SpawnerBase<T, G> where T : Component where G : SpawnProperty<T>
    {
        [SerializeField] protected Transform pool;
        [SerializeField] protected int initialPoolSize = 20;

        [SerializeField] protected List<G> spawnProperties;

        protected HashSet<G> spawnedPropertyPool;

        public virtual void Initialize()
        {
            for (int i = 0; i < spawnProperties.Count; i++)
            {
                G spawnProperty = spawnProperties[i];
                bool reinstanceComponent = spawnProperty.component.gameObject.scene.rootCount == 0;
                T instance = reinstanceComponent ? UnityEngine.Object.Instantiate(spawnProperty.component, pool) : spawnProperty.component;
                instance.gameObject.SetActive(false);
                instance.name = spawnProperty.component.name;

                spawnProperty.instance = instance;
            }

            int poolSize = GetInitialPoolSize();
            spawnedPropertyPool = new HashSet<G>(poolSize);

            for (int i = 0; i < poolSize; i++)
            {
                foreach (var spawnProperty in spawnProperties)
                {
                    G copyOfSpawnProperty = CreateCopy(spawnProperty);
                    AddSpawnPropertyToSpawnedPropertyPool(copyOfSpawnProperty, false, out _);
                }
            }
        }

        protected void AddSpawnPropertyToSpawnedPropertyPool(G spawnedProperty, bool setActiveValue, out T instance)
        {
            instance = UnityEngine.Object.Instantiate(spawnedProperty.component, pool);
            instance.gameObject.SetActive(setActiveValue);
            spawnedProperty.instance = instance;
            spawnedProperty.instance.name = $"{spawnedProperty.component.name} {spawnedPropertyPool.Count}";
            spawnedPropertyPool.Add(spawnedProperty);
        }

        protected async void SetDeactiveOnDuration(T component, float duration, Func<Vector3> onSetDeactiveOnDurationUpdate = null)
        {
            if (onSetDeactiveOnDurationUpdate != null)
            {
                float time = 0;
                while (time < duration)
                {
                    if (component == null || !component.gameObject.activeInHierarchy)
                    {
                        break;
                    }
                    Vector3 position = onSetDeactiveOnDurationUpdate.Invoke();
                    component.transform.position = position;
                    time += Time.deltaTime;
                    await UniTask.Yield();
                }
            }
            else
            {
                await UniTaskExtensions.DelayWithCancel(duration, () => component == null || !component.gameObject.activeInHierarchy);
            }
            if (component != null)
                component.gameObject.SetActive(false);
        }

        public int GetActiveSpawn()
        {
            int count = 0;
            foreach (var spawnedProperty in spawnedPropertyPool)
            {
                if (spawnedProperty.instance.gameObject.activeInHierarchy)
                    count++;
            }
            return count;
        }

        public void HideSpawn()
        {
            foreach (var spawnedProperty in spawnedPropertyPool)
            {
                spawnedProperty.instance.gameObject.SetActive(false);
            }
        }

        protected abstract G CreateCopy(G spawnProperty);

        protected int GetInitialPoolSize() => initialPoolSize * spawnProperties.Count;
    }

    public abstract class SingleSpawnerBase<T> : SpawnerBase<T, SpawnProperty<T>> where T : Component
    {
        protected void AddSpawnPropertyToSpawnedPropertyPool(List<SpawnProperty<T>> spawnProperties, bool setActiveValue, out T newComponent)
        {
            SpawnProperty<T> selectedSpawnProperty = spawnProperties.FirstOrDefault();
            if (selectedSpawnProperty.instance == null)
            {
                throw new IndexOutOfRangeException("not found!");
            }
            selectedSpawnProperty = CreateCopy(selectedSpawnProperty);
            AddSpawnPropertyToSpawnedPropertyPool(selectedSpawnProperty, setActiveValue, out newComponent);
        }

        public T GetSpawned(Vector3? position = null, Quaternion? rotation = null, Func<Vector3> onSetDeactiveOnDurationUpdate = null)
        {
            T component;
            foreach (var spawnedProperty in spawnedPropertyPool)
            {
                if (spawnedProperty.instance.gameObject.activeInHierarchy)
                {
                    continue;
                }

                component = spawnedProperty.instance;
                if (position.HasValue)
                    component.transform.position = position.Value;

                if (rotation.HasValue)
                    component.transform.rotation = rotation.Value;

                component.gameObject.SetActive(true);
                OnSpawn(component, onSetDeactiveOnDurationUpdate);
                return component;
            }

            AddSpawnPropertyToSpawnedPropertyPool(spawnProperties, true, out component);
            OnSpawn(component, onSetDeactiveOnDurationUpdate);

            return component;
        }

        protected override SpawnProperty<T> CreateCopy(SpawnProperty<T> spawnProperty)
        {
            return new()
            {
                component = spawnProperty.component,
                instance = spawnProperty.instance
            };
        }
        public abstract void OnSpawn(T component, Func<Vector3> onSetDeactiveOnDurationUpdate = null);
    }

    public abstract class MultiSpawnerBase<T, G> : SpawnerBase<T, MultiSpawnProperty<T, G>> where T : Component where G : Enum
    {

        protected void AddSpawnPropertyToSpawnedPropertyPool(List<MultiSpawnProperty<T, G>> spawnProperties, G type, bool setActiveValue, out T newComponent)
        {
            MultiSpawnProperty<T, G> selectedSpawnProperty = spawnProperties.FirstOrDefault(spawnProperty => spawnProperty.type.Equals(type));
            if (selectedSpawnProperty.instance == null)
            {
                throw new IndexOutOfRangeException(type + " not found!");
            }
            selectedSpawnProperty = CreateCopy(selectedSpawnProperty);
            AddSpawnPropertyToSpawnedPropertyPool(selectedSpawnProperty, setActiveValue, out newComponent);
        }

        public T GetSpawned(G type, Vector3? position = null, Quaternion? rotation = null, Func<Vector3> onSetDeactiveOnDurationUpdate = null)
        {
            T component;
            foreach (var spawnedProperty in spawnedPropertyPool)
            {
                if (spawnedProperty.instance.gameObject.activeInHierarchy)
                {
                    continue;
                }
                if (!spawnedProperty.type.Equals(type))
                {
                    continue;
                }

                component = spawnedProperty.instance;
                if (position.HasValue)
                    component.transform.position = position.Value;

                if (rotation.HasValue)
                    component.transform.rotation = rotation.Value;

                component.gameObject.SetActive(true);
                OnSpawn(component, type, onSetDeactiveOnDurationUpdate);
                return component;
            }

            AddSpawnPropertyToSpawnedPropertyPool(spawnProperties, type, true, out component);
            OnSpawn(component, type, onSetDeactiveOnDurationUpdate);

            return component;
        }

        protected override MultiSpawnProperty<T, G> CreateCopy(MultiSpawnProperty<T, G> spawnProperty)
        {
            return new()
            {
                component = spawnProperty.component,
                instance = spawnProperty.instance,
                type = spawnProperty.type
            };
        }

        public abstract void OnSpawn(T component, G type, Func<Vector3> onSetDeactiveOnDurationUpdate = null);
    }
}

[System.Serializable]
public class MultiSpawnProperty<T, G> : SpawnProperty<T> where T : Component where G : Enum
{
    public G type;
}

[System.Serializable]
public class SpawnProperty<T> where T : Component
{
    public T component;
    [ReadOnly]
    public T instance;
}