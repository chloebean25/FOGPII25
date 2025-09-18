using UnityEngine;

public class PickUpSweetB : MonoBehaviour
{
    public GameObject sweetBlock;
     
    void Start()
    {
        sweetBlock.SetActive(false);
        
    }

    private void OnTriggerStay(Collider other){
        if(other.gameObject.CompareTag("Player")){
            if(Input.GetKey(KeyCode.F)){
                this.gameObject.SetActive(false);
                
                sweetBlock.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            sweetBlock.SetActive(true);
            
        }        
    }
}
