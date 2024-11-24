using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAffect : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float damageFrequency;
    public List<GameObject> objectsInRange = new List<GameObject>();
    public bool isActive = true;

    private void Start()
    {
        StartCoroutine(ApplyDamage());
    }

    IEnumerator ApplyDamage()
    {
        yield return new WaitForSeconds(damageFrequency);
        if (isActive)
        {
            foreach (GameObject x in objectsInRange)
            {
                if (x != null)
                    x.GetComponent<Enemy>().health -= damage;
            }
        }
        objectsInRange = new List<GameObject>();
        StartCoroutine(ApplyDamage());
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            if (!objectsInRange.Contains(other.gameObject))
            {
                objectsInRange.Add(other.gameObject);
            }
        }
    }

}
