using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : NetworkBehaviour
{

    [Header("Movement")]
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] float turnSpeed = 15f;
    [SerializeField] float reverseSpeedDivisor = 2f;
    [SerializeField] float groundCheckOffset = -0.75f; // Ensures the the player still has control just after leaving the water. 
    [SerializeField] float tiltThreshold = 0.5f; //How far the boat can tilt before it needs to be corrected (used to prevent the boat capsizing)
    [SerializeField] float tiltCorrectionTime = 1f;

    [Header("Drag")]
    [SerializeField] float movingDrag = 1f;
    [SerializeField] float stoppingDrag = 2f;

    [Header("Rigidbody Setup")]
    [SerializeField] float maxAngularSpeed = 2f;
    [SerializeField] bool isInWater;
    [SerializeField] LayerMask waterLayerMask;

    [SerializeField] GameObject fx1;
    [SerializeField] GameObject fx2;

    Vector3 velocity = Vector3.zero;
    Vector3 angularVelocity = Vector3.zero;
    Rigidbody rb;

#region Unity Methods
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularSpeed;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner){return;}
        
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (!IsOwner){return;}
    }

    void Update()
    {
        if (IsOwner){
            if (isInWater){
            fx1.SetActive(true);
            fx2.SetActive(true);
        }
        else{
            fx1.SetActive(false);
            fx2.SetActive(false);
        }
        }

        
    }

    private   void OnTriggerStay(Collider other) {
        
    
        if (((1 << other.gameObject.layer) & waterLayerMask) != 0) {
        
            isInWater = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        
        if (((1 << other.gameObject.layer) & waterLayerMask) != 0) {
        
            isInWater = false;
        }
    }

    void FixedUpdate()
    {
        if (IsOwner){
                if (isInWater)
            {
                PerformMovement();
                PerformRotation();
            }
            PreventFlip(); 
        }
        
    }
#endregion

#region Public Methods
    
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity * maxSpeed;
    }

    public void Rotate(Vector3 _angularVelocity)
    {
        angularVelocity = _angularVelocity * turnSpeed;

	}

    


    public float GetCurrentSpeed()
    {
        return transform.InverseTransformDirection(rb.linearVelocity).z;
    }
#endregion


    
    

    void PreventFlip()
    {
        
        Quaternion currentRotation = transform.rotation;

        
        Quaternion targetRotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);

        
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime / tiltCorrectionTime);
    }



#region Private Methods
    void PerformMovement()
    {
        AlterDrag();
        if (GetCurrentSpeed() > 0)
        {
            rb.AddForce(velocity, ForceMode.Acceleration);
        }
        else if (GetCurrentSpeed() < 0)
        {
            rb.AddForce(velocity / reverseSpeedDivisor, ForceMode.Acceleration);
        }
    }

    void PerformRotation()
    {
        if (angularVelocity != Vector3.zero)
        {
            rb.AddTorque(angularVelocity, ForceMode.Acceleration);
        }
    }

    void AlterDrag()
    {
        if (velocity != Vector3.zero)
        {
            rb.linearDamping = movingDrag;
        }
        else
        {
            rb.linearDamping = stoppingDrag;
        }
    }
#endregion
}