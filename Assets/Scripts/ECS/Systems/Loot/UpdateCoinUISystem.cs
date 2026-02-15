using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial struct UpdateCoinUISystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (coinsCounter, updateFlag) in SystemAPI.Query<CoinsCollectedCounter, EnabledRefRW<UpdateCoinsUIFlag>>())
        {
            PlayerHUDManager.Instance.UpdateCoinsCollectedText(coinsCounter.Value);
            updateFlag.ValueRW = false;
        }
    }
}
