using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// 簡易的な敵AIです。
// EscapeとAttackの二つのモードを10秒ごとに切り替えます.
public class EnemyController : MotionController
{
    public Rigidbody player;
    public GameObject PlayerCannon;
    private Vector3 PlayerPosition;
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
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // プレイヤーとの相対的な位置を計算.
        PlayerPosition = player.transform.position - thisRigidbody.transform.position;

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

    /// <summary>
    /// Adjusts movement based on the player's distance.
    /// </summary>
    /// <param name="desiredDistance">The desired distance from the player.</param>
    private void AdjustMovementBasedOnDistance(float desiredDistance)
    {
        if (PlayerPosition.magnitude > desiredDistance)
        {
            moveInput = 1;
        }
        else
        {
            moveInput = -1;
        }
    }

    /// <summary>
    /// Initiates an attack by turning towards the player and adjusting movement based on distance.
    /// </summary>
    /// <param name="desiredDistance">The desired distance for the attack.</param>
    private void Attack(float desiredDistance)
    {
        TurnTo(PlayerPosition);
        AdjustMovementBasedOnDistance(desiredDistance);
    }
    /// <summary>
    /// Maintain a vertical orientation towards the player and perform long-range shooting.
    /// </summary>
    /// <param name="desiredDistance">The distance to be maintained.</param>
    private void Escape(float desiredDistance)
    {
        // Face the direction that is perpendicular to the player.
        TurnTo(PlayerPosition, 90.0f);

        // Check conditions for movement.
        float angleCosine = Mathf.Cos(Vector3.Angle(PlayerCannon.GetComponent<Rigidbody>().transform.up, thisRigidbody.transform.forward) * Mathf.PI / 180.0f);

        if (Mathf.Abs(angleCosine) < 0.2f)
        {
            // Move when the player's Cannon captures the aircraft or when the player gets too close.
            moveInput = -1;
        }
        else if (desiredDistance > PlayerPosition.magnitude)
        {
            // Move when the player is farther away.
            moveInput = 1;
        }
        else
        {
            // Otherwise, move randomly.
            moveInput = Random.Range(-1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Adjusts the turnInput to face the vector in the specified direction. 
    /// Apply the correction of the angle in deltaAngle (in degrees, 360-degree system).
    /// </summary>
    /// <param name="direction">The direction vector to face.</param>
    /// <param name="deltaAngle">Additional angle correction (in degrees).</param>
    void TurnTo(Vector3 direction, float deltaAngle = 0.0f)
    {
        var signedAngle = (Vector3.SignedAngle(thisRigidbody.transform.forward, direction, Vector3.up) + deltaAngle) * Mathf.PI / 180.0f;
        turnInput = Mathf.Sin(signedAngle);
    }
}