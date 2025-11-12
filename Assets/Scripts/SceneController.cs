using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
   [SerializeField] 
   private float fadeDuration;

   private SceneFade sceneFade;

   private void Awake()
   {
       sceneFade = GetComponentInChildren<SceneFade>();
   }
   private IEnumerator Start()
   {
       yield return sceneFade.FadeInCoroutine(fadeDuration);
   }
   public void LoadScene(string sceneName)
   {
       StartCoroutine(LoadSceneCoroutine(sceneName));
   }
   private IEnumerator LoadSceneCoroutine(string sceneName)
   {
       yield return sceneFade.FadeOutCoroutine(fadeDuration);
       yield return SceneManager.LoadSceneAsync(sceneName);
   }
   public void CutToBlack(){
    if (sceneFade == null) return;

    var color = sceneFade.GetComponent<UnityEngine.UI.Image>().color;
    color.a = 1f;
    sceneFade.GetComponent<UnityEngine.UI.Image>().color = color;
    sceneFade.gameObject.SetActive(true);
   }
}
