using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
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

    Vector3 velocity = Vector3.zero;
    Vector3 angularVelocity = Vector3.zero;
    Rigidbody rb;

#region Unity Methods
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularSpeed;
    }

    void Update()
    {
        //TODO Set the play state of your particle systems based on whether the boat is in/out of the water.  
    }

    void FixedUpdate()
    {
        if (IsInWater())
        {
            PerformMovement();
            PerformRotation();
        }
        PreventFlip(); //TODO: Write this method (outline below)
    }
#endregion

#region Public Methods
    /// <summary>
    /// Sets the velocity of the boat.
    /// Takes in a normalised vector.
    /// </summary>
    /// <param name="_velocity"></param>
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity * maxSpeed;
    }

    /// <summary>
    /// Sets the angular velocity of the boat.
    /// Takes in a normalised vector.
    /// </summary>
    /// <param name="_angularVelocity"></param>
    public void Rotate(Vector3 _angularVelocity)
    {
        angularVelocity = _angularVelocity * turnSpeed;

	}

    /// <summary>
    /// Returns the current speed of the boat.
    /// </summary>
    public float GetCurrentSpeed()
    {
        return transform.InverseTransformDirection(rb.linearVelocity).z;
    }
#endregion

#region Challenges
    /// <summary>
    /// Checks whether the boat is in the water.
    /// </summary>
    /// <returns></returns>
    public bool IsInWater()
    {
        //TODO: Make it so that the player can only move and maneuver the boat while they are in the water
        //Note: You can get the current water level from WaterLevel.cs
        return true;
    }

    void PreventFlip()
    {
        // Obtén la rotación actual del barco
        Quaternion currentRotation = transform.rotation;

        // Define la rotación objetivo (sin inclinación, 0 grados en X y Z)
        Quaternion targetRotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);

        // Interpola suavemente hacia la rotación objetivo
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime / tiltCorrectionTime);
    }

#endregion

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