using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private GameObject bulletCopy; // 実際に発射される弾.
    [SerializeField] private GameObject bulletMark;   // 弾を発射する起点となる所に(砲台の子オブジェクトとして)くっつけておくオブジェクト.生成などの際に位置を参照する.
    static private GameObject bulletPrefab; // Resourcesからプレハブをロードするための仮変数.
    private static readonly int queueLimit = 20;    // メモリを使いすぎないよう、Queueの上限を決めておく。
    private readonly Queue<GameObject> bulletPool = new(queueLimit); // 場に出た弾をQueueに入れておいてリサイクルする。
    private Vector3 CannonForward;

    //-----PUBLICVARIABLES-----//
    public int limit = 8;    // 場に存在できる自機の弾の数をここに格納.
    public float bulletSpeed = 20;  // 弾の初速を制御する変数.

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(LoadBulletPrefabAsync());
        bulletMark.SetActive(false);    // 目印となるオブジェクトは邪魔なので、activeをfalseにセットしておく.
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
            Debug.Log("Resourcesフォルダにbulletのプレハブが見つかりません");
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
        // 今回は弾が球形なので、弾の回転は考慮せずidentityで生成.
        bulletCopy = Instantiate(bulletPrefab, bulletMark.transform.position, Quaternion.identity);
        bulletPool.Enqueue(bulletCopy);
        if (gameObject.CompareTag("Player")) DontDestroyOnLoad(bulletCopy);
    }
    void Fire()
    {
        if (GameManager.IsWaiting() || !GameManager.gameIsPlaying) return;
        bulletCopy.transform.position = bulletMark.transform.position;  // 位置をマークの位置に
        bulletCopy.GetComponent<Bullet>().SetShouldExplode(false);
        bulletCopy.GetComponent<BulletSpeedManager>().SetSpeed(bulletSpeed);    // 射出速度をセットする.
        bulletCopy.GetComponent<Rigidbody>().velocity = CannonForward * bulletSpeed;
        bulletCopy.GetComponent<BulletCollisionManager>().ResetCount(); // 衝突回数をリセットする.
        bulletCopy.SetActive(true); // bulletCopyをactiveにする.
        SoundManager.Play("fire");
    }
    public void RecycleFire()
    {
        // Queueに入っている弾の数が上限より小さいなら、新しく生成しQueueに追加.
        if (bulletPool.Count < limit)
        {
            GenerateBulletCopy();   //  バレットのコピーを生成しQueueに入れる.
            Fire(); //  弾を初期化して射出する.
            return;
        }    
        
        // 弾が上限に達したら、Queueに入っているものを参照してリサイクルする.
        // Activeがfalseなら弾をリサイクルする.
        for (int i = 0; i < limit; ++i)
        {
            RotateQueue();  // Queueを回し、次の弾をbulletCopyに入れる.
            bool UsingNow = bulletCopy.activeSelf;  // 弾が使用中なら次に進める.
            if (UsingNow) continue;
            Fire();
            break;
        }
    }
}