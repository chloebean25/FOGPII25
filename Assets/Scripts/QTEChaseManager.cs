using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class QuickTimeEventManager : MonoBehaviour
{
    public float qteDuration = 2f;
    public float edgePadding = 100f;

    public GameObject qteUIPanel;
    public TMP_Text qteText;
    public GameObject failScreen;

    public Animator[] slowAnimators;
    public float slowMotionScale = 0.3f;

    private bool isFailed = false;

    public void TriggerQTE(char expectedLetter)
    {
        if (!isFailed)
            StartCoroutine(RunQTE(char.ToUpper(expectedLetter)));
    }

    public void TriggerQTEFromAnimation(string letter)
    {
        if (!string.IsNullOrEmpty(letter))
            TriggerQTE(char.ToUpper(letter[0]));
    }

    private IEnumerator RunQTE(char expectedLetter)
    {
        Time.timeScale = slowMotionScale;

        if (qteUIPanel != null)
            qteUIPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        if (qteText != null)
        {
            qteText.text = expectedLetter.ToString();

            RectTransform rect = qteText.transform.parent.GetComponent<RectTransform>();
            RectTransform canvasRect = qteUIPanel.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

            Vector2 panelSize = rect.rect.size;

            
            float halfWidth = (canvasRect.rect.width * 0.5f) - edgePadding - (panelSize.x * 0.5f);
            float halfHeight = (canvasRect.rect.height * 0.5f) - edgePadding - (panelSize.y * 0.5f);

            Vector2 randomPos = new Vector2(
                Random.Range(-halfWidth, halfWidth),
                Random.Range(-halfHeight, halfHeight)
            );

            rect.anchoredPosition = randomPos;
        }

        float timer = 0f;
        bool success = false;

        KeyCode expectedKey =
            (KeyCode)System.Enum.Parse(typeof(KeyCode), expectedLetter.ToString());

        while (timer < qteDuration)
        {
            if (Input.GetKeyDown(expectedKey))
            {
                success = true;
                break;
            }

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f;

        if (qteUIPanel != null)
            qteUIPanel.SetActive(false);

        if (!success)
        {
            isFailed = true;
            ShowFailScreen();
        }
    }

    private void ShowFailScreen()
    {
        if (failScreen != null)
            failScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        if (failScreen != null)
            failScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (var anim in slowAnimators)
        {
            anim.speed = 1f;
            anim.Rebind();
            anim.Update(0f);
            anim.Play(anim.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0f);
        }

        Time.timeScale = 1f;
        isFailed = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}