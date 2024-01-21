using UnityEngine;

/// <summary>
/// Bullet�N���X�́A�e�̑��x�ƒe�̔��ˉ񐔂��Ǘ����܂��B
/// desiredSpeed�͔��˂̍ۂɏ��������ĉ������B
/// </summary>

[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    private bool ShouldExplode;
    private GameObject explosion;
    private GameObject explosionCopy;

    public SoundManager soundManager; //�T�E���h�}�l�[�W���[

    // Start is called before the first frame update
    void Start()
    {
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

    // �O������desiredSpeed��ݒ肷�郁�\�b�h

    public void SetShouldExplode(bool setting)
    {
        ShouldExplode = setting;
    }

    public void Explode()
    {
        explosionCopy = Instantiate(explosion, transform.position, Quaternion.identity);
        try
        {
            soundManager.Play("hit");
        }
        catch (System.Exception e)
        {
            Debug.Log("Bullet.cs��SoundManager��n���Ă��܂���");
            Debug.Log(e.Message);
        }
        finally
        {
            if (explosionCopy != null) Destroy(explosionCopy, explosionCopy.GetComponent<ParticleSystem>().main.duration);
            InitiateBullet();
            GameObject ObjectToDisable = GetComponent<BulletCollisionManager>().GetObjectToDisable();
            if (ObjectToDisable != null && ObjectToDisable.activeSelf) ObjectToDisable.SetActive(false);
        }
    }
    void InitiateBullet()
    {
        ShouldExplode = false;
        gameObject.SetActive(false);
    }
}
