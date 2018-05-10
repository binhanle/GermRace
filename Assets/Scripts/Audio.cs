using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    private AudioSource audioSource;

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

    void Start()
    {
        // get audio source
        audioSource = GetComponent<AudioSource>();
    }
}
