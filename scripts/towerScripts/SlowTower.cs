using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            (other.gameObject.GetComponent<Enemy>()).speedMultiplier = 0.4f;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            (other.gameObject.GetComponent<Enemy>()).speedMultiplier = 1f;
        }
    }
}
