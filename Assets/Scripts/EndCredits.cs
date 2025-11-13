using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class ExtraMessage
{
    public TMP_Text messageText;
    public RawImage messageImage;
}

public class EndCredits : MonoBehaviour
{
    public TMP_Text thankYouText;
    public RectTransform creditsText;
    public ExtraMessage[] extraMessages;
    public string mainMenuSceneName = "MenuScreen";

    public float fadeDuration = 2f;
    public float thankYouHoldTime = 2f;
    public float scrollSpeed = 50f;
    public float extraMessageDelay = 1f;
    public float extraMessageHoldTime = 2f;
    public float postScrollDelay = 0f;

    private Vector2 creditsStartPos;

    void Start()
    {
        creditsStartPos = creditsText.anchoredPosition;

        if (thankYouText != null) thankYouText.alpha = 0;

        foreach (ExtraMessage em in extraMessages)
        {
            if (em.messageText != null) em.messageText.alpha = 0;
            if (em.messageImage != null)
            {
                Color c = em.messageImage.color;
                c.a = 0;
                em.messageImage.color = c;
            }
        }

        StartCoroutine(RunEndSequence());
    }

    private IEnumerator FadeText(TMP_Text text, float startAlpha, float endAlpha, float duration)
    {
        float t = 0f;
        text.alpha = startAlpha;
        while (t < duration)
        {
            text.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        text.alpha = endAlpha;
    }

    private IEnumerator FadeRawImage(RawImage img, float startAlpha, float endAlpha, float duration)
    {
        float t = 0f;
        Color color = img.color;
        color.a = startAlpha;
        img.color = color;
        while (t < duration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            img.color = color;
            t += Time.deltaTime;
            yield return null;
        }
        color.a = endAlpha;
        img.color = color;
    }

    private IEnumerator RunEndSequence()
    {
        if (thankYouText != null)
        {
            yield return FadeText(thankYouText, 0, 1, fadeDuration);
            yield return new WaitForSeconds(thankYouHoldTime);
            yield return FadeText(thankYouText, 1, 0, fadeDuration);
        }

        float screenHeight = ((RectTransform)creditsText.parent).rect.height;
        float endY = creditsStartPos.y + creditsText.rect.height + screenHeight;

        while (creditsText.anchoredPosition.y < endY)
        {
            creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            yield return null;
        }

        creditsText.anchoredPosition = new Vector2(creditsText.anchoredPosition.x, endY);

        if (postScrollDelay > 0f)
            yield return new WaitForSeconds(postScrollDelay);

        foreach (ExtraMessage em in extraMessages)
        {
            yield return new WaitForSeconds(extraMessageDelay);
            if (em.messageText != null) yield return FadeText(em.messageText, 0, 1, fadeDuration);
            if (em.messageImage != null) yield return FadeRawImage(em.messageImage, 0, 1, fadeDuration);
            yield return new WaitForSeconds(extraMessageHoldTime);
            if (em.messageText != null) yield return FadeText(em.messageText, 1, 0, fadeDuration);
            if (em.messageImage != null) yield return FadeRawImage(em.messageImage, 1, 0, fadeDuration);
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
