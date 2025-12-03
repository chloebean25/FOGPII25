using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text textComponent;
    public float typingSpeed = 50.0f;
    public KeyCode skipKey = KeyCode.Space;
    public float autoAdvanceDelay = 1.0f;
    public float displayDurationAfterLine = 2f;

    private Queue<string> dialogueQueue = new Queue<string>();
    private bool isTyping = false;

    void Start()
    {
        if (textComponent == null)
            textComponent = GetComponent<TMP_Text>();

        textComponent.text = "";
        textComponent.gameObject.SetActive(false);
    }

    public void EnqueueDialogue(string line)
    {
        dialogueQueue.Enqueue(line);

        if (!isTyping)
            StartCoroutine(ProcessDialogueQueue());
    }

    private IEnumerator ProcessDialogueQueue()
    {
        while (dialogueQueue.Count > 0)
        {
            isTyping = true;
            textComponent.gameObject.SetActive(true);

            string line = dialogueQueue.Dequeue();
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(autoAdvanceDelay);
            yield return new WaitForSeconds(displayDurationAfterLine);

            textComponent.gameObject.SetActive(false);
        }

        isTyping = false;
    }

    private IEnumerator TypeLine(string line)
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
