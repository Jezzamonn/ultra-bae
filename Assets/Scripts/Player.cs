using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Speed = 1f;
    public float HurtPushBack = 1f;
    public float HurtJump = 1f;

    public int NumBullets = 3;
    public float BulletSpread = 60f;
    public float BulletCooldown = 0.4f;

    private float _bulletCount = 0;

    public Bullet Bullet;

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
        if (_moveDir.sqrMagnitude > 0.1)
        {
            _bulletDir = _moveDir.normalized;
        }

        if (_bulletCount > 0)
        {
            _bulletCount -= Time.deltaTime;
        }

        // Get the ground normal
        RaycastHit hit;
        Vector3 bulletUp = Vector3.up;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1f))
        {
            bulletUp = hit.normal;
        }

        if (Input.GetButton("Jump") && _bulletCount <= 0)
        {
            // Spawn Bullet
            for (int i = 0; i < NumBullets; i ++)
            {
                float amt = 0.5f;
                if (NumBullets > 1) {
                    amt = (float) i / (NumBullets - 1);
                }
                Bullet bullet = Instantiate(Bullet, transform.position + 0.5f * Vector3.up, Quaternion.identity);
                Vector3 dir = Vector3.ProjectOnPlane(
                    Quaternion.AngleAxis((amt - 0.5f) * BulletSpread, Vector3.up) * _bulletDir,
                    bulletUp);

                bullet.Dir = dir;

                bullet.transform.forward = dir;
            }
            _bulletCount = BulletCooldown;
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
        if (other.tag == "HurtPlayer" || other.tag == "Enemy")
        {
            _body.AddForce(
                HurtPushBack * (transform.position - other.transform.position) +
                HurtJump * Vector3.up,
                ForceMode.Impulse);
        }
    }
}
