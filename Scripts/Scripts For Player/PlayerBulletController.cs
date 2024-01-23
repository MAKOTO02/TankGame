using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : BulletController
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RecycleFire();
        }
    }
}
