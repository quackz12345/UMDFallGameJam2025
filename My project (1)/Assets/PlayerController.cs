using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float sideSpeedMultiplier = 2.2f;
    public float verticalSpeed = 6f;
    public float boostMultiplier = 1.5f;

    // sideSpeed automatically tracks forwardSpeed
    public float sideSpeed => forwardSpeed * sideSpeedMultiplier;

    [Header("Camera Tilt")]
    public float maxRoll = 45f;
    public float maxPitch = 25f;
    public float tiltSpeed = 5f;

    private float currentRoll = 0f;
    private float currentPitch = 0f;
    private bool isHit = false;

    [Header("References")]
    public TimerText timer;
    public Transform cameraTransform;
    public Rigidbody rb;
    public AudioClip hitSound;
    private AudioSource audioSource;

    [Header("Intro Drop")]
    public float introDropAmount = 42f;
    public float introDuration = 2f;
    private bool canControl = false;

    void Start()
    {
        StartCoroutine(IntroDrop());
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator IntroDrop()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos - new Vector3(0, introDropAmount, 0);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / introDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        canControl = true;
        timer.StartTimer();
    }

    void Update()
    {
        if (!canControl || isHit)
            return;

        float boost = Input.GetKey(KeyCode.Space) ? boostMultiplier : 1f;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Movement
        transform.position += transform.forward * forwardSpeed * boost * Time.deltaTime;
        transform.position += transform.right * horizontal * sideSpeed * Time.deltaTime * (boost * 0.4f);
        transform.position += transform.up * vertical * verticalSpeed * Time.deltaTime * (boost * 0.4f);

        // Camera tilt
        currentRoll = Mathf.Lerp(currentRoll, -horizontal * maxRoll, Time.deltaTime * tiltSpeed);
        currentPitch = Mathf.Lerp(currentPitch, -vertical * maxPitch, Time.deltaTime * tiltSpeed * 0.5f);

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(currentPitch, 0f, currentRoll);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Finish"))
        {
            isHit = true;
            rb.useGravity = true;
            Debug.Log("Player collided with: " + collision.gameObject.name);
            audioSource.PlayOneShot(hitSound);
        }
    }
}
