using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Maze mazePrefab;
    private Maze mazeInstance;

    public int solutionsteps;

    void Start() {
        Begin();
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("A1");
        if (Input.GetKeyDown(KeyCode.Q))
            Application.Quit();
    }

    private void Begin() {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        solutionsteps = mazeInstance.Generate(new IntVector2(0, 0));
    }

    public void SetPlayerLocation(PlayerController player, MazeCell cell) {
        if (cell == null)
            player.SetLocation(mazeInstance.GetCell(new IntVector2(0, 0)));
        else
            player.SetLocation(cell);
    }

    public void SetPlayerLocation(PlayerController player, IntVector2 coord) {
        player.SetLocation(mazeInstance.GetCell(coord));
    }

    public void ResetMatrix(IntVector2 solveStart) {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        mazeInstance = Instantiate(mazePrefab) as Maze;
        solutionsteps = mazeInstance.Generate(solveStart);
    }
}
