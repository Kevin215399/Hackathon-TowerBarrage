using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    [SerializeField] private bool doMove = true;
    private int index;
    private float goX;
    private float goY;
    [SerializeField] private float speed = 2f;
    [SerializeField] public float health;
    [SerializeField] public float damage;
    [SerializeField] private Slider slider;
    [SerializeField] private GetParticleCollision getFireCollision;
    private float startHealth;
    public float speedMultiplier = 1;
    bool didDamage;

    private void Start()
    {
        speedMultiplier *= DebuffManager.Instance.enemySpeedMultiplier;
        health *= DebuffManager.Instance.enemyHealthMultiplier;
        startHealth = health;
    }
    private void Update()
    {
        if (goX == 0 && goY == 0)
        {
            goX = Path.Instance.path[1].x;
            goY = Path.Instance.path[1].y;
            if (doMove)
                transform.position = Path.Instance.path[0];
        }
        if (Vector3.Distance(transform.position, new Vector3(goX, goY, 0)) < 0.2f)
        {
            if (index == Path.Instance.path.Length - 1)
            {
                MainManager.Instance.HitTower(damage * DebuffManager.Instance.enemyDamageMultiplier);
                Destroy(gameObject);
            }
            else
            {
                index++;

                goX = Path.Instance.path[index].x;
                goY = Path.Instance.path[index].y;
            }
        }
        moveStep();

        if (health <= 0)
        {
            MainManager.Instance.KilledEnemy();
            Destroy(gameObject);
        }
        if (slider != null)
            slider.value = Mathf.Ceil(health / startHealth * 10) / 10;

        if(getFireCollision.hitParticle){
            health -= 0.15f * DebuffManager.Instance.playerDamageMultiplier;
        }
    }
    void moveStep()
    {
        if (Vector3.Distance(transform.position, new Vector3(goX, goY, 0)) > 0.2f && doMove)
            transform.position += new Vector3((goX - transform.position.x) / speed * Time.deltaTime / (Vector3.Distance(transform.position, new Vector3(goX, goY, 0)) / 4) * 3f * speedMultiplier, (goY - transform.position.y) / speed * Time.deltaTime / (Vector3.Distance(transform.position, new Vector3(goX, goY, 0)) / 4) * 3f * speedMultiplier, 0);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "healer")
        {
            if (health < startHealth)
                health += 0.02f;
        }
    }
    
}
