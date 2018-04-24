using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAudio : MonoBehaviour
{

    private static MainAudio instance = null;
    public static MainAudio Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position;
    }
}