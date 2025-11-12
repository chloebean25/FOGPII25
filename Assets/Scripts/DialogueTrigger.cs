using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(1, 4)]
    public string dialogueLine;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            DialogueManager manager = FindObjectOfType<DialogueManager>();
            if (manager != null)
            {
                manager.EnqueueDialogue(dialogueLine);
            }
        }
    }
}
