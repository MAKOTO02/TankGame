using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bulletの初期化の際に渡さなければいけない情報をここで一括管理します。
/// bulletクラスの変数を併せて確認してください。
/// </summary>
public class BuleltInstantiateInfo
{
    public Bullet bullet;
    public BulletCollisionManager collisionManager;
    public BulletSpeedManager speedManager;
    public SoundManager soundManager;

    // コンストラクタです
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
    // BulletControllerはフィールドの弾の数を制限するためのオブジェクトで、主にその処理を行います.

    //-----PRIVATEVARIABLES-----//
    private GameObject bulletPrefab; // Resourcesからプレハブをロードするための仮変数.
    private GameObject bulletCopy; // 実際に発射される弾.

    [SerializeField] private GameObject bulletMark;   // 弾を発射する起点となる所に(砲台の子オブジェクトとして)くっつけておくオブジェクト.生成などの際に位置を参照する.
    private static readonly int queueLimit = 20;    // メモリを使いすぎないよう、Queueの上限を決めておく。
    private readonly Queue<GameObject> bulletQue = new(queueLimit); // 場に出た弾をQueueに入れておいてリサイクルする。

    public SoundManager soundManager; //サウンドマネージャー

    //-----PUBLICVARIABLES-----//
    public int limit = 8;    // 場に存在できる自機の弾の数をここに格納.
    public float bulletSpeed = 20;  // 弾の初速を制御する変数.
    public Rigidbody Cannon;    // 砲台の向きを取得するために追加.

    // Start is called before the first frame update
    protected virtual void Start()
    {
        bulletPrefab = (GameObject)Resources.Load("bullet"); //  プレハブのデータをロード.
        bulletMark.SetActive(false);    // 目印となるオブジェクトは邪魔なので、activeをfalseにセットしておく.
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // マウスの左クリックを感知して弾を発射する.
        if (Input.GetMouseButtonDown(0)) RecycleShot();
    }


    void Shot()
    {
        bulletCopy.transform.position = bulletMark.transform.position;
        bulletCopy.SetActive(true);
        bulletCopy.GetComponent<Bullet>().SetShouldExplode(false);
        bulletCopy.GetComponent<BulletSpeedManager>().SetSpeed(bulletSpeed);
        bulletCopy.GetComponent<Rigidbody>().velocity = -Cannon.transform.up * bulletSpeed;    // -cannon.transform.upは砲台の前向き.
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
                Debug.Log("サウンドマネージャーの参照に失敗しました");
                Debug.Log("BulletControllerにSoundManagerを渡してください");
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
        // 今回は弾が球形なので、弾の回転は考慮せずidentityで生成.
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
                Debug.Log("サウンドマネージャーの参照に失敗しました");
                Debug.Log("BulletControllerにSoundManagerを渡してください");
            }
            bulletQue.Enqueue(bulletCopy);
        }
    }
    protected void RecycleShot()
    {
        // Queueに入っている弾の数が上限より小さいなら、新しく生成しQueueに追加.
        if (bulletQue.Count < limit)
        {
            GenerateBulletCopy();
            Shot();
        }
        else
        {
            // 弾が上限に達したら、Queueに入っているものを参照してリサイクルする.
            // Activeがfalseなら弾をリサイクルする.
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
