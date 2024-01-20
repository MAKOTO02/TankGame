using UnityEngine;
/// <summary>
/// �Փˎ��̏������s���܂��B
/// </summary>
[RequireComponent (typeof(Bullet))]
[RequireComponent(typeof(SphereCollider))]

public class BulletCollisionManager : MonoBehaviour
{
    // BulletCollision �͒e�� Active �Ɣ� Active ��Ԃ��Ǘ����A�K�؂ȂƂ��ɃT�E���h��G�t�F�N�g�𔭐������܂��B.
    // Bullet �̃Q�[���I�u�W�F�N�g�̃v���n�u�����A����ɃA�^�b�`���Ă�������.
    // Bullet�I�u�W�F�N�g�̃R���C�_�[��PhiscMaterial��������̂ŁA
    // �����ŁA���C0,�����W��1�AFrictionConbine��minimum,BounceConbine��maximum�ɃZ�b�g���ĉ�����.

    [SerializeField] private int durationTimes = 1;
    private int collisionCount = 0;
    private Bullet bullet;
    public BulletController buletController;
    private GameObject gameObjectToDisable;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);

        // �e�̓����������ŕt�^���Ă����܂��B
        // ���̃R�[�h�͂��K�؂ȏꏊ�����������炻���Ɉڂ��܂��B
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
                Debug.Log("�Q�[���I�[�o�[");
            }
            ResetCount();
            Debug.Log("Player,Enemy�������͔j��\�ȏ�Q���ɓ�����܂���");
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
            Debug.Log("����̏Փˉ񐔂𒴂��܂���");
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
            Debug.Log("�T�E���h�}�l�[�W���ɔ��ˉ���ݒ肵�ĉ�����");
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