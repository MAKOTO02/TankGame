using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    private static IEnumerator PlayAndStop(ParticleSystem particleSystem)
    {
        particleSystem.Play();
        yield return new WaitWhile(() => particleSystem.isPlaying);
        particleSystem.Stop();
    }

    private GameObject ExplosionPrefab;
    static private int EffectPoolSize = 20;
    private Queue<GameObject> effectPool = new Queue<GameObject>(EffectPoolSize);

    // Start is called before the first frame update
    protected override void Awake()
    {
        ExplosionPrefab = (GameObject)Resources.Load("Explosion");
        InitializeEffectPool();
    }

    private void InitializeEffectPool()
    {
        for (int i = 0; i < EffectPoolSize; i++)
        {
            GameObject explosionEffect = Instantiate(ExplosionPrefab, Vector3.zero, Quaternion.identity);
            explosionEffect.GetComponent<ParticleSystem>().Stop();
            explosionEffect.SetActive(false);
            explosionEffect.transform.parent = transform;
            effectPool.Enqueue(explosionEffect);
        }
    }

    // Update is called once per frame
    public void PlayEffect(Vector3 position)
    {
        GameObject explosionCopy = effectPool.Dequeue();
        explosionCopy.transform.position = position;
        explosionCopy.SetActive(true);
        // �񓯊������̂��߂�Coroutine��Ԃ��A�Đ����I�������v�[���ɖ߂�
        StartCoroutine(PlayAndStopCoroutine(explosionCopy.GetComponent<ParticleSystem>(), explosionCopy));
    }

    private IEnumerator PlayAndStopCoroutine(ParticleSystem particleSystem, GameObject effectObject)
    {
        yield return PlayAndStop(particleSystem);
        effectObject.SetActive(false);
        effectPool.Enqueue(effectObject);
    }
}
