using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternMaker : MonoBehaviour {

    public CellPattern Pattern;

    public GameObject CellPrefab;

	public int NumRow = 7;

    public int NumCol = 7;

    public float UNIT = 0.5f;

    private Cell[] Grid;

	// Use this for initialization
	void Start () {
        Pattern.Positions.Clear();
		Grid = new Cell[NumCol * NumRow];
		for (int i = 0; i < NumRow * NumCol; i++)
		{
			GameObject go = Instantiate(CellPrefab);
			go.transform.SetParent(transform);
			Grid[i] = go.GetComponent<Cell>();
			Grid[i].SetGrid(NumRow, NumCol);
			Grid[i].SetPosition(i % NumCol, i / NumCol);
            Grid[i].SetStatus(Cell.Status.Stationary);
            Grid[i].OnClicked += OnClicked;
		}
	}

    void OnClicked(int x, int y)
    {
        Position position = new Position();
        position.x = x - NumCol / 2;
        position.y = y - NumRow / 2;
        Pattern.Positions.Add(position);
    }
}
