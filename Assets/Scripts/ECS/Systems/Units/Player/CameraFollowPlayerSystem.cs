using Unity.Cinemachine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class CameraFollowPlayerSystem : SystemBase
{
    private GameObject _cameraTarget;

    protected override void OnStartRunning()
    {
        _cameraTarget = new GameObject("CameraTarget");
        var camera = Object.FindFirstObjectByType<CinemachineCamera>();
        camera.Follow = _cameraTarget.transform;
    }

    protected override void OnUpdate()
    {
        foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
        {
            _cameraTarget.transform.position = transform.ValueRO.Position;
        }
    }
}