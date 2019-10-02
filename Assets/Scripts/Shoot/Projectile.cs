using UnityEngine;

public class Projectile : MonoBehaviour {
    public GameObject projectile;
    public float live = 5f;
    // Update is called once per frame

    private void Start() {
        Destroy(projectile, live);
    }

    void Update() {
        if (projectile.transform.position.y < -5)
            Destroy(projectile.gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "obstacle" || collision.gameObject.tag == "end")
            Destroy(projectile.gameObject);
    }

}
