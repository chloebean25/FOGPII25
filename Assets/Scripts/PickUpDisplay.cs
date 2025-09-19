using UnityEngine;

public class PickUpDisplay : MonoBehaviour
{
   public GameObject pickupText;

   void Start()
    {
        pickupText.SetActive(false);
        
    }
    void OnTriggerEnter(Collider player){
        pickupText.SetActive(true);
     }
    void OnTriggerExit(Collider player){
        
        pickupText.SetActive(false);   

    }
}
