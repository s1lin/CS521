using UnityEngine;

public class MazeCell : MonoBehaviour {
    public IntVector2 coordinates;
    private int initializedEdgeCount;

    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];


    public MazeCellEdge GetEdge(MazeDirection direction) {
        return edges[(int)direction];
    }

    public void setEdge(MazeDirection direction, MazeCellEdge edge) {
        edges[(int)direction] = edge;
        initializedEdgeCount++;
    }

    public bool isFullyInitialised {
        get {
            return initializedEdgeCount == MazeDirections.Count;
        }
    }

    public MazeDirection RandomUninitializedDirection {
        get {
            int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
            for (int i = 0; i < MazeDirections.Count; i++)
                if (edges[i] == null) {
                    if (skips == 0)
                        return (MazeDirection)i;
                    skips--;
                }

            throw new System.InvalidOperationException("MazeCell has no uninitialised left.");
        }
    }
}
