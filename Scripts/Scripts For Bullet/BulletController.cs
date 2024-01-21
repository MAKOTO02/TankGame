using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bullet�̏������̍ۂɓn���Ȃ���΂����Ȃ����������ňꊇ�Ǘ����܂��B
/// bullet�N���X�̕ϐ��𕹂��Ċm�F���Ă��������B
/// </summary>
public class BuleltInstantiateInfo
{
    public Bullet bullet;
    public BulletCollisionManager collisionManager;
    public BulletSpeedManager speedManager;
    public SoundManager soundManager;

    // �R���X�g���N�^�ł�
    public BuleltInstantiateInfo(SoundManager soundManager)
    {
        this.soundManager = soundManager;
    }

    public void SetUpBullet(ref Bullet bullet)
    {
        bullet.soundManager = this.soundManager;
    }
}


public class BulletController : MonoBehaviour
{
    // BulletController�̓t�B�[���h�̒e�̐��𐧌����邽�߂̃I�u�W�F�N�g�ŁA��ɂ��̏������s���܂�.

    //-----PRIVATEVARIABLES-----//
    private GameObject bulletPrefab; // Resources����v���n�u�����[�h���邽�߂̉��ϐ�.
    private GameObject bulletCopy; // ���ۂɔ��˂����e.

    [SerializeField] private GameObject bulletMark;   // �e�𔭎˂���N�_�ƂȂ鏊��(�C��̎q�I�u�W�F�N�g�Ƃ���)�������Ă����I�u�W�F�N�g.�����Ȃǂ̍ۂɈʒu���Q�Ƃ���.
    private static readonly int queueLimit = 20;    // ���������g�������Ȃ��悤�AQueue�̏�������߂Ă����B
    private readonly Queue<GameObject> bulletQue = new(queueLimit); // ��ɏo���e��Queue�ɓ���Ă����ă��T�C�N������B

    public SoundManager soundManager; //�T�E���h�}�l�[�W���[

    //-----PUBLICVARIABLES-----//
    public int limit = 8;    // ��ɑ��݂ł��鎩�@�̒e�̐��������Ɋi�[.
    public float bulletSpeed = 20;  // �e�̏����𐧌䂷��ϐ�.
    public Rigidbody Cannon;    // �C��̌������擾���邽�߂ɒǉ�.

    // Start is called before the first frame update
    protected virtual void Start()
    {
        bulletPrefab = (GameObject)Resources.Load("bullet"); //  �v���n�u�̃f�[�^�����[�h.
        bulletMark.SetActive(false);    // �ڈ�ƂȂ�I�u�W�F�N�g�͎ז��Ȃ̂ŁAactive��false�ɃZ�b�g���Ă���.
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // �}�E�X�̍��N���b�N�����m���Ēe�𔭎˂���.
        if (Input.GetMouseButtonDown(0)) RecycleShot();
    }


    void Shot()
    {
        bulletCopy.transform.position = bulletMark.transform.position;
        bulletCopy.SetActive(true);
        bulletCopy.GetComponent<Bullet>().SetShouldExplode(false);
        bulletCopy.GetComponent<BulletSpeedManager>().SetSpeed(bulletSpeed);
        bulletCopy.GetComponent<Rigidbody>().velocity = -Cannon.transform.up * bulletSpeed;    // -cannon.transform.up�͖C��̑O����.
        try
        {
            soundManager.Play("shot");
        }
        catch
        {

        }

        finally
        {
            if (bulletCopy.GetComponent<Bullet>().soundManager == null)
            {
                Debug.Log("�T�E���h�}�l�[�W���[�̎Q�ƂɎ��s���܂���");
                Debug.Log("BulletController��SoundManager��n���Ă�������");
            }
            bulletQue.Enqueue(bulletCopy);
        }


    }
    void RotateQueue()
    {
        bulletCopy = bulletQue.Dequeue();
        bulletQue.Enqueue(bulletCopy);
    }
    void GenerateBulletCopy()
    {
        // ����͒e�����`�Ȃ̂ŁA�e�̉�]�͍l������identity�Ő���.
        bulletCopy = Instantiate(bulletPrefab, bulletMark.transform.position, Quaternion.identity);
        BuleltInstantiateInfo info = new(soundManager);
        try
        {
            Bullet bullet = bulletCopy.GetComponent<Bullet>();
            info.SetUpBullet(ref bullet);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
        finally
        {
            if (bulletCopy.GetComponent<Bullet>().soundManager == null)
            {
                Debug.Log("�T�E���h�}�l�[�W���[�̎Q�ƂɎ��s���܂���");
                Debug.Log("BulletController��SoundManager��n���Ă�������");
            }
            bulletQue.Enqueue(bulletCopy);
        }
    }
    protected void RecycleShot()
    {
        // Queue�ɓ����Ă���e�̐��������菬�����Ȃ�A�V����������Queue�ɒǉ�.
        if (bulletQue.Count < limit)
        {
            GenerateBulletCopy();
            Shot();
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
                    Shot();
                    break;
                }
            }
        }
    }
}
