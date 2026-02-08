using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct MoveDirection : IComponentData
{
    public float2 Value;
}

public struct MoveData : IComponentData
{
    public float MoveSpeed;
    public float RotationSpeed;
}

public struct MaxHitPoints : IComponentData
{
    public int Value;
}

public struct CurrentHitPoints : IComponentData
{
    public int Value;
}

public struct IncomingDamage : IBufferElementData
{
    public int Value;
}

public struct IsAlive : IComponentData, IEnableableComponent { }


public class UnitAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public float RotationSpeed;
    public int HitPoints;

    private class Baker : Baker<UnitAuthoring>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveDirection { Value = float2.zero });
            AddComponent(entity, new MoveData { MoveSpeed = authoring.MoveSpeed, RotationSpeed = authoring.RotationSpeed });
            AddComponent(entity, new MaxHitPoints { Value = authoring.HitPoints });
            AddComponent(entity, new CurrentHitPoints { Value = authoring.HitPoints });
            AddBuffer<IncomingDamage>(entity);
            AddComponent<IsAlive>(entity);
            SetComponentEnabled<IsAlive>(entity, true);
        }
    }
}
