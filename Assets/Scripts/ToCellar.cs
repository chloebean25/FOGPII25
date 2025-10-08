using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCellar : MonoBehaviour
{
    public GameObject pincersPlayer;
    public GameObject breakText;

    void Start(){
         breakText.SetActive(false);
    }
    private void OnTriggerStay(Collider other){
       
        if(other.gameObject.CompareTag("Player")&& pincersPlayer.activeSelf){
            breakText.SetActive(true);
            if(Input.GetKey(KeyCode.F)){
                SceneManager.LoadScene("Cellar");
            }
            
        }
    }
}