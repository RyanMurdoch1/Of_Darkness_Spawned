using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;

    private void FixedUpdate()
    {
        transform.position = new Vector3(followTransform.position.x, followTransform.position.y, transform.position.z);
    }
}
