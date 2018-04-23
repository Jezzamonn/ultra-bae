using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleHandler : MonoBehaviour {

    public int CurScreen = 0;
    public List<RectTransform> Screens;
    public bool SkipToGame = false;

    public SocketIoClient Client;
    public InputField UserNameInput;
    public Text UrlText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        bool nextScreen = Input.GetButtonDown("Submit");

        if (CurScreen == 5) {
            nextScreen = Input.GetKeyDown(KeyCode.Return) && UserNameInput.text.Length > 0;
        }
        if (CurScreen == 6) {
            nextScreen = Client.PhoneConnected;
        }
		
        if (nextScreen) {

            if (SkipToGame) {
                SceneManager.LoadScene("Level");
                return;
            }

            Screens[CurScreen].gameObject.SetActive(false);

            if (CurScreen == 5) {
                Client.UserName = UserNameInput.text;
                Client.DoOpen();
            }

            CurScreen++;

            if (CurScreen == 6) {
                UrlText.text = UrlText.text.Replace("USERNAME", Client.UserName);
            }

            if (CurScreen < Screens.Count) {
                Screens[CurScreen].gameObject.SetActive(true);
            }
            else {
                SceneManager.LoadScene("Level");
            }

        }
	}
}
