using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 15f;
    public float sideSpeed = 30f;
    public float verticalSpeed = 4f;
    public float boostMultiplier = 1.8f;

    [Header("Camera Tilt Settings")]
    public float maxRoll = 45f;     // camera roll
    public float maxPitch = 20f;    // camera pitch
    public float tiltSpeed = 4f;

    [Header("Virtual FPS Settings")]
    public float virtualFPS = 100f; // virtual fixed FPS

    [Header("References")]
    public Transform cameraTransform;

    // --- Internal state ---
    private float currentRoll = 0f;
    private float currentPitch = 0f;

    void Update()
    {
        // --- INPUT ---
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float boost = Input.GetKey(KeyCode.Space) ? boostMultiplier : 1f;

        // --- VIRTUAL FIXED FPS MOVEMENT ---
        float fixedDelta = 1f / virtualFPS;

        transform.position += transform.forward * forwardSpeed * boost * fixedDelta;
        transform.position += transform.right * horizontal * sideSpeed * fixedDelta * (boost/2f);
        transform.position += transform.up * vertical * verticalSpeed * fixedDelta * (boost/2f);

        // --- CAMERA VISUAL TILT (smooth) ---
        currentRoll = Mathf.Lerp(currentRoll, -horizontal * maxRoll, fixedDelta * tiltSpeed);
        currentPitch = Mathf.Lerp(currentPitch, -vertical * maxPitch, fixedDelta * (tiltSpeed*0.2f));

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(currentPitch, 0f, currentRoll);
        }
    }
}
