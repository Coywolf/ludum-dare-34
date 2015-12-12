using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    [Header("Prefabs")]
    public Transform Path;
    public Transform Spawn;
    public Transform Exit;
    public Transform Intersection;

    public Transform Selector;
        
    [Header("Sounds")]
    public AudioClip SelectClip;
    public AudioClip RotateClip;

    [Header("Game")]
    public int Lives = 5;
    public int Points = 0;

    private Map Map;

    private List<Assets.Intersection> Intersections;
    private int SelectedIntersection;
    private Transform SelectorInstance;

	// Use this for initialization
	void Start () {
        Intersections = new List<Assets.Intersection>();

        Map = new Map(10, 10);
        Map.Iterate(tile =>
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
                var path = Instantiate(prefab, new Vector3(tile.x, 0, tile.y), Quaternion.identity);

                if(prefab == Spawn)
                {
                    ((Transform)path).GetComponent<Spawner>().Initialize(tile.x, tile.y, Map);
                }
                else if(prefab == Intersection)
                {
                    Intersections.Add((Assets.Intersection)tile);
                    ((Assets.Intersection)tile).Instance = (Transform) path;
                }
            }
        });

        SelectorInstance = (Transform) Instantiate(Selector, new Vector3(0, -10, 0), Quaternion.identity);
        SetSelectedIntersection(0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Button1"))
        {
            SetSelectedIntersection(SelectedIntersection + 1);

            if (SelectClip != null)
            {
                AudioSource.PlayClipAtPoint(SelectClip, Camera.main.transform.position);
            }
        }

        if (Input.GetButtonDown("Button2"))
        {
            Intersections[SelectedIntersection].Direction++;

            if (RotateClip != null)
            {
                AudioSource.PlayClipAtPoint(RotateClip, Camera.main.transform.position);
            }
        }
	}

    public void DestroyUnit(Unit unit, int x, int y)
    {
        Destroy(unit.gameObject);
        //lives
        //points
    }

    private void SetSelectedIntersection(int selection)
    {
        SelectedIntersection = selection % Intersections.Count;
        UpdateSelectorPosition();
    }

    private void UpdateSelectorPosition()
    {
        var selectedIntersection = Intersections[SelectedIntersection];
        SelectorInstance.transform.position = new Vector3(selectedIntersection.Instance.position.x, 2, selectedIntersection.Instance.position.z);
    }
}
