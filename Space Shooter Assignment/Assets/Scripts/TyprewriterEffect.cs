using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class TyprewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.05f;       // Speed of typing
    public string fullText;                 // The full text to display
    private string currentText = "";        // The text currently displayed
    private TextMeshProUGUI textMeshPro;    // Reference to the text object

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            currentText += letter;                              // Add one letter at a time
            textMeshPro.text = currentText;                     // Update the text displayed
            yield return new WaitForSeconds(typingSpeed);       // Wait before typing the next letter
        }
    }
}
