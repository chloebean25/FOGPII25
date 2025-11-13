using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuickTimeEventManager : MonoBehaviour
{
    [Header("QTE Settings")]
    
    public float qteDuration = 2f;
    public float introDelay = 2f;
    public float qteTotalDuration = 10f; 

    
    public float slowMotionScale = 0.3f;
    public float edgePadding = 100f;

    
    public float timeBetweenEvents = 2f;

    [Header("UI References")]
    public GameObject qteUIPanel;     
    public TMP_Text qteText;          
    public GameObject failScreen;    
    public PlayerMovement playerMovement;  

    [Header("References")]
    public Animator chaseCameraAnimator;
    public Animator cowAnimator;

    private bool isFailed = false;

    private void Start()
    {
        
        if (qteUIPanel != null)
            qteUIPanel.SetActive(false);
        if (failScreen != null)
            failScreen.SetActive(false);

        
        StartCoroutine(QTEChaseSequence());
    }

    private IEnumerator QTEChaseSequence()
{
    // Wait for intro animation
    yield return new WaitForSeconds(introDelay);

    float elapsedTime = 0f;

    while (!isFailed && elapsedTime < qteTotalDuration)
    {
        yield return StartCoroutine(TriggerQTE());

        if (isFailed)
            break;

        
        yield return new WaitForSeconds(timeBetweenEvents);

        
        elapsedTime += qteDuration + timeBetweenEvents;
    }

   
    EndQTESequence();
}


    private IEnumerator TriggerQTE()
    {
        
        Time.timeScale = slowMotionScale;

    
        int randomIndex = Random.Range(0, 26);
        char randomLetter = (char)('A' + randomIndex);
        qteText.text = randomLetter.ToString();
        // Move text to a random screen position
        RectTransform rect = qteText.GetComponent<RectTransform>();
        if (rect != null)
        {
            
            Canvas canvas = qteText.canvas;
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            float halfWidth = canvasRect.rect.width / 2f - edgePadding;
            float halfHeight = canvasRect.rect.height / 2f - edgePadding;

            float randomX = Random.Range(-halfWidth, halfWidth);
            float randomY = Random.Range(-halfHeight, halfHeight);

            rect.anchoredPosition = new Vector2(randomX, randomY);
        }


    
        KeyCode expectedKey = (KeyCode)((int)KeyCode.A + randomIndex);

        
        qteUIPanel.SetActive(true);

    
        CanvasGroup cg = qteUIPanel.GetComponent<CanvasGroup>();
        if (cg != null)
            yield return StartCoroutine(FadeCanvasGroup(cg, 0, 1, 0.2f));

        Debug.Log($"QTE Prompt: {randomLetter}");

        float timer = 0f;
        bool success = false;

        while (timer < qteDuration)
        {
        
            if (Input.GetKeyDown(expectedKey))
            {
                Debug.Log("QTE Success!");
                success = true;
                break;
            }

            timer += Time.unscaledDeltaTime; 
            yield return null;
        }

        if (success)
        {
            
            if (cg != null)
                yield return StartCoroutine(FadeCanvasGroup(cg, 1, 0, 0.2f));

            qteUIPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("QTE Failed!");
            isFailed = true;
            Time.timeScale = 1f;
            ShowFailScreen();
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float time = 0f;
        cg.alpha = start;
        while (time < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, time / duration);
            time += Time.unscaledDeltaTime; 
            yield return null;
        }
        cg.alpha = end;
    }
    private void EndQTESequence()
    {
    Debug.Log("QTE Sequence Finished!");

    
    if (qteUIPanel != null)
        qteUIPanel.SetActive(false);

    
    Time.timeScale = 1f;

    
    if (chaseCameraAnimator != null && cowAnimator != null)
        chaseCameraAnimator.enabled = true;
        cowAnimator.enabled = true;

   
    }


   private void ShowFailScreen()
    {
        
        Time.timeScale = 0f; 

        failScreen.SetActive(true);
        if (playerMovement != null)
            playerMovement.LockInput(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (chaseCameraAnimator != null && cowAnimator != null)
        {
            cowAnimator.enabled = false;
            chaseCameraAnimator.enabled = false;
            chaseCameraAnimator.speed = 0f;
            cowAnimator.speed = 0f;
        }

        Debug.Log("Fail Screen Activated — Animation Paused");
    }




  public void Retry()
    {
        failScreen.SetActive(false);
        if (playerMovement != null)
            playerMovement.LockInput(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        if (chaseCameraAnimator != null&& cowAnimator != null)
        {
            chaseCameraAnimator.enabled = true;
            cowAnimator.enabled = true;
            cowAnimator.speed = 1f;
            chaseCameraAnimator.speed = 1f; 
            chaseCameraAnimator.Play("ChaseCamera", 0, 0f); 
            cowAnimator.Play("CowChase", 0, 0f);
        }

        isFailed = false;
        StopAllCoroutines();
        StartCoroutine(QTEChaseSequence());
    }
}

