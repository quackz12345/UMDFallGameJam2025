
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 30f;
    public float sideSpeed = 30f;
    public float verticalSpeed = 4f;
    public float boostMultiplier = 1.8f;

    [Header("Camera Tilt Settings")]
    public float maxRoll = 45f;     // camera roll
    public float maxPitch = 20f;    // camera pitch
    public float tiltSpeed = 4f;

    [Header("Virtual FPS Settings")]
    public float virtualFPS = 100f; // movement simulation FPS

    [Header("References")]
    public Transform cameraTransform;

    // --- Internal state ---
    private float currentRoll = 0f;
    private float currentPitch = 0f;
    private float accumulator = 0f; // tracks real time passed

    void Update()
    {
        // accumulate real delta time
        accumulator += Time.deltaTime;
        float fixedDelta = 1f / virtualFPS;

        // simulate movement in fixed virtual steps
        while (accumulator >= fixedDelta)
        {
            MovePlayer(fixedDelta);
            accumulator -= fixedDelta;
        }

        // smooth camera tilt on real frame
        UpdateCameraTilt();
    }

    void MovePlayer(float dt)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float boost = Input.GetKey(KeyCode.Space) ? boostMultiplier : 1f;

        // forward with boost
        transform.position += transform.forward * forwardSpeed * boost /40;

        // strafing and vertical movement (no boost)
        transform.position += transform.right * horizontal * sideSpeed /40;
        transform.position += transform.up * vertical * verticalSpeed /40;
    }

    void UpdateCameraTilt()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float rollTarget = -horizontal * maxRoll;
        float pitchTarget = -vertical * maxPitch;

        currentRoll = Mathf.MoveTowards(currentRoll, rollTarget, tiltSpeed /40);
        currentPitch = Mathf.MoveTowards(currentPitch, pitchTarget, tiltSpeed /40);
        
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(currentPitch, 0f, currentRoll);
        }
    }
}