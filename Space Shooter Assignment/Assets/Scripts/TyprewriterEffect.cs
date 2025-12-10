using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.05f;

    private TextMeshProUGUI textMesh;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void TypeText(string fullText)
    {
        // Stop any currently-running typewriter animation
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeTextRoutine(fullText));
    }

    private IEnumerator TypeTextRoutine(string fullText)
    {
        textMesh.text = "";
        foreach (char letter in fullText)
        {
            textMesh.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
