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
    public AudioClip HitClip;
    public AudioClip ScoreClip;

    [Header("Game")]
    public int Lives = 5;
    public int Points = 0;

    private Map Map;

    private List<Assets.Intersection> Intersections;
    private int SelectedIntersection;
    private Transform SelectorInstance;
    private GuiController GuiController;

	// Use this for initialization
	void Start () {
        GuiController = FindObjectOfType<GuiController>();
        GuiController.SetLives(Lives);
        GuiController.SetPoints(Points);

        CreateMap();
    }
	
	// Update is called once per frame
	void Update () {
        if (GuiController.GameOverUi.activeInHierarchy) { return; }

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
            while (true)
            {
                Intersections[SelectedIntersection].Direction++;
                var point = new Assets.Point(Intersections[SelectedIntersection].x, Intersections[SelectedIntersection].y);
                var nextPoint = point.Move(Intersections[SelectedIntersection].Direction);
                if(Map.GetTile(nextPoint.X, nextPoint.Y) != null)
                {
                    break;
                }
            }

            if (RotateClip != null)
            {
                AudioSource.PlayClipAtPoint(RotateClip, Camera.main.transform.position);
            }
        }
	}

    private void CreateMap()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Intersections = new List<Assets.Intersection>();

        Map = new Map(10, 10);
        Map.Iterate(tile =>
        {
            Transform prefab = null;

            if (tile is Assets.Path)
            {
                prefab = Path;
            }
            else if (tile is Assets.Spawn)
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
                var path = Instantiate(prefab, new Vector3(tile.x, 0, tile.y), Quaternion.identity) as Transform;
                path.parent = transform;

                if (prefab == Spawn)
                {
                    path.GetComponent<Spawner>().Initialize(tile.x, tile.y, Map);
                }
                else if (prefab == Intersection)
                {
                    Intersections.Add((Assets.Intersection)tile);
                    ((Assets.Intersection)tile).Instance = path;
                }
                else if (prefab == Exit)
                {
                    foreach (var ren in path.GetChild(0).GetComponentsInChildren<Renderer>())
                    {
                        ren.material.color = ((Assets.Exit)tile).Color;
                    }
                }
            }
        });

        SelectorInstance = (Transform)Instantiate(Selector, new Vector3(0, -10, 0), Quaternion.identity);
        SelectorInstance.parent = transform;
        SetSelectedIntersection(0);
    }
    
    public void DestroyUnit(Unit unit, int x, int y)
    {
        Destroy(unit.gameObject);

        var tile = Map.GetTile(x, y);

        if(tile is Assets.Exit && ((Assets.Exit)tile).Color == unit.Color)
        {
            if(ScoreClip != null)
            {
                AudioSource.PlayClipAtPoint(ScoreClip, Camera.main.transform.position);
            }
            Points++;
            GuiController.SetPoints(Points);
        }
        else
        {
            if (HitClip != null)
            {
                AudioSource.PlayClipAtPoint(HitClip, Camera.main.transform.position);
            }
            Lives--;
            GuiController.SetLives(Lives);
        }
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
