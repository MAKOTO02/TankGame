using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static readonly object lockObject = new object();

    public static T Instance
    {
        get
        {
            // ダブルチェックロッキングを使用して、最初のnullチェックをスレッドセーフに
            if (instance == null)
            {
                lock (lockObject)
                {
                    // 再度nullチェックを行う
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();

                        if (instance == null)
                        {
                            GameObject singletonObject = new GameObject(typeof(T).Name);
                            instance = singletonObject.AddComponent<T>();
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                }
            }

            return instance;
        }
    }

    // Awakeメソッドを使ってDontDestroyOnLoadを呼び出す
    protected virtual void Awake()
    {
        if (Instance != this)
        {
            // 既にインスタンスが存在する場合は、このオブジェクトを破棄する
            Destroy(gameObject);
            Debug.LogWarning("シングルトンオブジェクトの競合が確認されました");
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}