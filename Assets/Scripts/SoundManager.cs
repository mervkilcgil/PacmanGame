using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip intermissionSound;
    [SerializeField] private AudioClip chompSound;
    [SerializeField] private AudioClip eatGhostSound;
    [SerializeField] private AudioClip eatFruitSound;
    [SerializeField] private AudioClip deathSound;
    

    private static SoundManager instance;
    public static SoundManager Instance => instance;
    
    [SerializeField] private AudioSource mAudioSource;
    [SerializeField] private AudioSource mMusicAudioSource;

    public void Awake()
    {
        instance = this;
        mAudioSource.loop = false;
    }

    public void PlayInterMissionSound()
    {
        mAudioSource.clip = intermissionSound;
        mAudioSource.Play();
    }
    
    public void PlayMusic()
    {
        mMusicAudioSource.clip = introMusic;
        mMusicAudioSource.loop = true;
        mMusicAudioSource.Play();
    }
    
    public void StopMusic()
    {
        mMusicAudioSource.Stop();
    }

    public void PlayChomp()
    {
        mAudioSource.clip = chompSound;
        mAudioSource.Play();
    }
    
    public void PlayEatGhost()
    {
        mAudioSource.clip = eatGhostSound;
        mAudioSource.Play();
    }
    
    public void PlayEatFruit()
    {
        mAudioSource.clip = eatFruitSound;
        mAudioSource.Play();
    }
    
    public void PlayDeath()
    {
        mAudioSource.clip = deathSound;
        mAudioSource.Play();
    }
}
