using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // シーン内で同じ型のオブジェクトが存在するか確認
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    // 見つからなければ新しく生成し、シーンが切り替わっても破棄されないようにする
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }

    // 以下はオプション: Awakeメソッドを使ってDontDestroyOnLoadを呼び出す
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

