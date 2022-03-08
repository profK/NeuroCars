using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [HideInInspector]
    public Transform cameraTarget;
    [HideInInspector]
    public float sSpeed = 10.0f;
    [HideInInspector]
    public Vector3 dist;
    [HideInInspector]
    public Transform lookTarget;

    public void Start()
    {
        this.transform.GetComponent<Camera>().nearClipPlane = 0.2f;
    }

    void FixedUpdate()
    {
        Vector3 dPos = cameraTarget.position + dist;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, sSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.position);
    }
}
