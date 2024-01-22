using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // BulletControllerはフィールドの弾の数を制限するためのオブジェクトで、主にその処理を行います.

    //-----PRIVATEVARIABLES-----//
    static protected GameObject bulletPrefab; // Resourcesからプレハブをロードするための仮変数.
    protected GameObject bulletCopy; // 実際に発射される弾.
    
    [SerializeField] public GameObject bulletMark;   // 弾を発射する起点となる所に(砲台の子オブジェクトとして)くっつけておくオブジェクト.生成などの際に位置を参照する.
    protected static readonly int queueLimit = 20;    // メモリを使いすぎないよう、Queueの上限を決めておく。
    protected readonly Queue<GameObject> bulletQue = new(queueLimit); // 場に出た弾をQueueに入れておいてリサイクルする。

    private SoundManager soundManager; //サウンドマネージャー
    protected GameManager gameManager;

    //-----PUBLICVARIABLES-----//
    public int limit = 8;    // 場に存在できる自機の弾の数をここに格納.
    public float bulletSpeed = 20;  // 弾の初速を制御する変数.
    public Rigidbody Cannon;    // 砲台の向きを取得するために追加.

    // Start is called before the first frame update
    protected virtual void Start()
    {
        soundManager = SoundManager.Instance;
        gameManager = GameManager.Instance;
        StartCoroutine(LoadBulletPrefabAsync());
        bulletMark.SetActive(false);    // 目印となるオブジェクトは邪魔なので、activeをfalseにセットしておく.
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // マウスの左クリックを感知して弾を発射する.
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
            Debug.Log("Resourcesフォルダにbulletのプレハブが見つかりません");
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
            bulletCopy.GetComponent<Rigidbody>().velocity = -Cannon.transform.up * bulletSpeed;    // -cannon.transform.upは砲台の前向き.
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
            // 今回は弾が球形なので、弾の回転は考慮せずidentityで生成.
            bulletCopy = Instantiate(bulletPrefab, bulletMark.transform.position, Quaternion.identity);
            DontDestroyOnLoad(bulletCopy);
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
