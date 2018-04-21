using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilGuy : Enemy {

    public Rigidbody Coin;

	// Use this for initialization
	void Start () {
        Health = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        CheckHurt(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckHurt(other);
    }

    private void CheckHurt(Collider other)
    {
        if (other.tag == "HurtEnemy")
        {
            TakeDamage();
        }
    }

    public override void Die()
    {
        base.Die();
        for (int i = 0; i < 10; i++)
        {
            Rigidbody coin = Instantiate(
                Coin,
                transform.position + Random.insideUnitSphere,
                Quaternion.identity);
            coin.AddForce(Random.insideUnitSphere + Vector3.up, ForceMode.Impulse);
        }
    }
}
