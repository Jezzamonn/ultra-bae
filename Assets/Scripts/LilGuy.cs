using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilGuy : Enemy {

    public AudioClip DieSfx;
    public Rigidbody Coin;

    public float Speed = 0.5f;

    Transform _player;
    Rigidbody _body;

	// Use this for initialization
	void Start () {
        Health = 1;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        _body.MovePosition(_body.position + Speed * Time.fixedDeltaTime * (_player.position - transform.position).normalized);
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
        for (int i = 0; i < 3; i++)
        {
            Rigidbody coin = Instantiate(
                Coin,
                transform.position + Vector3.up + Random.insideUnitSphere,
                Quaternion.identity);
            coin.AddForce(Random.insideUnitSphere + Vector3.up, ForceMode.Impulse);
            Player p = _player.GetComponent<Player>();
            p.AudioSource.PlayOneShot(DieSfx);
        }
    }
}
