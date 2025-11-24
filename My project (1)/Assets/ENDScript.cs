using UnityEngine;
using TMPro;
using System.Collections;

public class MessageTrigger : MonoBehaviour
{
    public TMP_Text textObject;
    public float fadeDuration = 1.5f;
    public float holdTime = 1f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(ShowAndFade());
        }
    }

    IEnumerator ShowAndFade()
    {
        textObject.gameObject.SetActive(true);

        // Reset alpha to 1
        Color c = textObject.color;
        c.a = 1f;
        textObject.color = c;

        // Hold visible
        yield return new WaitForSeconds(holdTime);

        // Fade out
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);

            Color newColor = textObject.color;
            newColor.a = alpha;
            textObject.color = newColor;

            yield return null;
        }

        // Disable after fade
        textObject.gameObject.SetActive(false);
    }
}