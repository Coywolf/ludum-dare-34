using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
    public Transform Unit;

    public float MinFrequency = 2.0f;
    public float MaxFrequency = 10.0f;

    private float Frequency;
    private float TimeSinceSpawn = 0f;

    private Map Map;
    private int X;
    private int Y;
    private int Direction;

    private GuiController Controller;

    // Use this for initialization
    void Start () {
        Frequency = Random.Range(MinFrequency, MaxFrequency);
        Controller = FindObjectOfType<GuiController>();

        if (X == 0) Direction = 0;
        else if (Y == 0) Direction = 3;
        else if (X == Map.Width - 1) Direction = 2;
        else Direction = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (Controller.GameOverUi.activeInHierarchy) { return; }

        TimeSinceSpawn += Time.deltaTime;

        if(TimeSinceSpawn >= Frequency)
        {
            TimeSinceSpawn = 0f;
            Spawn();
        }
	}

    public void Initialize(int x, int y, Map map)
    {
        X = x;
        Y = y;
        Map = map;
    }

    public void Spawn()
    {
        var unit = Instantiate(Unit, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.Euler(0, 0, 90)) as Transform;
        unit.parent = transform.parent;
        unit.GetComponent<Unit>().Initialize(X, Y, Direction, Map, Map.Colors[Random.Range(0, Map.Colors.Count)]);
    }
}
