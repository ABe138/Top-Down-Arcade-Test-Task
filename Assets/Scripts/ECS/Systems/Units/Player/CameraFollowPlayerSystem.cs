using Unity.Cinemachine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class CameraFollowPlayerSystem : SystemBase
{
    private Transform _target;

    protected override void OnStartRunning()
    {
        _target = Object.FindFirstObjectByType<CameraPlayerTarget>().transform;    
    }

    protected override void OnUpdate()
    {
        foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
        {
            _target.position = transform.ValueRO.Position;
        }
    }
}