using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    [SerializeField]private float speed = 10;
    [SerializeField]private float damage = 5;
    
    // Update is called once per frame
    private void Update()
    {
        transform.position += -transform.right * speed * Time.deltaTime;
        if(Vector3.Distance(transform.position, Vector3.zero)>30){
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D other){
        Debug.Log("collided with: " + other.gameObject.tag);
        if(other.gameObject.tag == "enemy"){
            other.gameObject.GetComponent<Enemy>().health -= damage * DebuffManager.Instance.playerDamageMultiplier;
            Destroy(gameObject);
        }
    }
}
