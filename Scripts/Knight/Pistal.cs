using UnityEngine;
using Bronya;
public class Pistal : Gun
{
    protected override void Fire()
    {
        base.Fire();
        SoundManager.Instance.PlaySound("Pistal");
    }
}