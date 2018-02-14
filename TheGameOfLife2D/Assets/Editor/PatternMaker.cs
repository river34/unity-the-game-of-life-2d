using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PatternMaker : MonoBehaviour {

    public GameObject CellPrefab;

	public int NumRow = 7;

    public int NumCol = 7;

	public float UNIT = 0.5f;

    public CellPattern Pattern;

    Cell[] grid;

	// Use this for initialization
	void Start () {
        Pattern.Positions.Clear();
		grid = new Cell[NumCol * NumRow];
		for (int i = 0; i < NumRow * NumCol; i++)
		{
			GameObject go = Instantiate(CellPrefab);
			go.transform.SetParent(transform);
			grid[i] = go.GetComponent<Cell>();
			grid[i].SetGrid(NumRow, NumCol);
			grid[i].SetPosition(i % NumCol, i / NumCol);
            grid[i].SetStatus(Cell.Status.Stationary);
            grid[i].OnClicked += OnClicked;
		}
	}

    void OnClicked(int x, int y)
    {
        Position position = new Position();
        position.x = x - NumCol / 2;
        position.y = y - NumRow / 2;
        Pattern.Positions.Add(position);
    }

    private void OnApplicationPause(bool pause)
    {
        EditorUtility.SetDirty(Pattern);
    }
}
