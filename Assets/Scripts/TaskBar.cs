using UnityEngine;

public class TaskBar : MonoBehaviour
{
    public GameObject task1;
    public GameObject task2;    
    public GameObject task3;
    public GameObject task4;
    public GameObject task5;
    public GameObject task6;
    public GameObject task7;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        task1.SetActive(false);
        task2.SetActive(false);
        task3.SetActive(false);
        task4.SetActive(false);
        task5.SetActive(false);
        task6.SetActive(false);
        task7.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
