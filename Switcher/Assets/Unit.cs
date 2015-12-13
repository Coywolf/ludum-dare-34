using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
    public float Speed = 0.3f;

    public Color Color;
    
    private int TargetX;
    private int TargetY;
    private int Direction;

    private Map Map;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        var step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(TargetX, transform.position.y, TargetY), step);

        if(transform.position.x == TargetX && transform.position.z == TargetY)
        {
            if(Map.GetNextTarget(ref TargetX, ref TargetY, ref Direction))
            {
                transform.rotation = Quaternion.Euler(0, 90 * Direction, 90);
            }
            else
            {
                FindObjectOfType<Game>().DestroyUnit(this, TargetX, TargetY);
            }
        }
	}

    public void Initialize(int x, int y, int direction, Map map, Color color)
    {
        TargetX = x;
        TargetY = y;
        Direction = direction;
        Map = map;

        Map.GetNextTarget(ref TargetX, ref TargetY, ref Direction);
        transform.rotation = Quaternion.Euler(0, 90 * Direction, 90);

        Color = color;
        foreach(var ren in GetComponentsInChildren<Renderer>())
        {
            ren.material.color = color;
        }
    }
}
