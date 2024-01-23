using UnityEngine;

public class PlayerBulletController : BulletController
{
    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            RecycleFire();
        }
    }
}
