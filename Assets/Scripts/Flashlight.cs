using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    private bool FlashlightActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FlashlightLight.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            FlashlightActive = !FlashlightActive;
            FlashlightLight.gameObject.SetActive(FlashlightActive);
            
        }
    }
}
