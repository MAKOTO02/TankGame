using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Action OnFire;
    static protected GameObject bulletPrefab; // Resources����v���n�u�����[�h���邽�߂̉��ϐ�.
    protected GameObject bulletCopy; // ���ۂɔ��˂����e.

    [SerializeField] public GameObject bulletMark;   // �e�𔭎˂���N�_�ƂȂ鏊��(�C��̎q�I�u�W�F�N�g�Ƃ���)�������Ă����I�u�W�F�N�g.�����Ȃǂ̍ۂɈʒu���Q�Ƃ���.
    private static readonly int queueLimit = 20;    // ���������g�������Ȃ��悤�AQueue�̏�������߂Ă����B
    private readonly Queue<GameObject> bulletQue = new(queueLimit); // ��ɏo���e��Queue�ɓ���Ă����ă��T�C�N������B

    //-----PUBLICVARIABLES-----//
    public int limit = 8;    // ��ɑ��݂ł��鎩�@�̒e�̐��������Ɋi�[.
    public float bulletSpeed = 20;  // �e�̏����𐧌䂷��ϐ�.
    public Rigidbody Cannon;    // �C��̌������擾���邽�߂ɒǉ�.

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(LoadBulletPrefabAsync());
        bulletMark.SetActive(false);    // �ڈ�ƂȂ�I�u�W�F�N�g�͎ז��Ȃ̂ŁAactive��false�ɃZ�b�g���Ă���.
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
        bulletCopy = bulletQue.Dequeue();
        bulletQue.Enqueue(bulletCopy);
    }
    protected virtual void GenerateBulletCopy()
    {
        if (!GameManager.IsWaiting() && GameManager.gameIsPlaying)
        {
            // ����͒e�����`�Ȃ̂ŁA�e�̉�]�͍l������identity�Ő���.
            bulletCopy = Instantiate(bulletPrefab, bulletMark.transform.position, Quaternion.identity);
            if (gameObject.CompareTag("Player")) DontDestroyOnLoad(bulletCopy);
            bulletQue.Enqueue(bulletCopy);
        }
    }
    void Fire()
    {
        if (!GameManager.IsWaiting() && GameManager.gameIsPlaying)
        {
            bulletCopy.transform.position = bulletMark.transform.position;
            bulletCopy.SetActive(true);
            bulletCopy.GetComponent<Bullet>().SetShouldExplode(false);
            bulletCopy.GetComponent<BulletSpeedManager>().SetSpeed(bulletSpeed);
            bulletCopy.GetComponent<Rigidbody>().velocity = -Cannon.transform.up * bulletSpeed;    // -cannon.transform.up�͖C��̑O����.
            SoundManager.Play("shot");
            bulletQue.Enqueue(bulletCopy);
        }
    }
    public void RecycleFire()
    {
        // Queue�ɓ����Ă���e�̐��������菬�����Ȃ�A�V����������Queue�ɒǉ�.
        if (bulletQue.Count < limit)
        {
            GenerateBulletCopy();
            Fire();
        }
        else
        {
            // �e������ɒB������AQueue�ɓ����Ă�����̂��Q�Ƃ��ă��T�C�N������.
            // Active��false�Ȃ�e�����T�C�N������.
            for (int i = 0; i < limit; ++i)
            {
                RotateQueue();
                bool CanUseMore = bulletCopy.activeSelf;
                if (!CanUseMore)
                {
                    Fire();
                    break;
                }
            }
        }
    }
}

