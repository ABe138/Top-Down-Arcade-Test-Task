using UnityEngine;
using Unity.Entities;

public struct ProjectileTag : IComponentData { }

public struct ProjectileData : IComponentData
{
    public float Damage;
    public float TimeRemaining;
}

public class ProjectileAuthoring : MonoBehaviour
{
    private class Baker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<ProjectileTag>(entity);
            AddComponent<ProjectileData>(entity);
            AddComponent<DestroyEntityFlag>(entity);
            SetComponentEnabled<DestroyEntityFlag>(entity, false);
        }
    }
}
