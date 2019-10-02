using UnityEngine;

public class Shoot : MonoBehaviour {
    public Projectile projectile;

    private GameObject projectileInstnace;

    void Update() {
        //use the left mouse to click shoot
        if (Input.GetMouseButtonDown(0)) {
            Destroy(projectileInstnace);
            //Get main camera direction
            Camera camera = Camera.main;
            Vector3 shootDir = camera.transform.forward;

            //Generate the projectile
            GameObject pShoot = Instantiate(projectile.gameObject, camera.transform.position, Quaternion.identity, null);
            pShoot.transform.position += new Vector3(0f, 0.5f, 0f);

            //Add Rigidbody property
            Rigidbody pShootRB = pShoot.GetComponent<Rigidbody>();
            shootDir.y += 0.3f;
            pShootRB.AddForce(shootDir * 300);

            projectileInstnace = pShoot;
        }

    }


}
