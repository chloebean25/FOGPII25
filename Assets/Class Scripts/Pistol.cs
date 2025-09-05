using UnityEngine;

public class Pistol : Weapon
{
    public override void Use()
    {
        Debug.Log("Player shot 9mm!!");
    }
    public void Equiped()
    {
        Debug.Log("Equiped: " + gameObject.name);
    }
}
