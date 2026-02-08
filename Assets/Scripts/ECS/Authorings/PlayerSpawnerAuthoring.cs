using Unity.Entities;
using UnityEngine;

public struct PlayerSpawnData : IComponentData
{
    public Entity SpawnEntity;
}

public struct Respawn : IComponentData, IEnableableComponent { }

public class PlayerSpawnerAuthoring : MonoBehaviour
{
    public GameObject SpawnPrefab;

    public class Baker : Baker<PlayerSpawnerAuthoring>
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerSpawnData
            {
                SpawnEntity = GetEntity(authoring.SpawnPrefab, TransformUsageFlags.Dynamic)
            });
            AddComponent<Respawn>(entity);
            SetComponentEnabled<Respawn>(entity, true);
        }
    }
}
