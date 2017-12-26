using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour : MonoBehaviour {
    [System.Serializable]
    public class DropItem
    {
        public GameObject itemPrefab;
        public float dropChance;
    }

    [SerializeField]
    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float staggerDuration;
    [SerializeField]
    private float corpseSpanTime;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private DropItem[] dropitem;
    private bool _attackCooldown = false;

    private float health;
    private bool isChasing = false;
    private bool isDead = false;

    private GameObject player;
    private Animator anim;
    private ZombieSound zombieSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (health > 0)
            {
                health -= other.GetComponent<BulletBehaviour>().getDamage();
                Destroy(other.gameObject);
                zombieSound.PlaySoundHit();
                if (health <= 0)
                {
                    isDead = true;
                    isChasing = false;
                    anim.SetBool("isDead", true);
                    zombieSound.StartZombieSound();
                    for (int i = 0; i < dropitem.Length; i++)
                    {
                        float luckyNumber = Random.value;
                        if (luckyNumber <= dropitem[i].dropChance/100)
                        {
                            Instantiate(dropitem[i].itemPrefab, transform.position, Quaternion.identity);
                        }
                        //Debug.Log(luckyNumber);
                    }
                    GetComponent<CapsuleCollider2D>().enabled = false;
                    Destroy(gameObject, corpseSpanTime);
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
                zombieSound.PlaySoundAttack();
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
        zombieSound = GetComponent<ZombieSound>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        health = maxHealth;
        isChasing = true;
        zombieSound.StartZombieSound();
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
        if (isChasing && !isDead)
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
