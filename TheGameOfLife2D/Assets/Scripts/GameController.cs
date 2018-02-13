using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject CellPrefab;

	public int NumRow = 10;

	public int NumCol = 10;

	public CellPattern[] PatternBank;

	public float DeltaTime = 0.3f;

	Cell[] grid;

	CellPattern curPattern;

    float curTime;

	bool isRunning;

	CellPattern pattern;

    float TIME_CHANGE = 0.1f;

    int curPatternId = 0;

    void Start()
    {
        if (PatternBank.Length > 0)
		{
			pattern = PatternBank[curPatternId];
		}
    }

    // Use this for initialization
    void Init ()
    {
		// populate the grid of size numCol x numRow
		grid = new Cell[NumCol * NumRow];
        for (int i = 0; i < NumRow * NumCol; i++)
        {
			GameObject go = Instantiate(CellPrefab);
            go.transform.SetParent(transform);
			grid[i] = go.GetComponent<Cell>();
			grid[i].SetGrid(NumRow, NumCol);
			grid[i].SetPosition(i % NumCol, i / NumCol);
            grid[i].SetStatus(Cell.Status.Dead);
        }

        // populate the cell pattern
        for (int i = 0; i < pattern.Positions.Count; i++)
        {
            int x = pattern.Positions[i].x + NumCol / 2;
            int y = pattern.Positions[i].y + NumRow / 2;
            int position = y * NumCol + x;
			grid[position].SetStatus(Cell.Status.Alive);
        }

        curPattern = pattern;
	}

	// Update is called once per frame
	void Update()
	{
        // tick each active cell
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("tick"); 
            Tick();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isRunning == false)
			{
				isRunning = true;
			}
			else if (isRunning == true)
			{
                isRunning = false;
			}
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isRunning == true)
        {
            if (DeltaTime > 2 * TIME_CHANGE)
            {
                DeltaTime -= TIME_CHANGE;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			DeltaTime += TIME_CHANGE;
		}

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            curPatternId = (curPatternId + PatternBank.Length - 1) % PatternBank.Length;
            pattern = PatternBank[curPatternId];
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			curPatternId = (curPatternId + 1) % PatternBank.Length;
            pattern = PatternBank[curPatternId];
		}

        if (isRunning == true)
		{
			if (Time.time - curTime > DeltaTime)
			{
				curTime = Time.time;
				Tick();
			}
		}
	}

    void Tick()
    {
        for (int i = 0; i < grid.Length; i++)
		{
			grid[i].Tick(ref grid);
		}
        for (int i = 0; i < grid.Length; i++)
		{
			grid[i].UpdateStatus();
		}
    }

    void LateUpdate()
    {
        if (pattern != curPattern)
        {
            OnPatternChanged();
        }
    }

    void OnPatternChanged()
    {
        if (grid != null)
		{
			for (int i = 0; i < grid.Length; i++)
			{
				Destroy(grid[i].gameObject);
				grid[i] = null;
			}
        }
        Init();
    }
}
