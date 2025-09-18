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
            StartCoroutine("WaitForSec");
            if(Input.GetKeyDown(KeyCode.F)){
                responseText.SetActive(true);
                Debug.Log("You pressed F");
            }
        }
    }

    IEnumerator WaitForSec(){
        yield return new WaitForSeconds(5);
        displayText.SetActive(false);
        responseText.SetActive(false);
    }
}
