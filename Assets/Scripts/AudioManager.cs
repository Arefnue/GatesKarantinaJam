using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    
    public static AudioManager manager;
    
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    

    #endregion

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource enemySfxSource;
    public void PlaySfx(AudioClip clip,int priorityValue=128,float volumeValue = 0.2f, float pitchValue = 1f)
    {
        sfxSource.volume = volumeValue;
        sfxSource.priority = priorityValue;
        sfxSource.pitch = pitchValue;
        sfxSource.PlayOneShot(clip);
    }
    
    public void PlayEnemySfx(AudioClip clip,int priorityValue=128,float volumeValue = 0.05f, float pitchValue = 1f)
    {
        enemySfxSource.volume = volumeValue;
        enemySfxSource.priority = priorityValue;
        enemySfxSource.pitch = pitchValue;
        enemySfxSource.PlayOneShot(clip);
    }
    
    
}
