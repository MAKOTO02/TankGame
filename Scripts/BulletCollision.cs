using System.Diagnostics;
using UnityEngine;


public class BulletCollision : MonoBehaviour
{
    // BulletCollision �͒e�� Active �Ɣ� Active ��Ԃ��Ǘ����A�K�؂ȂƂ��ɃT�E���h��G�t�F�N�g�𔭐������܂��B.
    // Bullet �̃Q�[���I�u�W�F�N�g�̃v���n�u�����A����ɃA�^�b�`���Ă�������.
    // Bullet�I�u�W�F�N�g�̃R���C�_�[��PhiscMaterial��������̂ŁA
    // �����ŁA���C0,�����W��1�AFrictionConbine��minimum,BounceConbine��maximum�ɃZ�b�g���ĉ�����.

    [SerializeField] public SoundManager soundManager; //�T�E���h�}�l�[�W���[

    private GameObject explosion;
    private GameObject explosionCopy;
    public int durationTimes;    // �e�̒��e�񐔂̏�����߂܂�.
    public float speed;
    private int count = 0;  // ���݂̏Փˉ񐔂��i�[���Ă����ϐ��ł�.

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        count = 0;
        explosion = (GameObject)Resources.Load("Explosion");

        // �e�̓����������ŕt�^���Ă����܂��B
        // ���̃R�[�h�͂��K�؂ȏꏊ�����������炻���Ɉڂ��܂��B
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
        // ���t���[�����x���擾���ĊĎ����s���܂�.
        //�@���x�����͈̔͂𒴂���Ɣ�A�N�e�B�u��
        InvalidSpeedCheck();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsPlayerOrEnemyCollision(collision))
        {
            Explode();
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("�Q�[���I�[�o�[");
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
            // Effect �� Sound �̍Đ����s��.
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