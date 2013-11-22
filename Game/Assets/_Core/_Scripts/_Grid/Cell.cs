using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{
	SpriteRenderer _renderer;
	public Grid _grid;
	// Use this for initialization

	public Sprite _topLeft;
	public Sprite _topRight;
	public Sprite _bottomLeft;
	public Sprite _bottomright;

	public enum CellState {
		OPEN,
		EXTERIOR_WALL,
		INTERIOR_WALL,
		THRUSTER,
		CENTER,
		FLOOR,
		MAX
	}

	public enum Direction {
		UP,
		DOWN,
		LEFT,
		RIGHT
	}

	public bool dirty = false;
	public int cellIndex = 0;
	public float _angleFromCenter;

	public CellState cellState = CellState.OPEN;

	void Awake ()
	{
		_renderer = GetComponent<SpriteRenderer>();
	}

	public void SetCellColor(Color c) {
		_renderer.color = c;
	}

	public void SetCellState(CellState state) {
		cellState = state;
		if (cellState == CellState.EXTERIOR_WALL) {
			_renderer.color = Color.black;
		}
		else if (cellState == CellState.INTERIOR_WALL) {
			_renderer.color = Color.blue;
		}
		else if (cellState == CellState.THRUSTER) {
			_renderer.color = Color.yellow;
		}
		else if (cellState == CellState.CENTER) {
			_renderer.color = Color.red;	
		}
		else if (cellState == CellState.MAX) {
			_renderer.color = Color.magenta;
		}
		else {
			_renderer.color = Color.white;
		}
	}

	public float GetXPos() {
		return transform.position.x;
	}

	public float GetYPos() {
		return transform.position.y;
	}

	public float GetX() {
		return ( cellIndex % _grid.gridAcross );
	}

	public float GetY() {
		return Mathf.Floor ( ((float)cellIndex) / ((float)_grid.gridAcross) );
	}

	public string ToString() {
		return "(" + GetX() + "," + GetY() + ")";
	}

	public Cell[] SurroundingCells() {
		Cell[] surr = new Cell[4];
		surr[(int) Direction.UP] = _grid.GetCellAtIndex(cellIndex + _grid.gridAcross);
		surr[(int) Direction.DOWN] = _grid.GetCellAtIndex(cellIndex - _grid.gridAcross);
		surr[(int) Direction.LEFT] = _grid.GetCellAtIndex(cellIndex - 1);
		surr[(int) Direction.RIGHT] = _grid.GetCellAtIndex(cellIndex + 1);
		return surr;
	}
}

