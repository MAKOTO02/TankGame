using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// 簡易的な敵AIです。
// EscapeとAttackの二つのモードを10秒ごとに切り替えます.
public class EnemyController : MotionController
{
    public Rigidbody player;
    private float l, r;
    private Vector3 Pos;
    private Vector3 prePos;
    private int mode = -1;

    private IEnumerator ModeChange()
    {
        while (true)
        {
            if (Random.value > 0.5f) mode *= -1;
            yield return new WaitForSeconds(10.0f);
        }
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(ModeChange());
        prePos = player.transform.position - Rb.transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // プレイヤーとの相対的な位置を計算.
        Pos = player.transform.position - Rb.transform.position;

        if (mode == 1)
        {
            Attack(50.0f);
        }
        else if (mode == -1)
        {
            Escape(100.0f);
        }
        else
        {
            Debug.Log(mode);
        }
    }

    private void KeepDistance(float distance)
    {
        if (Pos.magnitude > distance)
        {
            moveInput = 1;
        }
        else
        {
            moveInput = -1;
        }
    }
    private void TargetPlayer()
    {
        var signedAngle = Vector3.SignedAngle(Rb.transform.forward, Pos, Vector3.up) * Mathf.PI / 180.0f;
        turnInput = Mathf.Sin(signedAngle);
    }
    private void Attack(float distance)
    {

        TargetPlayer();
        KeepDistance(distance);
    }
    private void Escape(float distance)
    {
        var signedAngle = (Vector3.SignedAngle(Rb.transform.forward, Pos, Vector3.up) + 90.0f) * Mathf.PI / 180.0f;
        turnInput = Mathf.Sin(signedAngle);

        if (Mathf.Abs(Mathf.Cos(Vector3.Angle(Cannon.GetComponent<Rigidbody>().transform.up, Rb.transform.forward) * Mathf.PI / 180.0f)) < 0.2f)
        {
            moveInput = -1;
        }
        else if (distance > Pos.magnitude)
        {
            moveInput = 1;
        }
        else
        {
            moveInput = 0;
        }
    }
    protected override void TurnCannon()
    {
        float distance = Pos.magnitude;
        if (prePos.magnitude > distance)
        {
            l = 100; r = 80;
        }
        else
        {
            l = 40; r = 0;
        }
        torque = Vector3.Cross((Pos + player.velocity * distance / (10.0f + Random.Range(r, l))).normalized, Cannon.GetComponent<Rigidbody>().transform.up);
        Cannon.GetComponent<Rigidbody>().AddTorque(torque);
        prePos = Pos;
    }
}

