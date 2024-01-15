using System.Collections;
using System.Collections.Generic;
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
}
