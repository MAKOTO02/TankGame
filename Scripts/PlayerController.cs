using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MotionController
{
    public Camera cam;

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Cannon = transform.Find("Cannon").gameObject;
        if (Anima ==null || Rb == null || Cannon == null)
        {
            Debug.Log("FAILED");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        Receive();
        base.Update();
    }

    public void Receive()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }
   
    protected override void TurnCannon()
    {
        // rb.transform.up‚Í–C‘ä‚ÌŒã•û‚ðŽæ“¾.
        torque = Vector3.Cross(cam.transform.forward, Cannon.GetComponent<Rigidbody>().transform.up);
        Cannon.GetComponent<Rigidbody>().AddTorque(torque * 2);
    }
}
