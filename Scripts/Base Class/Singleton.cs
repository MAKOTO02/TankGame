using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static readonly object lockObject = new object();

    public static T Instance
    {
        get
        {
            // �_�u���`�F�b�N���b�L���O���g�p���āA�ŏ���null�`�F�b�N���X���b�h�Z�[�t��
            if (instance == null)
            {
                lock (lockObject)
                {
                    // �ēxnull�`�F�b�N���s��
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

    // Awake���\�b�h���g����DontDestroyOnLoad���Ăяo��
    protected virtual void Awake()
    {
        if (Instance != this)
        {
            // ���ɃC���X�^���X�����݂���ꍇ�́A���̃I�u�W�F�N�g��j������
            Destroy(gameObject);
            Debug.LogWarning("�V���O���g���I�u�W�F�N�g�̋������m�F����܂���");
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}