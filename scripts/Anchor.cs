using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    public bool isOnScreen = false;

    private void Update(){
        if(GetComponent<SpriteRenderer>().isVisible){
            isOnScreen = true;
        }
    }
}
