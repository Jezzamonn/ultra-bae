using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    public List<Room> RoomTemplates;

    private List<Room> ActualRooms;
    private List<Room> FreeRooms;

	// Use this for initialization
	void Start()
	{  
        Generate();

	}

	// Update is called once per frame
	void Update()
	{
		
	}

    void Generate() {
        ActualRooms = new List<Room>();
        FreeRooms = new List<Room>();

        Room firstRoom = Instantiate(RoomTemplates.PickRandom(), Vector3.zero, Quaternion.identity);
        ActualRooms.Add(firstRoom);
        FreeRooms.Add(firstRoom);

        for (int i = 0; i < 10; i++)
        {
            Room parentRoom = FreeRooms.PickRandom();
            Transform pos = parentRoom.GetRoomPos();
            if (pos == null)
            {
                FreeRooms.Remove(parentRoom);
                if (FreeRooms.Count == 0) {
                    break;
                }
                continue;
            }

            Room newRoom = Instantiate(RoomTemplates.PickRandom(), Vector3.zero, Quaternion.identity);
            Transform newPos = newRoom.GetRoomPos();

            newRoom.transform.rotation =
                pos.rotation * Quaternion.Inverse(newPos.rotation) * Quaternion.AngleAxis(180, Vector3.up);
            newRoom.transform.position = pos.position - newPos.position;

            // Quick, non-foolproof way to check if things are touching
            bool validRoom = true;
            foreach (Room room in ActualRooms) {
                if ((room.transform.position - newRoom.transform.position).sqrMagnitude < 6 * 6) {
                    // Probably too close. Destroy the room and try again!
                    Destroy(newRoom.gameObject);
                    validRoom = false;
                    continue;
                }
            }
            if (!validRoom) {
                continue;
            }

            FreeRooms.Add(newRoom);
            ActualRooms.Add(newRoom);
            parentRoom.RemoveRoomPos(pos);
        }

        foreach (Room room in ActualRooms)
        {
            room.ClearExitMarkers();
        }
    }
}
