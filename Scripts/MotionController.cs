using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour
{
    //------ PUBLIC VARIABLES ------//
    public float MoveSpeedLimit = 20;
    public float Accel = 800;
    public float TurnSpeed = 80;

    //------ PRIVATE VARIABLES ------//
    protected Animator Anima;
    protected Rigidbody Rb;
    protected GameObject Cannon;
    protected float moveInput = 0;
    protected float turnInput = 0;
    protected Vector3 torque;

    //----- PRIVATE METHODS -----//

    //----- PUBLIC METHODS -----//

    protected virtual void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Anima = GetComponent<Animator>();
        Cannon = transform.Find("Cannon").gameObject;
        if (Rb == null || Anima == null || Cannon == null)
        {
            Debug.Log("Error");
        }
    }

    protected virtual void Update()
    {
        Move();Turn();AnimationSet();TurnCannon();
    }

    void Move()
    {
        // �������ɒn�ʂ����邩�𔻒�
        // �n�ʂ̔���ɂ��ẮADeubug�����Ă��Ȃ��̂ŁA�����͕s���ł�.
        if(Rb != null)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out _, 1.0f))
            {
                if (Rb.velocity.magnitude < MoveSpeedLimit)
                {
                    Rb.AddForce(transform.forward * Accel * moveInput, ForceMode.Force);    // rb.MovePosition���ƕǂ��ђʂ���̂ŕύX
                }
                if (Mathf.Abs(moveInput) < 0.05f)
                {
                    Rb.velocity = Vector3.zero;
                }
            }
        }
        
    }
    void Turn()
    {
        if(Rb != null)
        {
            Rb.MoveRotation(Rb.rotation * Quaternion.Euler(0, turnInput * TurnSpeed * Time.deltaTime, 0));
        }
    }
    void AnimationSet()
    {
        if(Anima != null)
        {
            Anima.SetFloat("MoveSpeed", 0.4f * Rb.velocity.magnitude);  // moveSpeed ���ő�(= 20) �̂Ƃ��Đ����x 8 �ɂȂ�悤�ɕ␳���āA�Đ����x���Z�b�g
            Anima.SetFloat("TurnSpeed", 4.0f);  // ����A�^�[���̃A�j���[�V�����ł͉������Ȃ��̂ŁA���̒l���̗p���܂�.
            if (moveInput > 0)
            {
                Anima.SetBool("rep", true);
                Anima.SetBool("reprev", false);
            }
            else if (moveInput < 0)
            {
                Anima.SetBool("rep", false);
                Anima.SetBool("reprev", true);
            }
            else if (moveInput == 0)
            {

                Anima.SetBool("rep", false);
                Anima.SetBool("reprev", false);
            }
            if (turnInput > 0)
            {
                Anima.SetBool("TurnR", true);
                Anima.SetBool("TurnL", false);
            }
            else if (turnInput < 0)
            {
                Anima.SetBool("TurnR", false);
                Anima.SetBool("TurnL", true);
            }
            else if (turnInput == 0)
            {

                Anima.SetBool("TurnL", false);
                Anima.SetBool("TurnR", false);
            }
        }
    }
    
    protected virtual void TurnCannon()
    {
        // Do Nothig
    }
}
