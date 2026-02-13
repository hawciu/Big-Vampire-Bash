using UnityEngine;

public interface IPickupEffect
{
    public void MakeReady(PickupBaseController parent);

    public void Activate();

    public void OnPrefabEffectFinished();
}
