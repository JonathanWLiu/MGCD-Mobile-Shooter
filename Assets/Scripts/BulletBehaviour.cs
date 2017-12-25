using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private float damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifespan;

	// Use this for initialization
	void Awake () {
        Destroy(gameObject, lifespan);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        transform.position += transform.right * speed;
    }

    public void setDamage(float n)
    {
        damage = n;
    }
    public float getDamage()
    {
        return damage;
    }
}
