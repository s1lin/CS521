using UnityEngine;
using UnityEngine.UI;

public class FloatController : MonoBehaviour {
    public int activeObstacles;
    public int offset;
    public Vector3 obstaclesInitPosition;

    public GameObject obstaclePrefab;
    public GameObject keyPrefab;

    public Canvas canvas;

    private void Start() {
        setActiveObstacleText(activeObstacles);
        for (int i = 0; i < activeObstacles; i++) {
            GameObject newObsacle = Instantiate(obstaclePrefab, transform) as GameObject;
            newObsacle.transform.position = new Vector3(obstaclesInitPosition.x + offset * i,
                obstaclesInitPosition.y, obstaclesInitPosition.z);
            GameObject key = Instantiate(keyPrefab, newObsacle.transform) as GameObject;
            key.transform.position = newObsacle.transform.position;
        }

    }

    private void setActiveObstacleText(int active) {
        Transform countParent = canvas.transform.Find("Floating and Key");
        Transform count = countParent.GetChild(0);

        Text countText = count.GetComponent<Text>();
        countText.text = active.ToString();
    }

    private void Update() {
        setActiveObstacleText(transform.childCount);

        if (transform.childCount == 0) {
            Transform countParent = canvas.transform.Find("Floating and Key");
            Transform count = countParent.GetChild(1);
            count.gameObject.SetActive(true);
        }
    }

}
