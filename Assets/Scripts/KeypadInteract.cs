using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeypadInteract : MonoBehaviour
{
    private bool isPlayerNear = false;
    public GameObject interactText;

    void Start(){
        interactText.SetActive(false);
    }
    

    void Update(){
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F)){
            KeyPadManager.Instance.OpenKeypad();
            
        }
    }

    private void OnTriggerStay(Collider other){
        if(other.CompareTag("Player")){
            isPlayerNear = true;
            Debug.Log(">>> Player entered keypad zone");
        }
    
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            interactText.SetActive(true);
            StartCoroutine("WaitForSeconds");
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            interactText.SetActive(false);
            isPlayerNear = false;
            Debug.Log(">>> Player left keypad zone");
        }
    }
    IEnumerator WaitForSeconds(){
            yield return new WaitForSeconds(0.5f);
            interactText.SetActive(false);
        }
}
