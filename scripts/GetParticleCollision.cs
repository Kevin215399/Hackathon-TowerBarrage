using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetParticleCollision : MonoBehaviour
{
    public bool hitParticle;
    [SerializeField] private string tagName;

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == tagName)
        {
            hitParticle = true;
        }
    }
    private void LateUpdate(){
        hitParticle=false;
    }
}
