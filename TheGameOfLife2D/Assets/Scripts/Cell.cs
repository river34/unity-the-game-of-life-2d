using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cell : MonoBehaviour {

	public float UNIT = 0.5f;

	public enum Status { Dead, Alive, Stationary, Clicked };

	Status curStatus, nextStatus;

    int x, y;

    int numRow, numCol;

    SpriteRenderer rend;

    Color activeColor = new Color(1, 1, 0, 1);

    Color inactiveColor = new Color(1, 1, 0, 0);

    public event Action<int, int> OnClicked;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public void SetGrid(int numRow, int numCol)
    {
        this.numRow = numRow;
        this.numCol = numCol;
    }

    public void SetPosition (int x, int y)
    {
        this.x = x;
        this.y = y;
		transform.position = new Vector2((x - numCol / 2) * UNIT, (y - numRow / 2) * UNIT);
	}

    public void SetStatus(Status status)
    {
        curStatus = status;
        if (curStatus == Status.Dead)
        {
            rend.color = inactiveColor;
        }
        if (curStatus == Status.Alive)
		{
			rend.color = activeColor;
		}
        if (curStatus == Status.Stationary)
        {
            rend.color = Color.gray;
        }
        if (curStatus == Status.Clicked)
        {
            rend.color = Color.green;
        }
    }

	// follow the rules:
	// - Each "populated" cell with one or no neighbour dies, as if by solitude
	// - Each "populated" cell with four or moe neighbour dies, as if by overpopulation
	// - Each "populated" cell with two or three neighbours survives
	// - Each "empty" cell with three neighbours becomes populated
	public void Tick(ref Cell[] grids)
    {
        // calculate number of neighbours
        int numNeighbours = 0;

        // check up, right-up, right, right-down, down, left-down, left, left-up
        int self = y * numCol + x;
        int up = self + numCol;

		//Debug.Log("---------" + row + ", " + col);
		//Debug.Log("self = " + self);
		if (y < numRow - 1 && grids[up].curStatus == Status.Alive)
        {
            numNeighbours++;
            //Debug.Log("Up = " + up);
        }
        int rightUp = up + 1;
        if (y < numRow - 1 && x < numCol - 1 && grids[rightUp].curStatus == Status.Alive)
        {
			numNeighbours++;
            //Debug.Log("Right up = " + rightUp);
        }
        int right = self + 1;
		if (x < numCol - 1 && grids[right].curStatus == Status.Alive)
		{
			numNeighbours++;
            //Debug.Log("Right = " + right);
		}
        int rightDown = self - numCol + 1;
        if (y > 0 && x < numCol - 1 && grids[rightDown].curStatus == Status.Alive)
		{
			numNeighbours++;
            //Debug.Log("Right down = " + rightDown);
		}
        int down = self - numCol;
		if (y  > 0 && grids[down].curStatus == Status.Alive)
		{
			numNeighbours++;
            //Debug.Log("Down = " + down);
		}
        int leftDown = down - 1;
		if (y > 0 && x > 0 && grids[leftDown].curStatus == Status.Alive)
		{
			numNeighbours++;
            //Debug.Log("Left down = " + leftDown);
		}
        int left = self - 1;
        if (x > 0 && grids[left].curStatus == Status.Alive)
        {
			numNeighbours++;
            //Debug.Log("Left = " + left);
        }
        int leftUp = up - 1;
        if (y < numRow - 1 && x > 0 && grids[leftUp].curStatus == Status.Alive)
		{
			numNeighbours++;
            //Debug.Log("Left up = " + leftUp);
		}

		if (curStatus == Status.Alive)
		{
			//Debug.Log("numNeighbours = " + numNeighbours);
		}

		// Each "populated" cell with one or no neighbour dies, as if by solitude 
        if (curStatus == Status.Alive && numNeighbours <= 1)
        {
            Mark(Status.Dead);
        }

		// Each "populated" cell with four or moe neighbour dies, as if by overpopulation
		if (curStatus == Status.Alive && numNeighbours >= 4)
		{
			Mark(Status.Dead);
		}

        // Each "populated" cell with two or three neighbours survives
        if (curStatus == Status.Alive && numNeighbours >= 2 && numNeighbours <= 3)
        {
            Mark(Status.Alive);
        }

		// Each "empty" cell with three neighbours becomes populated
		if (curStatus == Status.Dead && numNeighbours == 3)
        {
            Mark(Status.Alive);
        }
	}

    public void Mark(Status status)
    {
        nextStatus = status;
    }

    public void UpdateStatus()
    {
        SetStatus(nextStatus);
    }

    private void OnMouseDown()
    {
        if (curStatus != Status.Stationary) return;

        if (OnClicked != null)
        {
            Debug.Log("OnClicked. x = " + x + ", y = " + y);
            SetStatus(Status.Clicked);
            OnClicked(x, y);
        }
    }
}
