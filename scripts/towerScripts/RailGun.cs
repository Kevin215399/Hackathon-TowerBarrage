using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun : MonoBehaviour
{
    [SerializeField] private GameObject laser;
    [SerializeField] private float speed;
    [SerializeField] private float shootLength;
    [SerializeField] private TowerPlacement towerPlacement;
    [SerializeField] private BasicTower basicTower;
    private void Start()
    {
        towerPlacement.towerPlaced += EnableTower;
        MainManager.Instance.DebuffAll += DebuffTower;
    }
    private void DebuffTower(){
        speed *= 1.5f;
    }
    private void EnableTower()
    {
        StartCoroutine(laserShoot());
        towerPlacement.towerPlaced -= EnableTower;
    }
    IEnumerator laserShoot()
    {
        yield return new WaitForSeconds(speed);
        if (basicTower.target != null && Vector3.Distance(transform.position, basicTower.target.transform.position) < 3)
        {
            laser.GetComponent<SpriteRenderer>().enabled = true;
            laser.GetComponent<PoisonAffect>().isActive = true;
            GetComponent<SoundManager>().PlayClip("shoot");
            basicTower.pauseMovement = true;
        }
        StartCoroutine(laserOff());
    }
    IEnumerator laserOff()
    {
        yield return new WaitForSeconds(shootLength);
        laser.GetComponent<SpriteRenderer>().enabled = false;
        laser.GetComponent<PoisonAffect>().isActive = false;
        basicTower.pauseMovement = false;
        StartCoroutine(laserShoot());
    }
}
