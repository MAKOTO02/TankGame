using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BulletCollision : MonoBehaviour
{
    // BulletCollisionは弾のActiveと非Active状態を管理するためのスクリプトです.
    // Bulletのゲームオブジェクトのプレハブを作り、それにアタッチしてください.
    // BulletオブジェクトのコライダーにPhiscMaterialをつけられるので、
    // そこで、摩擦0,反発係数1、FrictionConbineをminimum,BounceConbineをmaximumにセットして下さい.

    [SerializeField] public SoundManager soundManager; //サウンドマネージャー

    private GameObject explosion;
    private GameObject explosionCopy;
    public int durationTimes = 3;    // 弾の跳弾回数の上限を定めます.
    public float maxSpeed = 40; // 速度の挙動がおかしくなったとき、非Activeに戻します.
    public float minSpeed = 10;
    private int count = 0;  // 現在の衝突回数を格納しておく変数です.

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        count = 0;
        explosion = (GameObject)Resources.Load("Explosion");
    }

    void Update()
    {
        // 毎フレーム速度を取得して監視を行います.
        //　速度が一定の範囲を超えると非アクティブ化
        InvalidSpeedCheck();
    }

    void OnCollisionEnter(Collision collision)
    {
        bool collideActor = collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player";
        if (collideActor || count >= durationTimes)
        {
            explosionCopy = Instantiate(explosion, transform.position, Quaternion.identity);
            if(collideActor ) 
            {
                soundManager.Play("hit");
                collision.gameObject.SetActive(false);
            }
            if(collision.gameObject.tag == "Player") 
            {
                Debug.Log("ゲームオーバー");
            }
            Destroy(explosionCopy, explosionCopy.GetComponent<ParticleSystem>().main.duration);
            initiateBullet();
        }
        else
        {
            // 反射の音を再生する.
            count++;
        }
    }

    void initiateBullet()
    {
        gameObject.SetActive(false);
        count = 0;
    }
    void InvalidSpeedCheck()
    {
        float speed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        if (speed > maxSpeed || speed < minSpeed)
        {
            // Effect や Sound の再生を行う.
            soundManager.Play("hot");
            initiateBullet();
        }
    }
}