using System.Collections;
using UnityEngine;

public class EnemyBulletController : BulletController
{
    private IEnumerator ShotAtRegularInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            RecycleFire();
        }
    }

// Start is called before the first frame update
void Start()
    {
        StartCoroutine(ShotAtRegularInterval());
    }
}
