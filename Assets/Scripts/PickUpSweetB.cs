using UnityEngine;

public class PickUpSweetB : MonoBehaviour
{
    public GameObject sweetBlock;
    public GameObject pickupText;
     
    void Start()
    {
        sweetBlock.SetActive(false);
        pickupText.SetActive(false);
        
    }

    private void OnTriggerStay(Collider other){
        pickupText.SetActive(true);
        if(other.gameObject.CompareTag("Player")){
            
            if(Input.GetKey(KeyCode.F)){
                this.gameObject.SetActive(false);
                pickupText.SetActive(false);  
                sweetBlock.SetActive(true);
            }
        }
    }
    void OnTriggerExit(Collider player){
        
        pickupText.SetActive(false);   

    }
}
