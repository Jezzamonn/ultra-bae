using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUi : MonoBehaviour {

    Player _player;
    public SpriteRenderer GoodHeart;
    public SpriteRenderer BadHeart;

    private List<SpriteRenderer> _goodHearts;
    private List<SpriteRenderer> _badHearts;

    // Use this for initialization
    void Start()
    {
        _player = FindObjectOfType<Player>();

        MakeHearts();
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < _goodHearts.Count; i ++) {
            _goodHearts[i].enabled = (i < _player.Health);
            _badHearts[i].enabled = !(i < _player.Health);
        }
	}

    void MakeHearts() {
        // TODO: clear existing stuff properly
        _goodHearts = new List<SpriteRenderer>();
        _badHearts = new List<SpriteRenderer>();

        for (int i = 0; i < _player.MaxHealth; i ++) {
            var good = Instantiate(GoodHeart, 80 * i * Vector3.right, Quaternion.identity);
            good.transform.localScale = 1000 * Vector3.one;
            good.transform.SetParent(transform, false);
            _goodHearts.Add(good);

            var bad = Instantiate(BadHeart, 80 * i * Vector3.right, Quaternion.identity);
            bad.transform.localScale = 1000 * Vector3.one;
            bad.transform.SetParent(transform, false);
            _badHearts.Add(bad);
        }
    }
}
