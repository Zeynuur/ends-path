using UnityEngine;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;

    
    void Start()
    {
        if(RoomController.instance == null )
        {
            Debug.Log("You pressed play in the wrong scene! ");
            return;
        }

        RoomController.instance.RegisterRoom(this);
    }

    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCentre()
    {
        return new Vector3(X * Width, Y * Height);
    }
}
