using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour {

    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float staggerDuration;

    [SerializeField]
    private float attackCooldown;

    private bool _attackCooldown = false;

    private float health;
    private bool isChasing = false;
    private bool isDead = false;

    private GameObject player;
    private Rigidbody2D rb;
    private Animator anim;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger");
        if (other.CompareTag("Bullet"))
        {
            if (health > 0)
            {
                health -= other.GetComponent<BulletBehaviour>().getDamage();
                if (health <= 0)
                {
                    isDead = true;
                    isChasing = false;
                    anim.SetBool("isDead", true);
                }
                else
                {
                    StartCoroutine(StaggerEffect());
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!_attackCooldown)
            {
                StartCoroutine(AttackCooldown());
                PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
                pc.AttackPlayer(damage);
                if (pc.getHealth() <= 0)
                {
                    isChasing = false;
                }
            }
        }
    }

    // Use this for initialization
    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = maxHealth;
        isChasing = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isDead)
        {
            Vector3 difference = player.transform.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
        
    }

    private void FixedUpdate()
    {
        if (isChasing)
        {
            transform.position += transform.right * speed;
        }
    }

    IEnumerator StaggerEffect()
    {
        anim.enabled = false;
        isChasing = false;
        yield return new WaitForSeconds(staggerDuration);
        anim.enabled = true;
        isChasing = true;

    }
    IEnumerator AttackCooldown()
    {
        _attackCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        _attackCooldown = false;
    }
}
