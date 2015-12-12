using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {
    public Transform Path;

    private MapGenerator Generator;

	// Use this for initialization
	void Start () {
        Generator = new MapGenerator(10, 10);
        Generator.Iterate((x, y) =>
        {
            var path = Instantiate(Path, new Vector3(x - 4.5f, 0, y - 4.5f), Quaternion.identity);
        });
	}
	
	// Update is called once per frame
	void Update () {
        	
	}
}
