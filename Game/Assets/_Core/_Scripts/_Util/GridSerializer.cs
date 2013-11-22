using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class GridSerializer
{
	List<Cell> currentGroup = null;
	List<List<Cell>> groups = new List<List<Cell>>();

	void PushPotentialGroup() {
		if (currentGroup != null && currentGroup.Count > 1) {
			for (int i = 0; i < currentGroup.Count; i++) {
				Cell c = currentGroup[i];
				c.dirty = true;
			}

			groups.Add(currentGroup);
		}

		currentGroup = null;
	}

	public string SerializeToString(Grid grid, int gridAcross, int gridDown) {
		return Json.Serialize(Serialize(grid, gridAcross, gridDown));
	}

	public Hashtable Serialize(Grid grid, int gridAcross, int gridDown) {
		Cell centerCell = grid.CenterCell();
		Hashtable root = new Hashtable();
		List<Hashtable> all = new List<Hashtable>();
		root.Add ("cells", all);
		for (int i = 0; i < gridAcross * gridDown; i++) {
			Cell c = grid.GetCellAtIndex(i);
			if (c.cellState == Cell.CellState.EXTERIOR_WALL || c.cellState == Cell.CellState.INTERIOR_WALL) {
				Hashtable cellTable = new Hashtable();
				cellTable.Add("X", c.GetX() - centerCell.GetX());
				cellTable.Add("Y", c.GetY() - centerCell.GetY());
				cellTable.Add("type", (int) c.cellState);
				all.Add(cellTable);
			}
		}
		

		return root;
	}
}

