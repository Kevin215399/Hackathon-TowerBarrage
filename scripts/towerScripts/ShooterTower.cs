using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTower : MonoBehaviour
{
    [SerializeField] private float shootSpeed;
    [SerializeField] private TowerPlacement towerPlacement;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float range = 2;
    [SerializeField] private BasicTower basicTower;
    void Start()
    {
        StartCoroutine(ShootDelay(shootSpeed));
    }
    

    IEnumerator ShootDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("enemy"));
        if (!towerPlacement.placing && enemies.Count > 0 && basicTower.target != null && Vector3.Distance(transform.position, basicTower.target.transform.position) < range)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            GetComponent<SoundManager>().PlayClip("shoot");
        }
        StartCoroutine(ShootDelay(delay));
    }
}
