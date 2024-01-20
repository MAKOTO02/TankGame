using UnityEngine;
/// <summary>
/// 衝突時の処理を行います。
/// </summary>
[RequireComponent (typeof(Bullet))]
[RequireComponent(typeof(SphereCollider))]

public class BulletCollisionManager : MonoBehaviour
{
    // BulletCollision は弾の Active と非 Active 状態を管理し、適切なときにサウンドやエフェクトを発生させます。.
    // Bullet のゲームオブジェクトのプレハブを作り、それにアタッチしてください.
    // BulletオブジェクトのコライダーにPhiscMaterialをつけられるので、
    // そこで、摩擦0,反発係数1、FrictionConbineをminimum,BounceConbineをmaximumにセットして下さい.

    [SerializeField] private int durationTimes = 1;
    private int collisionCount = 0;
    private Bullet bullet;
    public BulletController buletController;
    private GameObject gameObjectToDisable;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);

        // 弾の特性をここで付与しておきます。
        // このコードはより適切な場所が見つかったらそこに移します。
        PhysicMaterial ForBullet = new();
        {
            ForBullet.staticFriction = 0.0f;
            ForBullet.dynamicFriction = 0.0f;
            ForBullet.bounciness = 1.0f;
            ForBullet.frictionCombine = PhysicMaterialCombine.Minimum;
            ForBullet.bounceCombine = PhysicMaterialCombine.Maximum;
        }
        if (gameObject.GetComponent<Collider>().material == null) gameObject.GetComponent<Collider>().material = ForBullet;
        bullet = GetComponent<Bullet>();
    }

    void OnCollisionEnter(Collision collision)
    {
        CollisionCountCheck();
        if (IsPlayerOrEnemyCollision(collision))
        {
            bullet.SetShouldExplode(true);
            gameObjectToDisable = collision.gameObject;
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("ゲームオーバー");
            }
            ResetCount();
            Debug.Log("Player,Enemyもしくは破壊可能な障害物に当たりました");
        }
        else
        {
            PlayReflectionSound();
            CollisionCount();
        }
    }

    bool IsPlayerOrEnemyCollision(Collision collision)
    {
        return collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player");
    }

    void CollisionCountCheck()
    {
        if (collisionCount > durationTimes)
        {
            bullet.SetShouldExplode(true);
            ResetCount();
            Debug.Log("既定の衝突回数を超えました");
        }
    }
    void PlayReflectionSound()
    {
        // Your reflection sound logic goes here
        try
        {
            bullet.soundManager.Play("reflect");
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.Log("サウンドマネージャに反射音を設定して下さい");
        }
    }
    private void ResetCount()
    {
        collisionCount = 0;
    }
    private void CollisionCount()
    {
        ++collisionCount;
    }

    public GameObject GetObjectToDisable()
    {
        return gameObjectToDisable;
    }
}