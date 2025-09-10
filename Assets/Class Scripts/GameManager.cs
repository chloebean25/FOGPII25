using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance{ get; private set; }
    public bool isPlaying = true;

    public CPlayer player;
    //public List<Tank> enemies;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //optional
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //awake
            Destroy(this);
            return;
        }
    }
}
