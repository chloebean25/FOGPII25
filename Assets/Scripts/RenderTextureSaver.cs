using UnityEngine;
using System.IO;
using System.Collections;

public class RenderTextureSaver : MonoBehaviour
{
    public int width = 3840;
    public int height = 1240;

    void Start()
    {
        StartCoroutine(CaptureHero());
    }

    IEnumerator CaptureHero()
    {
        // 1. Force real render resolution
        Screen.SetResolution(width, height, false);

        // 2. Wait for Unity to actually apply it
        yield return new WaitForSeconds(0.5f);

        // 3. Wait until frame is fully rendered
        yield return new WaitForEndOfFrame();

        // 4. Capture high-res screenshot
        ScreenCapture.CaptureScreenshot("steam_hero.png", 1);

        Debug.Log("Steam hero captured at " + width + "x" + height);
    }
}