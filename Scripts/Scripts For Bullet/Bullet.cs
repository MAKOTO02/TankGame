using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Bulletクラスは、弾の速度と弾の反射回数を管理します。
/// desiredSpeedは発射の際に初期化して下さい。
/// </summary>

[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    private bool ShouldExplode;
    private GameObject explosion;
    private GameObject explosionCopy;

    private SoundManager soundManager; //サウンドマネージャー

    // Start is called before the first frame update
    void Start()
    {
        soundManager = SoundManager.Instance;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        ShouldExplode = false;
        explosion = (GameObject)Resources.Load("Explosion");
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldExplode)
        {
            gameObject.SetActive(true);
            Explode();
            InitiateBullet();
        }
    }

    void OnSceneUnloaded(Scene scene)
    {
        InitiateBullet();
    }

    // 外部からdesiredSpeedを設定するメソッド

    public void SetShouldExplode(bool setting)
    {
        ShouldExplode = setting;
    }

    public void Explode()
    {
        explosionCopy = Instantiate(explosion, transform.position, Quaternion.identity);
        soundManager.Play("hit");       
        if(explosionCopy != null) Destroy(explosionCopy, explosionCopy.GetComponent<ParticleSystem>().main.duration);
        InitiateBullet();
        GameObject ObjectToDisable = GetComponent<BulletCollisionManager>().GetObjectToDisable();
        if (ObjectToDisable != null && ObjectToDisable.activeSelf) ObjectToDisable.SetActive(false);
    }
    void InitiateBullet()
    {
        ShouldExplode = false;
        gameObject.SetActive(false);
    }
}
