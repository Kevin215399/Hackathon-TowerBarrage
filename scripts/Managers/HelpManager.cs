using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
    [SerializeField] GameObject[] contents;

    public void SwitchMenu(int i)
    {
        foreach (GameObject x in contents)
        {
            x.SetActive(false);
        }
        contents[i].SetActive(true);
    }
}
