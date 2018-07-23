using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    //Variable to store an AudioSource from the Background Music Game Object    
    private AudioSource audioSource;

    //Variable to track if sound has been muted
    private static bool muted = false;

    //Variables to store character sounds
    private static AudioSource happySound;
    private static AudioSource sadSound;
    private static AudioSource movingSound;
    private static AudioSource victorySound;

	public void SetEnabled(Toggle toggle)
    {
        // enables or mutes the audio
        audioSource.mute = !toggle.isOn;
    }

    public void SetVolume(Slider slider)
    {
        // set audio volume
        audioSource.volume = slider.value;
    }

    public static void PlayHappy()
    {
        // plays happy sound
        if (!muted)
        {
            happySound.Play();
        }
    }

    public static void PlaySad()
    {
        // plays sad sound
        if (!muted)
        {
            sadSound.Play();
        }
    }

    public static void PlayMoving()
    {
        // plays moving sound
        if (!muted)
        {
            movingSound.Play();
        }
    }

    public static void StopMoving()
    {
        // stops moving sound
        if (!muted)
        {
            movingSound.Stop();
        }
    }

    public static void PlayVictory()
    {
        // plays victory sound
        if (!muted)
        {
            victorySound.Play();
        }
    }

    void Awake()
    {
        // initialize audio objects
        happySound = GameObject.Find("Happy").GetComponent<AudioSource>();
        sadSound = GameObject.Find("Sad").GetComponent<AudioSource>();
        movingSound = GameObject.Find("Moving").GetComponent<AudioSource>();
        victorySound = GameObject.Find("Victory").GetComponent<AudioSource>();
    }

    void Start()
    {
        // get audio source
        audioSource = GetComponent<AudioSource>();
    }
}
