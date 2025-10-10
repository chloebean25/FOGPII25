using UnityEngine;

public class CellarKeypadInteract : MonoBehaviour
{
    public GameObject interactText;
    private bool isPlayerNear = false;

    private void Start()
    {
        interactText.SetActive(false);
    }

    private void Update()
    {
        
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            if (CellarKeypad.Instance != null && !CellarKeypad.Instance.isOpen)
                CellarKeypad.Instance.OpenKeypad();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactText.SetActive(false);
        }
    }
}
