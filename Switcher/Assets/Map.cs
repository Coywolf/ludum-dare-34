using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {
    public Transform Path;
    public Transform Spawn;
    public Transform Exit;
    public Transform Intersection;

    private MapGenerator Generator;

	// Use this for initialization
	void Start () {
        Generator = new MapGenerator(10, 10);
        Generator.Iterate(tile =>
        {
            Transform prefab = null;

            if(tile is Assets.Path)
            {
                prefab = Path;
            }
            else if(tile is Assets.Spawn)
            {
                prefab = Spawn;
            }
            else if (tile is Assets.Exit)
            {
                prefab = Exit;
            }
            else if (tile is Assets.Intersection)
            {
                prefab = Intersection;
            }

            if (prefab != null)
            {
                var path = Instantiate(prefab, new Vector3(tile.x - 4.5f, 0, tile.y - 4.5f), Quaternion.identity);
            }
        });
	}
	
	// Update is called once per frame
	void Update () {
        	
	}
}
