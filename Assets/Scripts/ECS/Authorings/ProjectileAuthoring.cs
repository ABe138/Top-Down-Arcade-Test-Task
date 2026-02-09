using UnityEngine;
using Unity.Entities;

public struct ProjectileTag : IComponentData { }

public struct ProjectileData : IComponentData
{
    public float Damage;
    public float TimeRemaining;
}

public struct PiercingData : IComponentData
{
    public int MaxPierceCount;
    public int CurrentHitCount;
}

public struct HitTarget : IBufferElementData
{
    public Entity Value;
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
            AddComponent<PiercingData>(entity);
            AddBuffer<HitTarget>(entity);
            AddComponent<DestroyEntityFlag>(entity);
            SetComponentEnabled<DestroyEntityFlag>(entity, false);
        }
    }
}
