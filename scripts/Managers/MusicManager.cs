using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set;}

    void Start(){
        if(Instance == null){
            Instance = this;
            gameObject.AddComponent<DontDestroyOnLoad>();
        } else {
            Destroy(gameObject);
        }
    }
}
