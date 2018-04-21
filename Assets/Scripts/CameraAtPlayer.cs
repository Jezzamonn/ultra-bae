using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAtPlayer : MonoBehaviour {

    Transform _player;
    float Offset = 20;

	// Use this for initialization
	void Start () {
        _player = GameObject.Find("Player").transform;
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, _player.position - Offset * transform.forward, 0.1f);
	}
}
