using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector introTimeline;
    public PlayerMovement playerMovement;
    public Camera playerCamera;
    public GameObject cutsceneModel; 

    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        
        playerMovement.LockInput(true);
        playerCamera.gameObject.SetActive(false);

    
        introTimeline.Play();

        
        yield return new WaitForSeconds((float)introTimeline.duration);

        
        cutsceneModel.SetActive(false);

        
        playerCamera.gameObject.SetActive(true);
        playerMovement.LockInput(false);
    }
}
