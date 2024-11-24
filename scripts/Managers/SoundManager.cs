using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [System.Serializable]
    public class Clip
    {
        public AudioClip sound;
        public string name;
    }
    [SerializeField] private Clip[] sounds;
    public void PlayClip(string name)
    {
        foreach (Clip clip in sounds){
            if(clip.name == name){
                audioSource.PlayOneShot(clip.sound,1f);
                return;
            }
        }
        Debug.LogWarning("audio clip: " + name + " was not found");
    }

}
