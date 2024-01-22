using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // BulletController�̓t�B�[���h�̒e�̐��𐧌����邽�߂̃I�u�W�F�N�g�ŁA��ɂ��̏������s���܂�.

    //-----PRIVATEVARIABLES-----//
    static protected GameObject bulletPrefab; // Resources����v���n�u�����[�h���邽�߂̉��ϐ�.
    protected GameObject bulletCopy; // ���ۂɔ��˂����e.
    
    [SerializeField] public GameObject bulletMark;   // �e�𔭎˂���N�_�ƂȂ鏊��(�C��̎q�I�u�W�F�N�g�Ƃ���)�������Ă����I�u�W�F�N�g.�����Ȃǂ̍ۂɈʒu���Q�Ƃ���.
    protected static readonly int queueLimit = 20;    // ���������g�������Ȃ��悤�AQueue�̏�������߂Ă����B
    protected readonly Queue<GameObject> bulletQue = new(queueLimit); // ��ɏo���e��Queue�ɓ���Ă����ă��T�C�N������B

    private SoundManager soundManager; //�T�E���h�}�l�[�W���[
    protected GameManager gameManager;

    //-----PUBLICVARIABLES-----//
    public int limit = 8;    // ��ɑ��݂ł��鎩�@�̒e�̐��������Ɋi�[.
    public float bulletSpeed = 20;  // �e�̏����𐧌䂷��ϐ�.
    public Rigidbody Cannon;    // �C��̌������擾���邽�߂ɒǉ�.

    // Start is called before the first frame update
    protected virtual void Start()
    {
        soundManager = SoundManager.Instance;
        gameManager = GameManager.Instance;
        StartCoroutine(LoadBulletPrefabAsync());
        bulletMark.SetActive(false);    // �ڈ�ƂȂ�I�u�W�F�N�g�͎ז��Ȃ̂ŁAactive��false�ɃZ�b�g���Ă���.
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // �}�E�X�̍��N���b�N�����m���Ēe�𔭎˂���.
        if(Input.GetMouseButtonDown(0) && Time.timeScale > 0) RecycleShot();
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

    void Shot()
    {
        if ((!gameManager.pausedForWating) && gameManager.GameIsPlaying)
        {
            bulletCopy.transform.position = bulletMark.transform.position;
            bulletCopy.SetActive(true);
            bulletCopy.GetComponent<Bullet>().SetShouldExplode(false);
            bulletCopy.GetComponent<BulletSpeedManager>().SetSpeed(bulletSpeed);
            bulletCopy.GetComponent<Rigidbody>().velocity = -Cannon.transform.up * bulletSpeed;    // -cannon.transform.up�͖C��̑O����.
            soundManager.Play("shot");
            bulletQue.Enqueue(bulletCopy);
        }
    }
    void RotateQueue()
    {
        bulletCopy = bulletQue.Dequeue();
        bulletQue.Enqueue(bulletCopy);
    }
    protected virtual void GenerateBulletCopy()
    {
        if ((!gameManager.pausedForWating) && gameManager.GameIsPlaying)
        {
            // ����͒e�����`�Ȃ̂ŁA�e�̉�]�͍l������identity�Ő���.
            bulletCopy = Instantiate(bulletPrefab, bulletMark.transform.position, Quaternion.identity);
            DontDestroyOnLoad(bulletCopy);
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
