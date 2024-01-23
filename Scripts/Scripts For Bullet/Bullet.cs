using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Bulletクラスは、弾の速度と弾の反射回数を管理します。
/// desiredSpeedは発射の際に初期化して下さい。
/// </summary>

[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    //----- PRIVATE VARIABLES -----//
    private bool ShouldExplode;
    private GameObject explosion;
    private GameObject explosionCopy;

    // Start is called before the first frame update
    void Start()
    {
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
    void InitiateBullet()
    {
        ShouldExplode = false;
        gameObject.SetActive(false);
    }
    void Explode()
    {
        explosionCopy = Instantiate(explosion, transform.position, Quaternion.identity);
        SoundManager.Play("hit");       
        if(explosionCopy != null) Destroy(explosionCopy, explosionCopy.GetComponent<ParticleSystem>().main.duration);
        InitiateBullet();
        GameObject ObjectToDisable = GetComponent<BulletCollisionManager>().GetObjectToDisable();
        if (ObjectToDisable != null && ObjectToDisable.activeSelf) ObjectToDisable.SetActive(false);
    }
    //----- PUBLIC METHODS -----//
    public void SetShouldExplode(bool setting)
    {
        ShouldExplode = setting;
    }
}
