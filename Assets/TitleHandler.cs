using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleHandler : MonoBehaviour {

    public int CurScreen = 0;
    public List<RectTransform> Screens;
    public bool SkipToGame = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetButtonDown("Submit")) {

            Screens[CurScreen].gameObject.SetActive(false);
            CurScreen++;
            if (CurScreen < Screens.Count) {
                Screens[CurScreen].gameObject.SetActive(true);
            }
            else {
                SceneManager.LoadScene("Level");
            }

        }
	}
}
