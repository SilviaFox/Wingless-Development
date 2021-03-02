using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public class Sound // Creates a class that the AudioManager can call
{

    // Info about an audio clip
    public string name; 

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
