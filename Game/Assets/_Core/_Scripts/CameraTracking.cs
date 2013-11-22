using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour {
    public Transform target;
    public float distance = 3.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    public bool smoothRotation = false;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;

    void LateUpdate () {
		if (target != null) {
           transform.position = Vector3.Lerp (transform.position, new Vector3(target.position.x, target.position.y, -10.0f), Time.deltaTime * damping);
		}
     }
}