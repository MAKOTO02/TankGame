using UnityEngine;

public class EnemyCannonController : BaseCannonController
{
    [SerializeField] protected private Rigidbody playerRigidbody;
    protected Vector3 PlayerPosition;
    protected Vector3 PlayerVelocity;

    [SerializeField] private BulletController bulletController;
    private float BulletSpeed => bulletController?.bulletSpeed ?? 0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Debug.Log($"Bullet Speed: {BulletSpeed}");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        CalculateTargetDirection();
        TurnCannon();
    }

    void CalculateTargetDirection()
    {
        PlayerPosition = playerRigidbody.transform.position - GetComponent<Rigidbody>().transform.position;
        PlayerVelocity = playerRigidbody.velocity;
        if (playerRigidbody == null)
        {
            Debug.Log("Playerのオブジェクトにリジッドボディを追加し、このスクリプトに渡してください");
            return;
        }

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

