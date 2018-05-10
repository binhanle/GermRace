using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    private AudioSource audioSource;

    public static AudioSource happySound;
    public static AudioSource sadSound;
    public static AudioSource movingSound;
    public static AudioSource victorySound;

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

    public void PlayHappy()
    {
        // plays happy sound
        happySound.Play();
    }

    public void PlaySad()
    {
        // plays sad sound
        sadSound.Play();
    }

    public void PlayMoving()
    {
        // plays moving sound
        movingSound.Play();
    }

    public void PlayVictory()
    {
        // plays victory sound
        victorySound.Play();
    }

    void Awake()
    {
        // initialize audio objects
        happySound = GameObject.Find("Happy").GetComponent<AudioSource>();
        sadSound = GameObject.Find("Sad").GetComponent<AudioSource>();
        movingSound = GameObject.Find("Moving").GetComponent<AudioSource>();
        happySound = GameObject.Find("Victory").GetComponent<AudioSource>();
    }

    void Start()
    {
        // get audio source
        audioSource = GetComponent<AudioSource>();
    }
}
