using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    IList<Transform> roomArrows;
    IList<Transform> spawnPositions;

	// Use this for initialization
	void Awake () {
        roomArrows = new List<Transform>();
        spawnPositions = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.tag == "RoomArrow")
            {
                roomArrows.Add(child);
            }
            else if (child.tag == "Respawn") {
                spawnPositions.Add(child);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    public Bounds GetBounds() {
        Bounds b = new Bounds(transform.position, 0.1f * Vector3.one);
        foreach (Transform child in transform) {
            b.Encapsulate(child.position);
        }
        return b;
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

    public Transform GetSpawnPos() {
        if (spawnPositions.Count == 0)
        {
            return null;
        }
        return spawnPositions.PickRandom();
    }

    public void RemoveSpawnPos(Transform pos)
    {
        spawnPositions.Remove(pos);
    }

    public void ClearExitMarkers(Transform replacement=null) {
        foreach (Transform child in transform)
        {
            if (child.tag == "RoomArrow")
            {
                Destroy(child.gameObject);
            }
        }

        if (replacement) {
            // Add doors for the unused exists
            foreach (Transform unusedExit in roomArrows)
            {
                var door = Instantiate(replacement, unusedExit.transform.position, unusedExit.transform.rotation);
                // Congrats! I'm your Dad now!!
                door.SetParent(transform);
            }
        }
        roomArrows.Clear();
    }

    public void ClearRespawnThingos() {
        spawnPositions.Clear();
        foreach (Transform child in transform)
        {
            if (child.tag == "Respawn")
            {
                Destroy(child.gameObject);
            }
        }
    }
}
