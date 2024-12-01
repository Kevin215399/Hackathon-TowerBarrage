using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : MonoBehaviour
{
    [SerializeField] private TowerPlacement towerPlacement;
    
    public GameObject target {get;private set;}
    private List<GameObject> enemies = new List<GameObject>();
    public bool pauseMovement;

    private void Update()
    {
        enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("enemy"));
        if (enemies.Count > 0 && !towerPlacement.placing)
        {
            GameObject closestObject = null;
            float closestDistance = 1000;
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = enemy;
                }
            }
            Vector2 direction = closestObject.transform.position - transform.position;
            if(!pauseMovement)
            transform.rotation = Quaternion.FromToRotation(Vector3.left, direction);
            target = closestObject;
        } else {
            target = null;
        }
    }
    
}
