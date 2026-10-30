using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    public GameObject displayText;
    public GameObject responseText;

    private bool playerInRange = false;
    private bool hasKnocked = false;

    void Start()
    {
        displayText.SetActive(false);
        responseText.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !hasKnocked && Input.GetKeyDown(KeyCode.F))
        {
            displayText.SetActive(false);  
            responseText.SetActive(true);  
            hasKnocked = true;

            Debug.Log("Player knocked.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasKnocked)
        {
            playerInRange = true;
            displayText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            displayText.SetActive(false);
            responseText.SetActive(false);
        }
    }
}