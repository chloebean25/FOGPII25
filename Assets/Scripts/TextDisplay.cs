using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextDisplay : MonoBehaviour
{
    public GameObject displayText;
    public GameObject responseText;
    
    void Start(){
        displayText.SetActive(false);
        responseText.SetActive(false);
    }
    void OnTriggerStay(Collider player){
        if(player.gameObject.tag=="Player"){
            displayText.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F)){
                responseText.SetActive(true);
                Debug.Log("You pressed F");
            }
        }
    }

    void OnTriggerExit(Collider player){
        displayText.SetActive(false);
        responseText.SetActive(false);
    }
}
