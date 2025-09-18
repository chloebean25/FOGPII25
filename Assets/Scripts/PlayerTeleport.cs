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
            StartCoroutine("WaitForSec");
            if(Input.GetKeyDown(KeyCode.F)){
                player.transform.position = teleportSpot.position;

            }
        }
    }
    IEnumerator WaitForSec(){
        yield return new WaitForSeconds(5);
        displayText.SetActive(false);
    }
}
