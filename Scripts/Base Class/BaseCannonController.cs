using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseCannonController : MonoBehaviour
{
    //----- PUBLIC VARIABLES -----//
    public float turnSpeed = 2.0f;
    public Vector3 CannonForward {  get; private set; }

    [SerializeField] private Rigidbody Cannon;
    protected Vector3 AimDirection;
 
    // Start is called before the first frame update
    protected virtual void Start()
    {        
        Cannon.angularDrag = 5.0f;
        Cannon.useGravity = false;
        CannonForward = (-Cannon.transform.up).normalized;
        AimDirection = CannonForward;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CannonForward = -Cannon.transform.up;
    }

    protected  virtual void TurnCannon()
    {
        Vector3 torque = Vector3.Cross(CannonForward, AimDirection.normalized) * turnSpeed;
        Cannon.AddTorque(torque);
    }
}
