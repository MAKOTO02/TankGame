using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // BulletControllerという空のオブジェクトを生成してそれにアタッチしてください.
    // BulletControllerはフィールドの弾の数を制限するためのオブジェクトで、主にその処理を行います.

    //-----PRIVATEVARIABLES-----//
    private GameObject BulletPrefab; // Resourcesからプレハブをロードするための仮変数.
    private GameObject bulletCopy; // 実際に発射される弾.
    [SerializeField] private SoundManager soundManager; //サウンドマネージャー
    [SerializeField] private GameObject bullet;   // 弾を発射する起点となる所に(砲台の子オブジェクトとして)くっつけておくオブジェクト.生成などの際に位置を参照する.
    private static readonly int queueLimit = 20;    // メモリを使いすぎないよう、Queueの上限を決めておく。
    private readonly Queue<GameObject> bulletQue = new Queue<GameObject>(queueLimit); // 場に出た弾をQueueに入れておいてリサイクルする。

    //-----PUBLICVARIABLES-----//
    public int limit = 8;    // 場に存在できる自機の弾の数をここに格納.
    public int durationTimes = 3;
    public float bulletSpeed = 20;  // 弾の初速を制御する変数.
    public Rigidbody Cannon;    // 砲台の向きを取得するために追加.

    // Start is called before the first frame update
    protected virtual void Start()
    {
        BulletPrefab = (GameObject)Resources.Load("bullet"); //  プレハブのデータをロード.
        bullet.SetActive(false);    // 目印となるオブジェクトは邪魔なので、activeをfalseにセットしておく.
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // マウスの左クリックを感知して弾を発射する.
        if (Input.GetMouseButtonDown(0)) RecycleShot();
    }


    void Shot()
    {
        bulletCopy.transform.position = bullet.transform.position;
        bulletCopy.SetActive(true);
        bulletCopy.GetComponent<Rigidbody>().velocity = -Cannon.transform.up * bulletSpeed;    // -cannon.transform.upは砲台の前向き.
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
        // 今回は弾が球形なので、弾の回転は考慮せずidentityで生成.
        bulletCopy = Instantiate(BulletPrefab, bullet.transform.position, Quaternion.identity);
        bulletCopy.GetComponent<BulletCollision>().soundManager = soundManager;
        try
        {
            bulletCopy.GetComponent<BulletCollision>().BulletController = GetComponent<BulletController>();
            if (bulletCopy.GetComponent<BulletCollision>() == null)
            {
                Debug.Log("BulletController.cs: BulletCopyの初期化に失敗しています");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("BulletController.cs: " + e.Message);
        }
        bulletQue.Enqueue(bulletCopy);
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
                rotateQueue();
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
