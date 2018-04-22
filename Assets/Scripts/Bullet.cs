using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float Speed = 5f;
    public float Length = 10f;
    public Vector3 Dir;

    Rigidbody _body;
    float _lengthTraveled = 0;

    // Use this for initialization
    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_lengthTraveled > Length)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        _body.MovePosition(_body.position + Speed * Time.fixedDeltaTime * Dir);
        _lengthTraveled += Speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage();
            Destroy(gameObject);;
        }
    }
}
