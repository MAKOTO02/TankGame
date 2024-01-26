using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private GameObject bulletCopy; // ���ۂɔ��˂����e.
    [SerializeField] private GameObject bulletMark;   // �e�𔭎˂���N�_�ƂȂ鏊��(�C��̎q�I�u�W�F�N�g�Ƃ���)�������Ă����I�u�W�F�N�g.�����Ȃǂ̍ۂɈʒu���Q�Ƃ���.
    static private GameObject bulletPrefab; // Resources����v���n�u�����[�h���邽�߂̉��ϐ�.
    private static readonly int queueLimit = 20;    // ���������g�������Ȃ��悤�AQueue�̏�������߂Ă����B
    private readonly Queue<GameObject> bulletPool = new(queueLimit); // ��ɏo���e��Queue�ɓ���Ă����ă��T�C�N������B
    private Vector3 CannonForward;

    //-----PUBLICVARIABLES-----//
    public int limit = 8;    // ��ɑ��݂ł��鎩�@�̒e�̐��������Ɋi�[.
    public float bulletSpeed = 20;  // �e�̏����𐧌䂷��ϐ�.

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(LoadBulletPrefabAsync());
        bulletMark.SetActive(false);    // �ڈ�ƂȂ�I�u�W�F�N�g�͎ז��Ȃ̂ŁAactive��false�ɃZ�b�g���Ă���.
    }

    protected virtual  void Update()
    {
        CannonForward = gameObject.GetComponent<BaseCannonController>().CannonForward;
    }
    private IEnumerator LoadBulletPrefabAsync()
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>("bullet");

        while (!request.isDone)
        {
            yield return null;
        }

        if (request.asset != null)
        {
            bulletPrefab = (GameObject)request.asset;
        }
        else
        {
            Debug.Log("Resources�t�H���_��bullet�̃v���n�u��������܂���");
        }
    }
    void RotateQueue()
    {
        bulletCopy = bulletPool.Dequeue();
        bulletPool.Enqueue(bulletCopy);
    }
    void GenerateBulletCopy()
    {
        if (GameManager.IsWaiting() || !GameManager.gameIsPlaying) return;
        // ����͒e�����`�Ȃ̂ŁA�e�̉�]�͍l������identity�Ő���.
        bulletCopy = Instantiate(bulletPrefab, bulletMark.transform.position, Quaternion.identity);
        bulletPool.Enqueue(bulletCopy);
        if (gameObject.CompareTag("Player")) DontDestroyOnLoad(bulletCopy);
    }
    void Fire()
    {
        if (GameManager.IsWaiting() || !GameManager.gameIsPlaying) return;
        bulletCopy.transform.position = bulletMark.transform.position;  // �ʒu���}�[�N�̈ʒu��
        bulletCopy.GetComponent<Bullet>().SetShouldExplode(false);
        bulletCopy.GetComponent<BulletSpeedManager>().SetSpeed(bulletSpeed);    // �ˏo���x���Z�b�g����.
        bulletCopy.GetComponent<Rigidbody>().velocity = CannonForward * bulletSpeed;
        bulletCopy.GetComponent<BulletCollisionManager>().ResetCount(); // �Փˉ񐔂����Z�b�g����.
        bulletCopy.SetActive(true); // bulletCopy��active�ɂ���.
        SoundManager.Play("fire");
    }
    public void RecycleFire()
    {
        // Queue�ɓ����Ă���e�̐��������菬�����Ȃ�A�V����������Queue�ɒǉ�.
        if (bulletPool.Count < limit)
        {
            GenerateBulletCopy();   //  �o���b�g�̃R�s�[�𐶐���Queue�ɓ����.
            Fire(); //  �e�����������Ďˏo����.
            return;
        }    
        
        // �e������ɒB������AQueue�ɓ����Ă�����̂��Q�Ƃ��ă��T�C�N������.
        // Active��false�Ȃ�e�����T�C�N������.
        for (int i = 0; i < limit; ++i)
        {
            RotateQueue();  // Queue���񂵁A���̒e��bulletCopy�ɓ����.
            bool UsingNow = bulletCopy.activeSelf;  // �e���g�p���Ȃ玟�ɐi�߂�.
            if (UsingNow) continue;
            Fire();
            break;
        }
    }
}