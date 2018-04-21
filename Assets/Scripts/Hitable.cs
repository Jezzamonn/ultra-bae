using UnityEngine;
using System.Collections;

public class Hitable : MonoBehaviour
{

    protected int Health { get; set; }

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
			
	}

    public virtual void TakeDamage() {
        Health--;
        if (Health <= 0) {
            Die();
        }
    }

    public virtual void Die() {
        // For the mo, just remove the game object
        Destroy(gameObject);
    }
}
