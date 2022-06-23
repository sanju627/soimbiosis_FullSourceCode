using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	public bool lockCursor;
	public float mouseSenstivity = 10;

	public Transform target;
	public float distFromTarget = 2;
	public Vector2 pitchMinMax = new Vector2 (-40, 80);

	public float rotationSmoothTime = 1.2f;

	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;

	float yaw;
	float pitch;

    float x;
    float y;

	void Start()
    {
        if(lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSenstivity + x;
        pitch -= Input.GetAxis("Mouse Y") * mouseSenstivity + y;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3 (pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distFromTarget;

    }

    public void Recoil(float x_r, float y_r)
    {
        x = x_r;
        y = y_r;
    }
}
