using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour {
    public MazeCell curCell, neighCell;
    public MazeDirection direction;

    public virtual void Initialize(MazeCell curCell, MazeCell neighCell, MazeDirection direction) {
        this.curCell = curCell;
        this.neighCell = neighCell;
        this.direction = direction;

        curCell.setEdge(direction, this);

        transform.parent = curCell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }

    public virtual void OnPlayerEntered() { }

    public virtual void OnPlayerExited() { }

}
