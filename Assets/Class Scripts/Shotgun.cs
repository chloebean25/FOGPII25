using UnityEngine;

public class Shotgun : Weapon
{
    public override void Use()
    {
        Debug.Log("Shot Buck shot?");
    }
    public void Equiped()
    {
        Debug.Log("Equiped: " + gameObject.name);
    }
}
