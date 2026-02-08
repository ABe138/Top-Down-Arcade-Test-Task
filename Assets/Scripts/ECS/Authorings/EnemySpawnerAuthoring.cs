using Unity.Entities;
using UnityEngine;

public struct EnemySpawnData : IComponentData 
{
    public Entity SpawnEntity;
    public float SpawnCooldown;
    public float TimeSinceLastSpawn;
}

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject SpawnPrefab;
    public float SpawnCooldown;

    public class Baker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnemySpawnData 
            {
                SpawnEntity = GetEntity(authoring.SpawnPrefab, TransformUsageFlags.Dynamic),
                SpawnCooldown = authoring.SpawnCooldown,
                TimeSinceLastSpawn = 0
            });
        }
    }
}
