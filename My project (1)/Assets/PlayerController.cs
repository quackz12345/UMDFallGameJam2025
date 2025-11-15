using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float sideSpeed = 8f;
    public float upDownSpeed = 6f;
    public float boostMultiplier = 1.5f;
    public float tiltMax = 45f;
    public float tiltSpeed = 5f;

    private float currentTilt = 0f;

    void Update()
    {
        // Always move forward
        float boost = Input.GetKey(KeyCode.Space) ? boostMultiplier : 1f;
        transform.position += transform.forward * forwardSpeed * boost * Time.deltaTime;

        // Left / Right movement
        float horizontal = Input.GetAxis("Horizontal");

        // Move sideways
        transform.position += transform.right * horizontal * sideSpeed * Time.deltaTime;

        // Tilt the camera/player
        currentTilt = Mathf.Lerp(currentTilt, horizontal * tiltMax, Time.deltaTime * tiltSpeed);
        transform.localRotation = Quaternion.Euler(0, currentTilt, 0);
    }
}
