using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleOutPos : MonoBehaviour
{
    public Transform vehicle;

	public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = vehicle.position + offset;
    }
}
