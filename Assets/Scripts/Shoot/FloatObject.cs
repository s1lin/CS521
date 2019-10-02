using UnityEngine;

public class FloatObject : MonoBehaviour {
    public GameObject obstaclePrefab;
    public GameObject keyPrefab;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "bullet") {
            obstaclePrefab.GetComponent<Rigidbody>().useGravity = true;
            if (transform.parent.childCount == 1) {
                Transform keyInside = obstaclePrefab.gameObject.transform.GetChild(0);
                obstaclePrefab.gameObject.transform.DetachChildren();
                Destroy(obstaclePrefab.gameObject);
                keyInside.gameObject.SetActive(true);
            } else {
                Destroy(obstaclePrefab.gameObject);
            }

        }

    }
}
