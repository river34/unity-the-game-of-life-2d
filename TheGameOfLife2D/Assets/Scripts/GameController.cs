using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject CellPrefab;

	public int NumRow = 10;

	public int NumCol = 10;

    public CellPattern Pattern;

    private Cell[] Grid;

    private CellPattern _Pattern;

	// Use this for initialization
	void Start () {
		// populate the grid of size numCol x numRow
		Grid = new Cell[NumCol * NumRow];
        for (int i = 0; i < NumRow * NumCol; i++)
        {
			GameObject go = Instantiate(CellPrefab);
            go.transform.SetParent(transform);
			Grid[i] = go.GetComponent<Cell>();
			Grid[i].SetGrid(NumRow, NumCol);
			Grid[i].SetPosition(i % NumCol, i / NumCol);
            Grid[i].SetStatus(Cell.Status.Dead);
        }

        // populate the cell pattern
        for (int i = 0; i < Pattern.Positions.Count; i++)
        {
            int x = Pattern.Positions[i].x + NumCol / 2;
            int y = Pattern.Positions[i].y + NumRow / 2;
            int position = y * NumCol + x;
			Grid[position].SetStatus(Cell.Status.Alive);
        }

        _Pattern = Pattern;
	}
	
	// Update is called once per frame
	void Update () {
        // tick each active cell
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("tick"); 
			for (int i = 0; i < NumCol * NumRow; i++)
			{
				Grid[i].Tick(ref Grid);
			}
			for (int i = 0; i < NumCol * NumRow; i++)
			{
                Grid[i].UpdateStatus();
			}
        }
	}

    private void LateUpdate()
    {
        if (Pattern != _Pattern)
        {
            OnPatternChanged();
        }
    }

    void OnPatternChanged()
    {
        for (int i = 0; i < Grid.Length; i++)
        {
            Destroy(Grid[i].gameObject);
            Grid[i] = null;
        }
        Start();
    }
}
