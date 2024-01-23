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
        // すぐ下に地面があるかを判定
        // 地面の判定については、Deubug等していないので、挙動は不明です.
        if (!Physics.Raycast(transform.position, Vector3.down, out _, 1.0f)) return;
        // 下に地面があるときは入力に従い動かす.
        if (thisRigidbody.velocity.magnitude < MoveSpeedLimit)
        {
            thisRigidbody.AddForce(transform.forward * Accel * moveInput, ForceMode.Force);    // rb.MovePositionだと壁を貫通するので変更
        }
        // 慣性を消去し、滑らないようにする.
        DeleteSpeed();
    }  
    void Turn()
    {
        thisRigidbody.MoveRotation(thisRigidbody.rotation * Quaternion.Euler(0, turnInput * TurnSpeed * Time.deltaTime, 0));
    }
    void AnimationSet()
    {
        animator.SetFloat("MoveSpeed", 0.4f * thisRigidbody.velocity.magnitude);  // moveSpeed が最大(= 20) のとき再生速度 8 になるように補正して、再生速度をセット
        animator.SetFloat("TurnSpeed", 4.0f);  // 今回、ターンのアニメーションでは加速がないので、一定の値を採用します.
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
