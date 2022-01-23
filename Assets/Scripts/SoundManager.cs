using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip soundSource;
    [SerializeField] private AudioClip musicSource;
    

    public void PlaySound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = soundSource;
        audio.Play();
    }
    
    public void PlayMusic()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = musicSource;
    }

}
