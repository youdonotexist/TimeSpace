using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Grid : MonoBehaviour
{
	public int gridAcross = 5;
	public int gridDown = 3;

	float tempSize = 1.0f;

	public LayerMask NodeLayer;

	public Cell cellPrefab;
	Cell[] cells;
	Cell.CellState cellState = Cell.CellState.EXTERIOR_WALL;

	public int[] _stateCounts; 
	public int[] _stateMaxCount; 

	class CellData {
		float x;
		float y;
		Cell.CellState state;

		public CellData(float x, float y, Cell.CellState s) {
			this.x = x;
			this.y = y;
			this.state = s;
		}
	}

	void Awake() {
		_stateCounts = new int[(int) Cell.CellState.MAX];
		_stateMaxCount = new int[(int) Cell.CellState.MAX];

		Messenger.AddListener("exteriorWallsEvent", ExteriorWalls);
		Messenger.AddListener("interiorWallsEvent", InteriorWalls);
		Messenger.AddListener("thrustersEvent", Thrusters);
		Messenger.AddListener("outEvent", OutEvent);

		_stateMaxCount[(int)Cell.CellState.EXTERIOR_WALL] = 200;
		_stateMaxCount[(int)Cell.CellState.INTERIOR_WALL] = int.MinValue;
		_stateMaxCount[(int)Cell.CellState.THRUSTER] = 6;
		_stateMaxCount[(int)Cell.CellState.OPEN] = int.MinValue;
	}

	// Use this for initialization
	void Start () {
		cells = new Cell[gridAcross * gridDown];
		BuildGrid ();	
		CenterCell().SetCellState(Cell.CellState.CENTER);
	}

	public Cell CenterCell() {
		return cells[CenterIndex()];
	}

	public Cell GetCellAtIndex(int index) {
		if (index >= 0 && index < cells.Length) {
			return cells[index];
		}

		return null;
	}

	int CenterIndex() {
		float midish = ((float) (gridAcross * gridDown)) * 0.5f;
		float acroset = ((float) gridAcross) * 0.5f;
		return Mathf.FloorToInt(midish) + Mathf.FloorToInt(acroset);
	}

	void ExteriorWalls() {
		cellState = Cell.CellState.EXTERIOR_WALL;
	}

	void InteriorWalls() {
		cellState = Cell.CellState.INTERIOR_WALL;
	}

	void Thrusters() {
		cellState = Cell.CellState.THRUSTER;
	}

	void FixedUpdate() {
		if (Input.GetMouseButton(0)) {
			if (Input.GetKey(KeyCode.LeftShift)) {
				Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); worldPos.z = 0.0f;
				Cell c = WorldPointToCell(worldPos);
				Debug.Log ("Debug Pos: (" + c.GetX() + "," + c.GetY () + ") OR (" + c.GetXPos() + "," + c.GetYPos() + ")");
			}
			else {
				Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); worldPos.z = 0.0f;
				Cell c = WorldPointToCell(worldPos);
				UpdateCellState(c, cellState);
			}
		}

		if (Input.GetMouseButton(1)) {
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); worldPos.z = 0.0f;
			Cell c = WorldPointToCell(worldPos);
			UpdateCellState(c, Cell.CellState.OPEN);
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			OutEvent();
		}
	}

	void UpdateCellState(Cell cell, Cell.CellState state) {
		int toState = _stateMaxCount[(int) state];
		int fromState = _stateMaxCount[(int) cell.cellState];

		int toStateCurrent = _stateCounts[(int) state];
		int fromStateCurrent = _stateCounts[(int) cell.cellState];

		if (toState == int.MinValue || toStateCurrent < toState - 1) { //We can add whatever
			_stateCounts[(int) state]++;
			_stateCounts[(int) cell.cellState]--;
			cell.SetCellState(state);
		}
	}

	Cell WorldPointToCell(Vector3 worldPos) {
		float x = Mathf.Floor( worldPos.x / ((float)tempSize) );
		float y = Mathf.Floor( worldPos.y / ((float)tempSize) );
		
		int index = (int) (x + (y * gridAcross));
		
		Cell c = cells[index];
		return c;
	}

	void BuildGrid() {
		for (int i = 0; i < gridAcross * gridDown; i++) {
			Cell go = (Cell) Instantiate(cellPrefab);
			cells[i] = go;
			go.cellIndex = i;
			go._grid = this;

			Vector2 size = go.GetComponent<BoxCollider2D>().size;
			float x = ((i % gridAcross) * size.x) + (size.x * 0.5f);
			float y = ((Mathf.Floor (i / gridAcross)) * size.y) + (size.x * 0.5f);
			go.transform.position = new Vector3(x, y, 0.0f);

			go.transform.parent = transform;
			tempSize = size.x;
		}

		FocusCameraOnGameObject(Camera.main, gameObject);
	}
	
	void FocusCameraOnGameObject(Camera c, GameObject go) {
		Bounds b = CalculateBounds(go);
		Vector3 max = b.size;
		c.orthographicSize = max.y * 0.5f;
		
		Vector3 pos = c.transform.position;
		pos.x = (gridAcross) * 0.5f * tempSize;
		pos.y = (gridDown) * 0.5f * tempSize;
		c.transform.position = pos;
	}

	Bounds CalculateBounds(GameObject go) {
		Bounds b = new Bounds(go.transform.position, Vector3.zero);
		Object[] rList = go.GetComponentsInChildren(typeof(Renderer));
		foreach (Renderer r in rList) {
			b.Encapsulate(r.bounds);
		}
		return b;
	}

	void OutEvent() {
		GridRealizer gs = new GridRealizer();
		//List<Cell> edgeCells = gs.RealizeShell(cells, gridAcross, gridDown);
		//List<Cell> edgeCells = gs.RealizeAllShell (cells);
		//edgeCells = SortCounterClockwise2(edgeCells);
		List<Cell>[] things = gs.RealizeShellFill(cells, CenterCell());
		List<Cell> edgeCells = things[0];
		List<Cell> interiorCells = things[1];
		//StartCoroutine (Test(edgeCells));

		//Swap the rest back to open
		List<Cell> allCells = new List<Cell>(cells);
		IEnumerable<Cell> trashCells = allCells.Except(edgeCells);
		trashCells = trashCells.Except(interiorCells);
		Cell[] tcells = trashCells.ToArray();
		for (int i = 0; i < tcells.Length; i++) {
			Cell c = tcells[i];
			c.SetCellState(Cell.CellState.OPEN);
		}

		Debug.Log("Did it" + Time.time);

		GridSerializer gridSerializer = new GridSerializer();
		Hashtable s = gridSerializer.Serialize(this, gridAcross, gridDown);
		CI.Request req = new CI.Request();
		//req.POST("http://spacetime.aws.af.cm/ship/", s);
		req.POST("http://spacetime.aws.af.cm/ship", s);

	}

	IEnumerator Test(List<Cell> l) {
		int index = 0;
		while (index < l.Count) {
			if (Input.GetKeyDown(KeyCode.G)) {
				l[index].SetCellColor(Color.magenta);
				index++;
			}

			yield return null;
		}
	}

	List<Cell> SortCounterClockwise(List<Cell> edge) {
		List<Cell> sorted = new List<Cell>();
		List<Cell> localEdge = new List<Cell>(edge);

		Debug.Log ("In: " + localEdge.Count);

		//Find the left-most and right-most points
		Cell minx = edge[0], maxx = edge[0];
		for (int i = 1; i < edge.Count; i++) {
			Cell c = edge[i];

			if (c.GetX() > maxx.GetX()) {
				maxx = c;
			}

			if (c.GetX() < minx.GetX()) {
				minx = c;
			}
		}

		localEdge.Remove(minx);
		localEdge.Remove(maxx);

		Debug.Log ("Adding: ");
		Debug.Log (minx.ToString());
		Debug.Log (maxx.ToString());

		//Find all points below the line LM-RM
		List<Cell> lower = new List<Cell>();
		lower.Add(minx);
		for (int i = 0; i < localEdge.Count; i++) {
			Cell c = localEdge[i];
			float cross = ((maxx.GetX() - minx.GetX()) * (c.GetY() - minx.GetY()) - (maxx.GetY() - minx.GetY()) * (c.GetX() - minx.GetX()));
			if (cross < 0) {
				//Sort the points beneath LM-RM left to right
				bool inserted = false;
				for (int j = 1; j < lower.Count; j++) {
					Cell cj = lower[j];
					if (c.GetX () < cj.GetX ()) {
						lower.Insert(j, c);
						inserted = true;
						break;
					}
					else if (c.GetX () == cj.GetX ()) {
						if (c.GetY() < cj.GetY()) {
							lower.Insert (j, c);
							inserted = true;
							break;
						}
						else {
							lower.Insert (j+1, c);
							inserted = true;
							break;
						}
					}
				}

				if (!inserted) {
					lower.Add(c);
				}
			}
		}
		lower.Add(maxx);

		//Remove the lowers from the localEdge list
		for (int i = 0; i < lower.Count; i++) {
			localEdge.Remove(lower[i]);
		}


		//Now sort the points above from right to left
		List<Cell> upper = new List<Cell>();
		upper.Add (localEdge[0]);
		for (int i = 1; i < localEdge.Count; i++) {
			Cell c = localEdge[i];
			bool inserted = false;
			for (int j = 0; j < upper.Count; j++) {
				Cell cj = upper[j];
				if (c.GetX () > cj.GetX ()) {
					upper.Insert(j, c);
					inserted = true;
					break;
				}
				else if (c.GetX() == cj.GetX()) {
					if (c.GetY() > cj.GetY()) {
						upper.Insert (j, c);
						inserted = true;
						break;
					}
					else {
						upper.Insert (j+1, c);
						inserted = true;
						break;
					}
				}
			}
		}

		for (int j = 0; j < upper.Count; j++) {
			lower.Add (upper[j]);
		}

		Debug.Log ("Out: " + lower.Count);
		return lower;
	}

	List<Cell> SortCounterClockwise2(List<Cell> edge) {
		Vector2 center = centroid(edge);

		for (int i = 0; i < edge.Count; i++) {
			Cell cell = edge[i];
			cell._angleFromCenter = Mathf.Atan2(cell.GetY() - center.y, cell.GetX() - center.x);
		}

		edge.Sort((x,y) => x._angleFromCenter.CompareTo(y._angleFromCenter));
		return edge;

	}

	Vector2 centroid(List<Cell> cells)  {
		float centroidX = 0, centroidY = 0;
		
		for(int i = 0; i < cells.Count; i++) {
			Cell cell = cells[i];
			centroidX += cell.GetX();
			centroidY += cell.GetY();
		}
		return new Vector2(centroidX / cells.Count, centroidY / cells.Count);
	}
}

