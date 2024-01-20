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
                // �V�[�����œ����^�̃I�u�W�F�N�g�����݂��邩�m�F
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    // ������Ȃ���ΐV�����������A�V�[�����؂�ւ���Ă��j������Ȃ��悤�ɂ���
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }

    // �ȉ��̓I�v�V����: Awake���\�b�h���g����DontDestroyOnLoad���Ăяo��
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

