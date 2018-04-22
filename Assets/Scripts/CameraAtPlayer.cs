using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAtPlayer : MonoBehaviour
{

    Transform _player;
    public float Offset = 10;

    RaycastHit[] _hideStuffHits;

    // Use this for initialization
    void Start()
    {
        _player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _player.position - Offset * transform.forward, 0.1f);
        HideStuffBetweenPlayer();
    }

    void HideStuffBetweenPlayer()
    {
        if (_hideStuffHits != null)
        {
            foreach (var hit in _hideStuffHits)
            {
                var r = hit.collider.GetComponent<Renderer>();
                if (r)
                {
                    r.enabled = true;
                }
            }
        }

        RaycastHit[] hits = Physics.RaycastAll(
            transform.position,
            (_player.transform.position - transform.position),
            Vector3.Distance(transform.position, _player.transform.position + 0.75f * Vector3.up) - 0.2f);

        foreach (RaycastHit hit in hits)
        {
            Renderer r = hit.collider.GetComponent<Renderer>();
            if (r)
            {
                r.enabled = false;
            }
        }

        // Save for next time
        _hideStuffHits = hits;
    }
}
