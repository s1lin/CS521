using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

    public IntVector2 mazeSize;

    public MazePath path;
    public MazeWall wall;
    public MazeCell cell;

    private MazeCell[,] cells;

    private int solutionSteps = 0;

    private bool[,] isMarked;
    private bool[,] correctPath;

    public MazeCell GetCell(IntVector2 coord) {
        return cells[coord.x, coord.z];
    }

    public int Generate(IntVector2 solveStart) {
        cells = new MazeCell[mazeSize.x, mazeSize.z];
        isMarked = new bool[mazeSize.x, mazeSize.z];
        correctPath = new bool[mazeSize.x, mazeSize.z];

        List<MazeCell> activeCells = new List<MazeCell>();
        CreateFirstCell(activeCells);

        while (activeCells.Count > 0)
            CreateRemainCells(activeCells);


        MazeCell startCell = GetCell(new IntVector2(0, 0));
        MazeCell endCell = GetCell(new IntVector2(mazeSize.z - 1, mazeSize.z - 1));

        Transform startWallS = startCell.transform.Find("MazeWallSouth");
        Transform endWallE = endCell.transform.Find("MazeWallEast");

        Destroy(startWallS.gameObject);
        Destroy(endWallE.gameObject);

        if (solveStart == null)
            solveStart = new IntVector2(0, 0);

        PathFinder(solveStart);
        return solutionSteps;
    }

    private MazeCell CreateCell(IntVector2 coord) {
        MazeCell newCell = Instantiate(this.cell) as MazeCell;

        cells[coord.x, coord.z] = newCell;
        newCell.coordinates = coord;
        newCell.name = "MazeCell" + coord.x + "," + coord.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition =
            new Vector3(coord.x - mazeSize.x * 0.5f + 0.5f, 0f, coord.z - mazeSize.z * 0.5f + 0.5f);

        return newCell;
    }

    public IntVector2 RandomCoordinates {
        get {
            return new IntVector2(Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.z));
        }
    }

    public bool ContainsCoordinates(IntVector2 coord) {
        return coord.x >= 0 && coord.x < mazeSize.x && coord.z >= 0 && coord.z < mazeSize.z;
    }


    private void CreateFirstCell(List<MazeCell> activeCells) {
        activeCells.Add(CreateCell(new IntVector2(0, 0)));
    }

    private void CreateRemainCells(List<MazeCell> activeCells) {
        int currentIndex = activeCells.Count - 1;
        MazeCell current = activeCells[currentIndex];

        if (current.isFullyInitialised) {
            activeCells.RemoveAt(currentIndex);
            return;
        }

        MazeDirection direction = current.RandomUninitializedDirection;
        IntVector2 nextCoord = current.coordinates + direction.ToIntVector2();

        if (ContainsCoordinates(nextCoord)) {
            MazeCell neighbour = GetCell(nextCoord);
            if (neighbour == null) {
                neighbour = CreateCell(nextCoord);
                CreatePassage(current, neighbour, direction);
                activeCells.Add(neighbour);
            } else {
                CreateWall(current, neighbour, direction);
            }
        } else {
            CreateWall(current, null, direction);
        }
    }

    private void CreatePassage(MazeCell curCell, MazeCell neighCell, MazeDirection direction) {
        MazePath passage = Instantiate(this.path) as MazePath;
        passage.Initialize(curCell, neighCell, direction);
        passage = Instantiate(this.path) as MazePath;
        passage.Initialize(neighCell, curCell, direction.GetOpposite());
    }

    private void CreateWall(MazeCell curCell, MazeCell neighCell, MazeDirection direction) {
        MazeWall wall = Instantiate(this.wall) as MazeWall;
        wall.Initialize(curCell, neighCell, direction);
        wall.name = "MazeWall" + direction;
        if (neighCell != null) {
            wall = Instantiate(this.wall) as MazeWall;
            wall.Initialize(neighCell, curCell, direction.GetOpposite());
            wall.name = "MazeWall" + direction.GetOpposite();
        }
    }

    private void PaintKeyQuad(MazeCell curCell) {
        Transform keyQuad = curCell.transform.Find("KeyQuad");
        keyQuad.gameObject.SetActive(true);

        Transform quad = curCell.transform.Find("Quad");
        quad.gameObject.SetActive(false);
        solutionSteps++;
    }

    private bool NoWestWall(MazeCell curCell) {
        return null == curCell.transform.Find("MazeWallWest");
    }
    private bool NoEastWall(MazeCell curCell) {
        return null == curCell.transform.Find("MazeWallEast");
    }
    private bool NoNorthWall(MazeCell curCell) {
        return null == curCell.transform.Find("MazeWallNorth");
    }
    private bool NoSouthWall(MazeCell curCell) {
        return null == curCell.transform.Find("MazeWallSouth");
    }

    public bool PathFinder(IntVector2 coord) {
        int x = coord.x;
        int z = coord.z;

        MazeCell curCell = GetCell(coord);

        if (x == mazeSize.x - 1 && z == mazeSize.z - 1) {
            PaintKeyQuad(curCell);
            return true;
        }

        if (isMarked[x, z]) return false;


        isMarked[x, z] = true;
        if (NoWestWall(curCell) && x != 0) 
            if (PathFinder(new IntVector2(x - 1, z)))
            { 
                correctPath[x, z] = true;
                PaintKeyQuad(curCell);
                return true;
            }
        if (NoEastWall(curCell) && x != mazeSize.x - 1)
            if (PathFinder(new IntVector2(x + 1, z)))
            {
                correctPath[x, z] = true;
                PaintKeyQuad(curCell);
                return true;
            }
        if (NoNorthWall(curCell) && z != mazeSize.z - 1)
            if (PathFinder(new IntVector2(x, z + 1)))
            {
                correctPath[x, z] = true;
                PaintKeyQuad(curCell);
                return true;
            }
        if (NoSouthWall(curCell) && z != 0)
            if (PathFinder(new IntVector2(x, z - 1)))
            {
                correctPath[x, z] = true;
                PaintKeyQuad(curCell);
                return true;
            }
        return false;
    }

}
