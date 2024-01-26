using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// �Փˎ��̏������s���܂��B
/// </summary>
[RequireComponent (typeof(Bullet))]
[RequireComponent(typeof(SphereCollider))]
public class BulletCollisionManager : MonoBehaviour
{
    //----- PRIVATE VARIABLES -----//
    [SerializeField] private int durationTimes = 1; // �e�̔��ˉ񐔂̏��.
    private int collisionCount;
    private Bullet bullet;
    private GameObject gameObjectToDisable;

    // Start is called before the first frame update
    void Start()
    {
        collisionCount = 0;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        gameObject.SetActive(true);

        // �e�̓����������ŕt�^���Ă����܂��B
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
        if (IsDistractiveCollision(collision))
        {
            bullet.SetShouldExplode(true);
            gameObjectToDisable = collision.gameObject;
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("�Q�[���I�[�o�[");
                GameManager.Instance.GameOver();
            }
            ResetCount();
            Debug.Log("�j��\�ȏ�Q���ɓ�����܂���");
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

    bool IsDistractiveCollision(Collision collision)
    {
        return collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Distractive");
    }

    void CollisionCountCheck()
    {
        if (collisionCount > durationTimes)
        {
            bullet.SetShouldExplode(true);
            ResetCount();
            Debug.Log("����̏Փˉ񐔂𒴂��܂���");
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
        // �e�͔̂��˂̍ۂɊO�̃N���X����Ăяo���̂ŁApublic�ɂ��܂����B
        collisionCount = 0;
    }
    public GameObject GetObjectToDisable()
    {
        return gameObjectToDisable;
    }
}