using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float dampTime;
    private Vector3 velocity = Vector3.zero;

    private Camera cam;

    void Start(){
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    public void OnUpdate(Transform target)
    {
        if(target){
            Vector3 point = cam.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }

}
