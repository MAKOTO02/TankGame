using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(BoxCollider))]
[RequireComponent (typeof(Animator))]
public class MotionController : MonoBehaviour
{
    //------ PUBLIC VARIABLES ------//
    public float MoveSpeedLimit = 20;
    public float Accel = 800;
    public float TurnSpeed = 80;

    //------ PRIVATE VARIABLES ------//
    private Animator animator;

    //----- PROTECTED VARIABLES -----//
    protected Rigidbody thisRigidbody;
    protected float moveInput = 0;
    protected float turnInput = 0;

    protected virtual void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        thisRigidbody.mass = 100.0f;
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (GameManager.IsWaiting() || !GameManager.gameIsPlaying) return;
        Move(); Turn(); AnimationSet();
    }

    void Move()
    {
        // �������ɒn�ʂ����邩�𔻒�
        // �n�ʂ̔���ɂ��ẮADeubug�����Ă��Ȃ��̂ŁA�����͕s���ł�.
        if (!Physics.Raycast(transform.position, Vector3.down, out _, 1.0f)) return;
        // ���ɒn�ʂ�����Ƃ��͓��͂ɏ]��������.
        if (thisRigidbody.velocity.magnitude < MoveSpeedLimit)
        {
            thisRigidbody.AddForce(transform.forward * Accel * moveInput, ForceMode.Force);    // rb.MovePosition���ƕǂ��ђʂ���̂ŕύX
        }
        // �������������A����Ȃ��悤�ɂ���.
        DeleteSpeed();
    }  
    void Turn()
    {
        thisRigidbody.MoveRotation(thisRigidbody.rotation * Quaternion.Euler(0, turnInput * TurnSpeed * Time.deltaTime, 0));
    }
    void AnimationSet()
    {
        animator.SetFloat("MoveSpeed", 0.4f * thisRigidbody.velocity.magnitude);  // moveSpeed ���ő�(= 20) �̂Ƃ��Đ����x 8 �ɂȂ�悤�ɕ␳���āA�Đ����x���Z�b�g
        animator.SetFloat("TurnSpeed", 4.0f);  // ����A�^�[���̃A�j���[�V�����ł͉������Ȃ��̂ŁA���̒l���̗p���܂�.
        if (moveInput > 0)
        {
            animator.SetBool("rep", true);
            animator.SetBool("reprev", false);
        }
        else if (moveInput < 0)
        {
            animator.SetBool("rep", false);
            animator.SetBool("reprev", true);
        }
        else if (moveInput == 0)
        {

            animator.SetBool("rep", false);
            animator.SetBool("reprev", false);
        }

        if (turnInput > 0)
        {
            animator.SetBool("TurnR", true);
            animator.SetBool("TurnL", false);
        }
        else if (turnInput < 0)
        {
            animator.SetBool("TurnR", false);
            animator.SetBool("TurnL", true);
        }
        else if (turnInput == 0)
        {

            animator.SetBool("TurnL", false);
            animator.SetBool("TurnR", false);
        }
    }

    void DeleteSpeed()
    {
        if (Mathf.Abs(moveInput) < 0.05f)
        {
            thisRigidbody.velocity = new Vector3(0.0f, thisRigidbody.velocity.y, 0.0f);
        }
    }
}
