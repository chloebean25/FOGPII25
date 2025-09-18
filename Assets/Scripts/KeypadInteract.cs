using UnityEngine;

public class KeypadInteract : MonoBehaviour
{
    private bool isPlayerNear = false;

    void Update(){
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F)){
            KeyPadManager.Instance.OpenKeypad();
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            isPlayerNear = true;
            Debug.Log(">>> Player entered keypad zone");
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            isPlayerNear = false;
            Debug.Log(">>> Player left keypad zone");
        }
    }
}
