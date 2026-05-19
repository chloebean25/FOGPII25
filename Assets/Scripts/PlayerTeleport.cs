using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform player;
    public Transform teleportSpot;
    public GameObject displayText;

    private CharacterController cc;
    private bool playerInRange = false;
    private bool canUse = true;

    void Start()
    {
        displayText.SetActive(false);
        cc = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (playerInRange && canUse && Input.GetKeyDown(KeyCode.F))
        {
            Teleport();
        }
    }

    void Teleport()
    {
        canUse = false;

        displayText.SetActive(false);

        if (cc != null)
            cc.enabled = false;

        player.position = teleportSpot.position;

        if (cc != null)
            cc.enabled = true;

        playerInRange = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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

            canUse = true;
        }
    }
}