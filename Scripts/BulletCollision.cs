using System.Diagnostics;
using UnityEngine;


public class BulletCollision : MonoBehaviour
{
    // BulletCollision は弾の Active と非 Active 状態を管理し、適切なときにサウンドやエフェクトを発生させます。.
    // Bullet のゲームオブジェクトのプレハブを作り、それにアタッチしてください.
    // BulletオブジェクトのコライダーにPhiscMaterialをつけられるので、
    // そこで、摩擦0,反発係数1、FrictionConbineをminimum,BounceConbineをmaximumにセットして下さい.

    [SerializeField] public SoundManager soundManager; //サウンドマネージャー

    private GameObject explosion;
    private GameObject explosionCopy;
    public int durationTimes;    // 弾の跳弾回数の上限を定めます.
    public float speed;
    private int count = 0;  // 現在の衝突回数を格納しておく変数です.

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        count = 0;
        explosion = (GameObject)Resources.Load("Explosion");

        // 弾の特性をここで付与しておきます。
        // このコードはより適切な場所が見つかったらそこに移します。
        PhysicMaterial ForBullet = new PhysicMaterial();
        {
            ForBullet.staticFriction = 0.0f;
            ForBullet.dynamicFriction = 0.0f;
            ForBullet.bounciness = 1.0f;
            ForBullet.frictionCombine = PhysicMaterialCombine.Minimum;
            ForBullet.bounceCombine = PhysicMaterialCombine.Maximum;
        }
        if (gameObject.GetComponent<Collider>().material == null) gameObject.GetComponent<Collider>().material = ForBullet;
    }

    void Update()
    {
        // 毎フレーム速度を取得して監視を行います.
        //　速度が一定の範囲を超えると非アクティブ化
        InvalidSpeedCheck();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsPlayerOrEnemyCollision(collision))
        {
            Explode();
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("ゲームオーバー");
            }
            DisableGameObject(collision.gameObject);
        }
        else if (ShouldExplode())
        {
            Explode();
        }
        else
        {
            PlayReflectionSound();
        }
    }

    bool IsPlayerOrEnemyCollision(Collision collision)
    {
        return collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player");
    }

    void DisableGameObject(GameObject gameObjectToDisable)
    {
        gameObjectToDisable.SetActive(false);
    }

    bool ShouldExplode()
    {
        return count >= durationTimes;
    }
    void PlayReflectionSound()
    {
        // Your reflection sound logic goes here
        count++;
    }

    void InitiateBullet()
    {
        gameObject.SetActive(false);
        count = 0;
    }
    void InvalidSpeedCheck()
    {

        float speed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        float maxSpeed = speed + 10.0f;
        float minSpeed = speed - 10.0f;
        if (speed > maxSpeed || speed < minSpeed)
        {
            // Effect や Sound の再生を行う.
            soundManager.Play("shot");
            InitiateBullet();
        }
    }

    void Explode()
    {
        explosionCopy = Instantiate(explosion, transform.position, Quaternion.identity);
        soundManager.Play("hit");
        Destroy(explosionCopy, explosionCopy.GetComponent<ParticleSystem>().main.duration);
        InitiateBullet();
    }
}