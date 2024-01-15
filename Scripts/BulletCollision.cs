using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BulletCollision : MonoBehaviour
{
    // BulletCollision�͒e��Active�Ɣ�Active��Ԃ��Ǘ����邽�߂̃X�N���v�g�ł�.
    // Bullet�̃Q�[���I�u�W�F�N�g�̃v���n�u�����A����ɃA�^�b�`���Ă�������.
    // Bullet�I�u�W�F�N�g�̃R���C�_�[��PhiscMaterial��������̂ŁA
    // �����ŁA���C0,�����W��1�AFrictionConbine��minimum,BounceConbine��maximum�ɃZ�b�g���ĉ�����.

    [SerializeField] public SoundManager soundManager; //�T�E���h�}�l�[�W���[

    private GameObject explosion;
    private GameObject explosionCopy;
    public int durationTimes = 3;    // �e�̒��e�񐔂̏�����߂܂�.
    public float maxSpeed = 40; // ���x�̋��������������Ȃ����Ƃ��A��Active�ɖ߂��܂�.
    public float minSpeed = 10;
    private int count = 0;  // ���݂̏Փˉ񐔂��i�[���Ă����ϐ��ł�.

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        count = 0;
        explosion = (GameObject)Resources.Load("Explosion");
    }

    void Update()
    {
        // ���t���[�����x���擾���ĊĎ����s���܂�.
        //�@���x�����͈̔͂𒴂���Ɣ�A�N�e�B�u��
        InvalidSpeedCheck();
    }

    void OnCollisionEnter(Collision collision)
    {
        bool collideActor = collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player";
        if (collideActor || count >= durationTimes)
        {
            explosionCopy = Instantiate(explosion, transform.position, Quaternion.identity);
            if(collideActor ) 
            {
                soundManager.Play("hit");
                collision.gameObject.SetActive(false);
            }
            if(collision.gameObject.tag == "Player") 
            {
                Debug.Log("�Q�[���I�[�o�[");
            }
            Destroy(explosionCopy, explosionCopy.GetComponent<ParticleSystem>().main.duration);
            initiateBullet();
        }
        else
        {
            // ���˂̉����Đ�����.
            count++;
        }
    }

    void initiateBullet()
    {
        gameObject.SetActive(false);
        count = 0;
    }
    void InvalidSpeedCheck()
    {
        float speed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        if (speed > maxSpeed || speed < minSpeed)
        {
            // Effect �� Sound �̍Đ����s��.
            soundManager.Play("hot");
            initiateBullet();
        }
    }
}