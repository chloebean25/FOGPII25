using UnityEngine;

public class PickupBook : MonoBehaviour
{
    public GameObject pickupText;
    public GameObject image;
    public GameObject text;
    public PlayerMovement playerMovement;

    private bool isReading = false;
    private bool playerInRange = false;

    void Start()
    {
        pickupText.SetActive(false);
        image.SetActive(false);
        text.SetActive(false);
    }

    void Update()
    {
        // Open book
        if (playerInRange && !isReading && Input.GetKeyDown(KeyCode.F))
        {
            isReading = true;

            pickupText.SetActive(false);
            image.SetActive(true);
            text.SetActive(true);

            if (playerMovement != null)
                playerMovement.LockInput(true);

            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }

        // Close book
        if (isReading && Input.GetKeyDown(KeyCode.Escape))
        {
            isReading = false;

            image.SetActive(false);
            text.SetActive(false);

            if (playerMovement != null)
                playerMovement.LockInput(false);

            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;

            
            if (playerInRange)
                pickupText.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;

        if (!isReading)
            pickupText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;
        pickupText.SetActive(false);

        
        if (!isReading)
        {
            image.SetActive(false);
            text.SetActive(false);
        }
    }
}