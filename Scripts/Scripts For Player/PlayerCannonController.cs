using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannonController : BaseCannonController
{
    [SerializeField] private Camera MainCamera;

    // Update is called once per frame
    protected override void Update()
    {
        AimDirection = MainCamera.transform.forward;
        base.Update();
        TurnCannon();
    }
}
