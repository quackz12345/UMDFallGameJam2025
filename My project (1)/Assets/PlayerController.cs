using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float sideSpeed = 8f;
    public float verticalSpeed = 6f;
    public float boostMultiplier = 1.5f;

    public float maxRoll = 45f;     // camera roll
    public float maxPitch = 25f;    // camera pitch
    public float tiltSpeed = 5f;

    private float currentRoll = 0f;
    private float currentPitch = 0f;
    private bool isHit = false;
    public Transform cameraTransform;  // assign your camera here
    public Rigidbody rb;
    void Update()
    {
        if (!isHit)
        {
            float boost = Input.GetKey(KeyCode.Space) ? boostMultiplier : 1f;

            // --- PLAYER MOVEMENT ---
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Move the player
            transform.position += transform.forward * forwardSpeed * boost * Time.deltaTime;
            transform.position += transform.right * horizontal * sideSpeed * Time.deltaTime * (boost * .4f);
            transform.position += transform.up * vertical * verticalSpeed * Time.deltaTime * (boost * .4f);

            // --- CAMERA VISUAL TILT ---
            currentRoll = Mathf.Lerp(currentRoll, -horizontal * maxRoll, Time.deltaTime * tiltSpeed);
            currentPitch = Mathf.Lerp(currentPitch, -vertical * maxPitch, Time.deltaTime * (tiltSpeed * .5f));

            if (cameraTransform != null)
            {
                cameraTransform.localRotation = Quaternion.Euler(currentPitch, 0f, currentRoll);
            }
        }
        // --- BOOST ---
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Finish"))
        {
            isHit = true;
            rb.useGravity = true;
            Debug.Log("Player collided with: " + collision.gameObject.name);
        }

    }
    
}

