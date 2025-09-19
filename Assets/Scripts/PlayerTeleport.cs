using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTeleport : MonoBehaviour
{
    public GameObject player;
    public Transform teleportSpot;
    public GameObject displayText;
     void Start(){
        displayText.SetActive(false);
    }
    
    void OnTriggerStay(Collider player){
        if(player.gameObject.tag=="Player"){
            displayText.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F)){
                player.transform.position = teleportSpot.position;

            }
        }
    }
    void OnTriggerExit(Collider player){
        displayText.SetActive(false);
    }
}
