using UnityEngine;

public class CrashAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip crashClip;

    
    public void PlayCrashSound()
    {
        if (audioSource != null && crashClip != null)
        {
            audioSource.PlayOneShot(crashClip);
        }
    }
}
