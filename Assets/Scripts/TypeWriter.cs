using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueTyper : MonoBehaviour
{
    [Header("References")]
    public TMP_Text textComponent;
    [TextArea(2, 6)]
    public string[] dialogueLines;

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public KeyCode skipKey = KeyCode.Space;
    public float autoAdvanceDelay = 1.0f; 
    public bool playOnStart = true;

    private int currentLine = 0;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (textComponent == null)
            textComponent = GetComponent<TMP_Text>();

        if (playOnStart && dialogueLines.Length > 0)
            StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        while (currentLine < dialogueLines.Length)
        {
            yield return StartCoroutine(TypeLine(dialogueLines[currentLine]));

            
            yield return new WaitForSeconds(autoAdvanceDelay);

            currentLine++;
        }

        Debug.Log("Dialogue finished!");
    }

    IEnumerator TypeLine(string line)
    {
        textComponent.text = "";

        foreach (char c in line)
        {
            textComponent.text += c;

            
            if (Input.GetKeyDown(skipKey))
            {
                textComponent.text = line;
                break;
            }

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
