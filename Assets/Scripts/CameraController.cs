using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform player;
    public Transform puppet;

    public Vector3 offset;

    public float rotateSpeed;

    void Start() {
        offset = player.position - transform.position;
        puppet.transform.position = player.transform.position;
        puppet.transform.parent = player.transform;

        //Hide Mouse
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        //Get the X;
        float h = Input.GetAxis("Mouse X") * rotateSpeed;
        player.Rotate(0, h, 0);

        float v = Input.GetAxis("Mouse Y") * rotateSpeed;
        puppet.Rotate(-v, 0, 0);

        //no snapping
        if (puppet.rotation.eulerAngles.x > 60f && puppet.rotation.eulerAngles.x < 180f)
            puppet.rotation = Quaternion.Euler(60, 0, 0);

        if (puppet.rotation.eulerAngles.x > 180f && puppet.rotation.eulerAngles.x < 300f)
            puppet.rotation = Quaternion.Euler(300, 0, 0);

        //new look at position
        float y = player.eulerAngles.y;
        float x = puppet.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(x, y, 0);

        //set camera to the same place with player and rotate the camera
        transform.position = player.position - (rotation * offset);
        transform.rotation = transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);

        //Set Y never below 0
        if (transform.position.y < player.position.y)
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);

        transform.LookAt(player);
    }
}