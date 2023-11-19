using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource backsoundSource;
    public AudioSource salesSource;

    public AudioClip backSoundClip;
    public AudioClip gehuClip;
    public AudioClip bercandaClip;

    public static SoundManager instance;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        backsoundSource = GetComponent<AudioSource>();
        backsoundSource.clip = backSoundClip;

        // Check if the audio source is assigned an audio clip
        if (backsoundSource.clip != null)
        {
            // Set the audio source to loop
            backsoundSource.loop = true;

            // Play the music
            backsoundSource.Play();
        }
    }

    public void PlayAudioSales()
    {
        int randomInt = Random.Range(1, 3);

        if(randomInt == 1)
        {
            salesSource.clip = gehuClip;

            // Check if the audio source is assigned an audio clip
            if (salesSource.clip != null)
            {

                // Play the music
                salesSource.Play();
            }
        }
        else
        {
            salesSource.clip = bercandaClip;

            // Check if the audio source is assigned an audio clip
            if (salesSource.clip != null)
            {

                // Play the music
                salesSource.Play();
            }
        }
        
    }
}
