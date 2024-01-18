using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // BulletController�Ƃ�����̃I�u�W�F�N�g�𐶐����Ă���ɃA�^�b�`���Ă�������.
    // BulletController�̓t�B�[���h�̒e�̐��𐧌����邽�߂̃I�u�W�F�N�g�ŁA��ɂ��̏������s���܂�.

    private GameObject obj; // Resources����v���n�u�����[�h���邽�߂̉��ϐ�.
    private GameObject bulletCopy; // ���ۂɔ��˂����e.
    [SerializeField]
    private SoundManager soundManager; //�T�E���h�}�l�[�W���[
    static readonly int queueLimit = 20;
    private readonly Queue<GameObject> bulletQue = new Queue<GameObject>(queueLimit); // ��ɏo���e��Queue�ɓ���Ă����ă��T�C�N������.3��Queue�̏��.
    public int limit = 8;    // ��ɑ��݂ł��鎩�@�̒e�̐��������Ɋi�[.
    public Rigidbody Cannon;    // �C��̌������擾���邽�߂ɒǉ�.
    public GameObject bullet;   // �e�𔭎˂���N�_�ƂȂ鏊��(�C��̎q�I�u�W�F�N�g�Ƃ���)�������Ă����I�u�W�F�N�g.�����Ȃǂ̍ۂɈʒu���Q�Ƃ���.
    public float bulletSpeed = 20;  // �e�̏����𐧌䂷��ϐ�.
    public int durationTimes = 3;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        obj = (GameObject)Resources.Load("bullet"); //  �v���n�u�̃f�[�^�����[�h.
        bullet.SetActive(false);    // �ڈ�ƂȂ�I�u�W�F�N�g�͎ז��Ȃ̂ŁAactive��false�ɃZ�b�g���Ă���.
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // �}�E�X�̍��N���b�N�����m���Ēe�𔭎˂���.
        if (Input.GetMouseButtonDown(0)) RecycleShot();
    }


    void shot()
    {
        bulletCopy.transform.position = bullet.transform.position;
        bulletCopy.SetActive(true);
        bulletCopy.GetComponent<Rigidbody>().velocity = -Cannon.transform.up * bulletSpeed;    // -cannon.transform.up�͖C��̑O����.
        bulletCopy.GetComponent<BulletCollision>().speed = bulletSpeed;
        bulletCopy.GetComponent<BulletCollision>().durationTimes = durationTimes;
        soundManager.Play("shot");
    }
    void rotateQueue()
    {
        bulletCopy = bulletQue.Dequeue();
        bulletQue.Enqueue(bulletCopy);
    }
    void GenerateBulletCopy()
    {
        // ����͒e�����`�Ȃ̂ŁA�e�̉�]�͍l������identity�Ő���.
        bulletCopy = Instantiate(obj, bullet.transform.position, Quaternion.identity);
        bulletCopy.GetComponent<BulletCollision>().soundManager = soundManager;
        bulletQue.Enqueue(bulletCopy);
    }
    protected void RecycleShot()
    {
        // Queue�ɓ����Ă���e�̐��������菬�����Ȃ�A�V����������Queue�ɒǉ�.
        if (bulletQue.Count < limit)
        {
            GenerateBulletCopy();
            shot();
        }
        else
        {
            // �e������ɒB������AQueue�ɓ����Ă�����̂��Q�Ƃ��ă��T�C�N������.
            // Active��false�Ȃ�e�����T�C�N������.
            for (int i = 0; i < limit; ++i)
            {
                rotateQueue();
                bool CanUseMore = bulletCopy.activeSelf;
                if (!CanUseMore)
                {
                    shot();
                    break;
                }
            }
        }
    }
}
