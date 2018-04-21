using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Rigidbody _body;

    Vector3 _moveDir;

    public float Speed = 1f;
    public float HurtPushBack = 1f;
    public float HurtJump = 1f;

    // Use this for initialization
    void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
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
