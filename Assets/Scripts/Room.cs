using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    IList<Transform> roomArrows;

	// Use this for initialization
	void Awake () {
        roomArrows = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.tag == "RoomArrow")
            {
                roomArrows.Add(child);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Transform GetRoomPos() {
        if (roomArrows.Count == 0) {
            return null;
        }
        Transform pos = roomArrows[Random.Range(0, roomArrows.Count)];
        return pos;
    }

    public void RemoveRoomPos(Transform pos) {
        roomArrows.Remove(pos);
    }

    public void ClearExitMarkers() {
        foreach (Transform child in transform)
        {
            if (child.tag == "RoomArrow")
            {
                Destroy(child.gameObject);
            }
        }
    }
}
