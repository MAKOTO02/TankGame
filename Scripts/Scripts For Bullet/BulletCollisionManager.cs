using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 衝突時の処理を行います。
/// </summary>
[RequireComponent (typeof(Bullet))]
[RequireComponent(typeof(SphereCollider))]
public class BulletCollisionManager : MonoBehaviour
{
    //----- PRIVATE VARIABLES -----//
    [SerializeField] private int durationTimes = 1; // 弾の反射回数の上限.
    private int collisionCount;
    private Bullet bullet;
    private GameObject gameObjectToDisable;

    // Start is called before the first frame update
    void Start()
    {
        collisionCount = 0;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        gameObject.SetActive(true);

        // 弾の特性をここで付与しておきます。
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

    void OnSceneUnloaded(Scene scene)
    {
        ResetCount();
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
        SoundManager.Play("reflect");
    }
    void CollisionCount()
    {
        ++collisionCount;
    }
    //----- PUBLIC METHODS -----//
    public void ResetCount()
    {
        collisionCount = 0;
    }
    public GameObject GetObjectToDisable()
    {
        return gameObjectToDisable;
    }
}