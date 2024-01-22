using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //------ PUBLIC VARIABLES ------//
    public Transform target;    // unity���ŎQ�Ƃ�^����
    public float sensitiveRotate = 5.0f;    // ���x
    //------ PRIVATE VARIABLES ------//
    // private Camera cam = GetComponent<Camera>();
    private Vector3 offsetPos;

    // Start is called before the first frame update
    void Start()
    {
        offsetPos = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            AdjustPos();
            MouseRightInput();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Reset();
            }
        }
    }

    void AdjustPos()
    {
        // �@�̂̒ǔ����� 
        transform.position = target.position + transform.rotation * offsetPos;
    }
    void MouseRightInput()
    {
        // �}�E�X�̒ǔ�����
        // �E�N���b�N����Ă���Ƃ�����
        if (Input.GetMouseButton(1))
        {
            float RotateX = Input.GetAxis("Mouse X") * sensitiveRotate;
            transform.Rotate(0.0f, RotateX, 0.0f);
        }
    }
    void Reset()
    {
        transform.rotation = target.rotation;
        transform.position = target.position + transform.rotation * offsetPos;
    }
}
