using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileTag : IComponentData { }

public struct ProjectileData : IComponentData
{
    public float Damage;
    public float TimeRemaining;
}

public class ProjectileAuthoring : UnityEngine.MonoBehaviour
{
    private class Baker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<ProjectileTag>(entity);
            AddComponent<ProjectileData>(entity);
        }
    }
}
