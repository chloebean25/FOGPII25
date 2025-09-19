using UnityEngine;

public class Blocker : MonoBehaviour
{
    public GameObject sweetBlockPlayer;
    public GameObject blocker;
    public GameObject giveText;
    public GameObject trigger;

    void Start(){
        giveText.SetActive(false);
    }
    
    private void OnTriggerStay(Collider other){
        if(other.gameObject.CompareTag("Player")&&sweetBlockPlayer.activeSelf){
            giveText.SetActive(true);
            if(Input.GetKey(KeyCode.F)){
                Destroy(blocker);
                Destroy(trigger);
                sweetBlockPlayer.SetActive(false);
                Destroy(giveText);
            }
        }
    }
  private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            giveText.SetActive(false);
        }
    }
}
