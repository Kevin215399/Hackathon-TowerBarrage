using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set;}
    public Vector2 mousePosition = Vector2.zero;
    private void Start(){
        if(Instance != null){
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    private void Update(){
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
