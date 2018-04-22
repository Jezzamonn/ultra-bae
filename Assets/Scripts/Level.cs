using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level : MonoBehaviour
{
    public List<Room> RoomTemplates;
    public List<Transform> SpawnThings;
    public Transform Ladder;
    public Transform Door;

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
        //if (Input.GetButtonDown("Fire1")) {
        //    DestroyAll();
        //    Generate();
        //}
    }

    void DestroyAll()
    {
        foreach (var room in ActualRooms)
        {
            Destroy(room.gameObject);
        }
    }

    void Generate()
    {
        ActualRooms = new List<Room>();
        FreeRooms = new List<Room>();

        Room firstRoom = Instantiate(RoomTemplates.PickRandom(), Vector3.zero, Quaternion.identity);
        ActualRooms.Add(firstRoom);
        FreeRooms.Add(firstRoom);

        while (ActualRooms.Count < 15)
        {
            Room parentRoom = FreeRooms.PickRandom();
            Transform pos = parentRoom.GetRoomPos();
            if (pos == null)
            {
                FreeRooms.Remove(parentRoom);
                if (FreeRooms.Count == 0)
                {
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
            Bounds b = newRoom.GetBounds();
            b.Expand(-1f);
            foreach (Room room in ActualRooms)
            {
                if (room == parentRoom)
                {
                    continue;
                }
                if (b.Intersects(room.GetBounds()))
                {
                    // Probably too close. Destroy the room and try again!
                    Destroy(newRoom.gameObject);
                    validRoom = false;
                    continue;
                }
            }
            if (!validRoom)
            {
                continue;
            }

            FreeRooms.Add(newRoom);
            ActualRooms.Add(newRoom);
            parentRoom.RemoveRoomPos(pos);
            newRoom.RemoveRoomPos(newPos);
        }


        foreach (Room room in ActualRooms)
        {
            room.ClearExitMarkers(Door);
            // Just fill it with stuff for the moment? >:)
        }

        // -- Spawning Stuff --

        // Spawn Ladder. Skip the player room.
        var ranRoom = ActualRooms[Random.Range(1, ActualRooms.Count())];
        // This can't be null I don't think...
        Instantiate(Ladder, ranRoom.GetSpawnPos().position, Quaternion.identity);

        foreach (Room room in ActualRooms.Skip(1))
        { // don't spawn stuff in the player's room, we're nice
            for (int i = 0; i < 3; i++)
            { // instead of an infinite loop just to be safe
                var pos = room.GetSpawnPos();
                if (pos == null)
                {
                    break;
                }

                Instantiate(SpawnThings.PickRandom(), pos.position, Quaternion.identity);
                // Very important! :)
                room.RemoveSpawnPos(pos);
            }

        }

        foreach (Room room in ActualRooms)
        {
            room.ClearRespawnThingos();
        }
    }
}
