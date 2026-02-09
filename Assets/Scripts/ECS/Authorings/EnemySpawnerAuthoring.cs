using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct EnemySpawnData : IComponentData 
{
    public Entity SpawnEntity;
    public float SpawnCooldown;
    public float SpawnDistance;
    public float3 SpawnOffset;
    public float TimeSinceLastSpawn;
}

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject SpawnPrefab;
    public float SpawnDistance = 10f;
    public float SpawnOffset = 5f;
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
                SpawnDistance = authoring.SpawnDistance,
                SpawnOffset = new float3(0, 0, authoring.SpawnOffset),
                TimeSinceLastSpawn = 0
            });
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.forward * SpawnOffset, SpawnDistance);
    }
}
