using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    [SerializeField] private ParticleSystem flame;
    [SerializeField] private BasicTower basicTower;
    [SerializeField] private TowerPlacement towerPlacement;
    private void Update(){
        if (!towerPlacement.placing && basicTower.target != null && Vector3.Distance(transform.position, basicTower.target.transform.position) < 2)
        {
            if(!flame.isPlaying)
            flame.Play();
        } else {
            flame.Stop();
        }
    }
    
}
