using System.Collections;
using UnityEngine;

public class EnemyBulletController : BulletController
{
    private IEnumerator ShotAtRegularInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            RecycleShot();
        }
    }

// Start is called before the first frame update
protected override void Start()
    {
        base.Start();
        StartCoroutine(ShotAtRegularInterval());
    }
    protected override void Update()
    {
       // DoNothing
    }

    protected override void GenerateBulletCopy()
    {
        if ((!gameManager.pausedForWating) && gameManager.GameIsPlaying)
        {
            // 今回は弾が球形なので、弾の回転は考慮せずidentityで生成.
            bulletCopy = Instantiate(bulletPrefab, bulletMark.transform.position, Quaternion.identity);
            // DontDestroyOnLoad(bulletCopy);この行を消去.
            bulletQue.Enqueue(bulletCopy);
        }
    }
}
