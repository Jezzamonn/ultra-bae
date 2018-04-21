using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public float Speed = 1f;
    public float HurtPushBack = 1f;
    public float HurtJump = 1f;

    public Rigidbody Bullet;

    Rigidbody _body;
    Vector3 _moveDir;
    Vector3 _bulletDir;

   // Use this for initialization
    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (_moveDir.sqrMagnitude > 0.1) {
            _bulletDir = _moveDir.normalized;
        }

        if (Input.GetButtonDown("Jump")) {
            // Spawn Bullet
            Rigidbody bullet = Instantiate(Bullet, transform.position + 0.5f * Vector3.up, Quaternion.identity);
            bullet.AddForce(5f * _bulletDir, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        _body.MovePosition(_body.position + Speed * Time.fixedDeltaTime * _moveDir);
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
        if (other.tag == "HurtPlayer")
        {
            _body.AddForce(
                HurtPushBack * (transform.position - other.transform.position) +
                HurtJump * Vector3.up,
                ForceMode.Impulse);
        }
    }
}
