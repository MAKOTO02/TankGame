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
            // ¡‰ñ‚Í’e‚ª‹…Œ`‚È‚Ì‚ÅA’e‚Ì‰ñ“]‚Íl—¶‚¹‚¸identity‚Å¶¬.
            bulletCopy = Instantiate(bulletPrefab, bulletMark.transform.position, Quaternion.identity);
            // DontDestroyOnLoad(bulletCopy);‚±‚Ìs‚ğÁ‹.
            bulletQue.Enqueue(bulletCopy);
        }
    }
}
