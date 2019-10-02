using UnityEngine;

public class Key : MonoBehaviour {
    public GameObject keyPrefab;
    public GameObject arrow;
    public GameObject door;
    public GameObject entranceText;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Terrain") {
            keyPrefab.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player") 
            Destroy(keyPrefab);        
    }

    private void OnDestroy() {
        GameObject arrowGO = Instantiate(arrow, new Vector3(-6.96f, 5f, 30f), arrow.transform.rotation) as GameObject;
        arrowGO.SetActive(true);
        arrowGO.GetComponent<MeshRenderer>().enabled = true;

        Instantiate(door, new Vector3(-6.7f, -1.1f, 30.94f), door.transform.rotation);
        Instantiate(entranceText);
    }
}

