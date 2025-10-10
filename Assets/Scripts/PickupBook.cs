using UnityEngine;

public class PickupBook : MonoBehaviour
{
    public GameObject pickupText;
    public GameObject image;
     
    void Start()
    {
        pickupText.SetActive(false);
        image.SetActive(false);
        
    }

    private void OnTriggerStay(Collider other){
        pickupText.SetActive(true);
        if(other.gameObject.CompareTag("Player")){
            
            if(Input.GetKey(KeyCode.F)){

                pickupText.SetActive(false);  
                image.SetActive(true);
            }
        }
    }
    void OnTriggerExit(Collider player){
        
        pickupText.SetActive(false); 
        image.SetActive(false);  

    }
}

