using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
    public float Speed = 0.3f;

    private int LastX;
    private int LastY;
    private int TargetX;
    private int TargetY;
    private int Direction;

    private Map Map;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        //var direction = new Vector3(TargetX - LastX, 0, TargetY - LastY);
        //transform.Translate(direction.normalized * Speed * Time.deltaTime, Space.World);
        var step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(TargetX, transform.position.y, TargetY), step);

        if(transform.position.x == TargetX && transform.position.z == TargetY)
        {
            LastX = TargetX;
            LastY = TargetY;
            if(!Map.GetNextTarget(ref TargetX, ref TargetY, ref Direction))
            {
                transform.rotation = Quaternion.Euler(0, 90 * Direction, 90);
            }
        }
	}

    public void Initialize(int x, int y, int direction, Map map)
    {
        LastX = x;
        LastY = y;
        TargetX = x;
        TargetY = y;
        Direction = direction;
        Map = map;

        Map.GetNextTarget(ref TargetX, ref TargetY, ref Direction);
        transform.rotation = Quaternion.Euler(0, 90 * Direction, 90);
    }
}
