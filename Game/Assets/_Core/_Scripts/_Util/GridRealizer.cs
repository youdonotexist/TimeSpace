using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridRealizer
{
	List<Cell> potentialShellPieces = new List<Cell>();
	public List<Cell> RealizeShell(Cell[] cells, int gridAcross, int gridDown) {

		/**
		 * Loop through rows
		 */
		LeftToRight(cells, gridAcross, gridDown);
		RightToLeft(cells, gridAcross, gridDown);
		TopToBottom(cells, gridAcross, gridDown);
		BottomToTop(cells, gridAcross, gridDown);

		return potentialShellPieces;

	}

	public List<Cell> RealizeAllShell(Cell[] cells) {
		List<Cell> edges = new List<Cell>();
		for (int i = 0; i < cells.Length; i++) { 
			if (cells[i].cellState == Cell.CellState.EXTERIOR_WALL) {
				edges.Add (cells[i]);
			}
		}

		return edges;
	}

	public List<Cell>[] RealizeShellFill(Cell[] cells, Cell center) {
		Stack<Cell> fillStack = new Stack<Cell>();
		List<Cell> shell = new List<Cell>();
		List<Cell> interior = new List<Cell>();
		fillStack.Push(center);
		while (fillStack.Count > 0) {
			Cell n = fillStack.Pop();
			if (n.cellState == Cell.CellState.OPEN || n.cellState == Cell.CellState.CENTER) {
				n.SetCellState(Cell.CellState.INTERIOR_WALL);
				interior.Add(n);
				Cell[] surrounding = n.SurroundingCells();
				foreach (Cell c in surrounding) {
					if (c != null) {
						if (c.cellState == Cell.CellState.EXTERIOR_WALL) {
							shell.Add(c);
						}
						else if (c.cellState == Cell.CellState.OPEN) {
							fillStack.Push(c);
						}
					}
				}
			}
		}

		List<Cell>[] things = new List<Cell>[2];
		things[0] = shell;
		things[1] = interior;
		return things;
	}

	public List<Cell> RealizeInterior(List<Cell> cells, List<Cell> edge) {
		List<Cell> interior = new List<Cell>();
		for (int i = 0; i < cells.Count; i++) { 
			Cell c = cells[i];
			if (c.cellState != Cell.CellState.EXTERIOR_WALL) {
				if (PointInsideCellGroup(edge, c)) {
					interior.Add(c);
					c.SetCellColor(Color.red);
				}
			}
		}

		return interior;
	}

	void LeftToRight(Cell[] cells, int gridAcross, int gridDown) {
		int currentRow = 0;
		for (int i = 0; i < cells.Length; i++) { 
			if (Mathf.Floor (i / gridAcross) != currentRow) {
				currentRow++;
				i = currentRow * gridAcross;
			}
			else {
				Cell c = cells[i];

				if (potentialShellPieces.Contains(c)) {
					currentRow++;
					i = currentRow * gridAcross;
					i--;
				}
				
				else if (c.cellState == Cell.CellState.EXTERIOR_WALL) {//If we gots a wall and it's not DIRTAY
					potentialShellPieces.Add(c);
					currentRow++;
					i = currentRow * gridAcross;
					i--;
				}
			}
		}
	}

	void RightToLeft(Cell[] cells, int across, int down) {
		int currentRow = down - 1;
		for (int i = cells.Length - 1; i >= 0 ; i--) { 
			if (Mathf.Floor (i / across) != currentRow) {
				currentRow--;
				i = (currentRow * across) + across;
			}
			else {
				//Look at dis cell
				Cell c = cells[i];

				if (potentialShellPieces.Contains(c)) {
					currentRow--;
					i = (currentRow * across) + across;
				}

				if (c.cellState == Cell.CellState.EXTERIOR_WALL) {//If we gots a wall and it's not DIRTAY
					potentialShellPieces.Add(c);
					currentRow--;
					i = (currentRow * across) + across;
				}
			}
		}
	}

	void TopToBottom(Cell[] cells, int across, int down) {
		for (int column = 0; column < across; column++) {
			for (int row = 0; row < down; row++) {
				int i = column + (row * across);
				Cell c = cells[i];

				if (potentialShellPieces.Contains(c)) {
					break;
				}
				
				if (c.cellState == Cell.CellState.EXTERIOR_WALL) {//If we gots a wall and it's not DIRTAY
					potentialShellPieces.Add(c);
					break;
				}
			}
		}
	}

	void BottomToTop(Cell[] cells, int across, int down) {
		for (int column = 0; column < across; column++) {
			for (int row = down-1; row >= 0; row--) {
				int i = column + (row * across);
				Cell c = cells[i];

				if (potentialShellPieces.Contains(c)) {
					break;
				}

				if (c.cellState == Cell.CellState.EXTERIOR_WALL) {//If we gots a wall and it's not DIRTAY
					potentialShellPieces.Add(c);
					break;
				}
			}
		}
	}

	bool PointInsideCellGroup(List<Cell> cells, Cell testCell) {
		bool c = false;
		for (int i = 0, j = cells.Count-1; i < cells.Count; i++) {
			float 	CellXi = cells[i].GetXPos (),
					CellYi = cells[i].GetYPos (),
					CellXj = cells[j].GetXPos (),
					CellYj = cells[j].GetYPos (),
					
					TestX = testCell.GetXPos(),
					TestY = testCell.GetYPos();


			if ((CellYi < TestY && CellYj >= TestY || CellYj < TestY && CellYi >= TestY) &&
			    	(CellXi <= TestX || CellXj <= TestX)) {

		    	if (CellXi + (TestY - CellYi) / (CellYj - CellYi) * (CellXj - CellXi) < TestX) {
					c = !c;
				}
			}
			j = i;
		}
		return c;
	}
}

