using UnityEngine;

[RequireComponent(typeof(EnemyBulletController))]
public class EnemyCannonController : BaseCannonController
{
    public Rigidbody targetRigidbody;
    protected Vector3 PlayerPosition;
    protected Vector3 PlayerVelocity;
    private float BulletSpeed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        BulletSpeed = GetComponent<EnemyBulletController>().bulletSpeed;
        Debug.Log($"Bullet Speed: {BulletSpeed}");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (targetRigidbody != null)
        {
            CalculateTargetDirection();
            TurnCannon();
        }
    }

    void CalculateTargetDirection()
    {
        PlayerPosition = targetRigidbody.transform.position - GetComponent<Rigidbody>().transform.position;
        PlayerVelocity = targetRigidbody.velocity;

        float maxRange = 1.0f;
        float minRange = 0.0f;

        if (PlayerPosition.magnitude > 50 || PlayerVelocity.magnitude > 5)
        {
            maxRange = 1.2f;
            minRange = 0.5f;
        }

        float estimatedTime = (PlayerPosition.magnitude / BulletSpeed) * Random.Range(minRange, maxRange);
        AimDirection = PlayerPosition + PlayerVelocity * estimatedTime;
    }
}

