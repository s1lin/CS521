using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public CharacterController controller;

    public GameObject player;
    public GameObject entrance;
    public GameObject endPanel;
    public GameObject flaotKey;
    public GameObject stepsLeft;

    public float moveSpeed;
    public float jumpForce;
    public float gravityScale;

    public int mazeSteps;

    private Vector3 moveDirection;

    private MazeDirection curDirection;

    private MazeCell curCell;
    private GameManager game;

    private bool isInMaze = false;
    private int countReset = 0;
    private IntVector2 coord = new IntVector2(0, 0);

    private void Move(MazeDirection direction) {
        MazeCellEdge edge = curCell.GetEdge(direction);
        if (edge is MazePath)
            SetLocation(edge.neighCell);
    }

    public void SetLocation(MazeCell cell) {
        curCell = cell;
        float y = moveDirection.y > 0 ? moveDirection.y : 0;
        transform.localPosition = new Vector3(cell.transform.position.x, y, cell.transform.position.z - 0.2f);
    }

    private void Look(MazeDirection direction) {
        transform.localRotation = direction.ToRotation();
        curDirection = direction;
    }

    public void SetIsInMaze(bool isInMaze) {
        this.isInMaze = isInMaze;
    }

    private void Start() {
        controller = GetComponent<CharacterController>();
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update() {

        while (game.solutionsteps > mazeSteps + 1 && countReset < 300) {
            enabled = false;
            if (curCell != null)
                coord = curCell.coordinates;
            game.ResetMatrix(coord);
            countReset++;
        }

        enabled = true;

        if (isInMaze) 
            if (countReset <= 300)
                game.SetPlayerLocation(this, coord);
        
        if (countReset <= 300 && game.solutionsteps < mazeSteps) 
            countReset = 0;
        
        if (Input.GetKeyDown(KeyCode.Escape)) 
            ResetOnEscape();
        
    }
    void FixedUpdate() {
        if (!isInMaze) {
            float yStore = moveDirection.y;
            moveDirection = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;

            //No jump unless on the groud
            if (controller.isGrounded && Input.GetButtonDown("Jump"))
                moveDirection.y = jumpForce;

            moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime * 2;
            controller.Move(moveDirection * Time.deltaTime);
        }

        if (mazeSteps == 0)
            stepsLeft.transform.GetChild(0).GetComponent<Text>().text =
                "You have no more Moves, press ESC to reset.";

        if (isInMaze && mazeSteps > 0) {

            if (Input.GetButtonDown("Jump"))
                moveDirection.y = jumpForce;

            moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime / 4;
            controller.Move(moveDirection * Time.deltaTime);

            game.SetPlayerLocation(this, curCell);

            if (transform.position.y <= 2)
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                    IntVector2 lastCoord = curCell.coordinates;
                    Move(curDirection);
                    IntVector2 currCoord = curCell.coordinates;

                    if (currCoord == new IntVector2(7, 7)) {
                        endPanel.SetActive(true);
                    }
                    if (currCoord != lastCoord) {
                        mazeSteps--;
                        stepsLeft.transform.GetChild(0).GetComponent<Text>().text = mazeSteps.ToString();
                        coord = curCell.coordinates;

                        game.ResetMatrix(curCell.coordinates);
                        game.SetPlayerLocation(this, currCoord);

                    }
                    Look(curDirection);

                } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                    Look(curDirection.GetOpposite());
                } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    Look(curDirection.GetNextCounterclockwise());
                } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                    Look(curDirection.GetNextClockwise());
                }
        }
    }

    private void ResetOnEscape() {
        mazeSteps = 16;
        countReset = 0;
        stepsLeft.transform.GetChild(0).GetComponent<Text>().text = mazeSteps.ToString();
        coord = new IntVector2(0, 0);

        game.ResetMatrix(coord);
        game.SetPlayerLocation(this, coord);
        endPanel.SetActive(false);

        if (isInMaze)
            Look(curDirection);
    }

    void OnTriggerEnter(Collider collisionInfo) {
        if (collisionInfo.name == "EndDoor") {
            endPanel.SetActive(true);
        }

        if (collisionInfo.name == "StartDoor") {
            Destroy(collisionInfo.gameObject.transform.parent.gameObject);
            flaotKey.SetActive(false);
            stepsLeft.SetActive(true);
            isInMaze = true;
        }
    }
}

