using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class movement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("UI")]
    public TextMeshProUGUI speedText;

    [Header("Engine")]
    public float maxEngineForce = 1300f;
    public float throttleIncreaseSpeed = 0.5f;
    public float throttleDecreaseSpeed = 0.3f;
    private float throttle = 0f;

    [Header("Speed Limits")]
    public float maxSpeed = 40f;

    [Header("Takeoff")]
    public float takeOffSpeed = 20f;
    public float liftPower = 1f;

    [Header("Controls")]
    public float pitchPower = 200f;
    public float rollPower = 250f;
    public float yawPower = 150f;
    public float groundTurnPower = 20f;

    public float groundCheckDistance = 0.8f;

    public bool isGrounded;
    private float forwardSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateSpeedUI();
    }
    void FixedUpdate()
    {
        CheckGround();
        HandleThrottle();
        ApplyEngineForce();
        HandleLift();
        HandleRotation();
    }

    void UpdateSpeedUI()
    {
        forwardSpeed = Vector3.Dot(rb.velocity, transform.forward);

        float displaySpeed = Mathf.Max(0, forwardSpeed);

        if (speedText != null)
            speedText.text = "SPEED: " + displaySpeed.ToString("0");
    }
    void OnCollisionEnter(Collision collision)
    {
        // Reduce bounce impulse
        rb.velocity *= 0.8f;
        rb.angularVelocity *= 0.5f;
    }
    void HandleThrottle()
    {
        if (Input.GetKey(KeyCode.W))
        {
            throttle += throttleIncreaseSpeed * Time.fixedDeltaTime;
        }
        else
        {
            // Automatically reduce throttle when not pressing W
            throttle -= throttleDecreaseSpeed * Time.fixedDeltaTime;
        }

        // Extra reduction if pressing S
        if (Input.GetKey(KeyCode.S))
            throttle -= throttleDecreaseSpeed * 2f * Time.fixedDeltaTime;

        throttle = Mathf.Clamp01(throttle);
    }

    void ApplyEngineForce()
    {
        float forwardVelocity = Vector3.Dot(rb.velocity, transform.forward);

        if (forwardVelocity < maxSpeed)
            rb.AddForce(transform.forward * maxEngineForce * throttle);

        // Extra ground braking when S is pressed
        if (isGrounded && Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * maxEngineForce * 0.2f);
        }
    }

    void HandleLift()
    {
        forwardSpeed = Vector3.Dot(rb.velocity, transform.forward);

        // Only allow lift if above takeoff speed
        if (forwardSpeed > takeOffSpeed)
        {
            // Only apply strong lift when player presses pitch up
            if (Input.GetKey(KeyCode.Keypad2))
            {
                float lift = forwardSpeed * forwardSpeed * liftPower;
                rb.AddForce(transform.up * lift);
            }
        }
    }



    void HandleRotation()
    {
        // Pitch
        if (Input.GetKey(KeyCode.Keypad8))
            rb.AddTorque(transform.right * pitchPower);

        if (Input.GetKey(KeyCode.Keypad2))
            rb.AddTorque(-transform.right * pitchPower);

        // Roll
        if (Input.GetKey(KeyCode.Keypad4))
            rb.AddTorque(transform.forward * rollPower);

        if (Input.GetKey(KeyCode.Keypad6))
            rb.AddTorque(-transform.forward * rollPower);

        // Yaw
        float yawInput = 0f;

        if (Input.GetKey(KeyCode.A))
            yawInput = -1f;

        if (Input.GetKey(KeyCode.D))
            yawInput = 1f;

        if (yawInput != 0f)
        {
            if (isGrounded)
            {
                float speed = rb.velocity.magnitude;

                if (speed > 0.5f) // small movement required
                {
                    float turnAmount = yawInput * groundTurnPower * Time.fixedDeltaTime;
                    transform.Rotate(0f, turnAmount, 0f);
                }
            }
            else
            {
                rb.AddTorque(transform.up * yawInput * yawPower, ForceMode.Force);
            }
        }
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance);
    }
}