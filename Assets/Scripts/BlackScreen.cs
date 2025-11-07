using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject blackScreen;      
    public string nextSceneName = "Farm";
    public float delayBeforeLoad = 1f;

    private bool hasTriggered = false;

    private void OnTriggerStay(Collider other)
    {
        if (hasTriggered) return;

        
        if (other.name == "Truck" || other.CompareTag("Truck"))
        {
            Debug.Log("Truck triggered scene load!");
            hasTriggered = true;
            blackScreen.SetActive(true);
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private System.Collections.IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(nextSceneName);
    }
}
