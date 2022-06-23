using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public enum ControlType{KeyBoard = 1, Touch = 2};

public class InputSystem : MonoBehaviour
{
	public ControlType control;
	public float Accel;
	public float Steer;
	public float Brake;
	public float reverseSpeed;
	public GameObject UI;
    public FixedJoystick joyStick;
	public void AccelInput(float input){Accel = input;}
	public void SteerInput(float input){Steer = input;}
	public void BrakeInput(float input){Brake = input;}

	CarController car;

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(control == ControlType.KeyBoard)
        {
        	// Accel = Input.GetAxis("Vertical");
        	// Steer = Input.GetAxis("Horizontal");
        	// Brake = Input.GetAxis("Jump");
        	//UI.SetActive(false);
        }else
        {
        	//UI.SetActive(true);
        }
    }

    void FixedUpdate()
    {
    	car.Move(joyStick.Horizontal + Input.GetAxis("Horizontal"), joyStick.Vertical + Input.GetAxis("Vertical"), joyStick.Vertical + Input.GetAxis("Vertical"), 0);
    }
}
