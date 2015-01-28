using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour 
{
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationX = 0F;
	float rotationY = 0F;
	
	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	
	
	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0F;
	
	public float frameCounter = 20;
	
	Quaternion originalRotation;
		
	PlayerController pc;
	float fieldOfView = 85;
	void Update ()
	{
		if(Input.GetMouseButton(1))
		{
			if(gameObject.tag == "MainCamera")
			{
				fieldOfView = 70;
				//enable "[V] 2x zoom text
			}
			if(Input.GetKey(KeyCode.V))
				fieldOfView = 40;
//				else if(fieldOfView == 55)
//					fieldOfView = 35;
//				else 
//					fieldOfView = 70;

			sensitivityX = 5;
			sensitivityY = 5;
		}
		else
		{
			if(gameObject.tag == "MainCamera")
			{
				fieldOfView = 85;
			}
			sensitivityX = 10;
			sensitivityY = 10;
		}
		if(gameObject.tag == "MainCamera")
			camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, fieldOfView, 5*Time.deltaTime);

		if(!pc.paused)
		{
			Screen.lockCursor = true;
			Screen.showCursor = false;
			if (axes == RotationAxes.MouseXAndY)
			{			
				rotAverageY = 0f;
				rotAverageX = 0f;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
				
				rotArrayY.Add(rotationY);
				rotArrayX.Add(rotationX);
				
				if (rotArrayY.Count >= frameCounter) {
					rotArrayY.RemoveAt(0);
				}
				if (rotArrayX.Count >= frameCounter) {
					rotArrayX.RemoveAt(0);
				}
				
				for(int j = 0; j < rotArrayY.Count; j++) {
					rotAverageY += rotArrayY[j];
				}
				for(int i = 0; i < rotArrayX.Count; i++) {
					rotAverageX += rotArrayX[i];
				}
				
				rotAverageY /= rotArrayY.Count;
				rotAverageX /= rotArrayX.Count;
				
				rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
				rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);
				
				Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
				Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
				
				transform.localRotation = originalRotation * xQuaternion * yQuaternion;
			}
			else if (axes == RotationAxes.MouseX)
			{			
				rotAverageX = 0f;
				
				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
				
				rotArrayX.Add(rotationX);
				
				if (rotArrayX.Count >= frameCounter) {
					rotArrayX.RemoveAt(0);
				}
				for(int i = 0; i < rotArrayX.Count; i++) {
					rotAverageX += rotArrayX[i];
				}
				rotAverageX /= rotArrayX.Count;
				
				rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);
				
				Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
				transform.localRotation = originalRotation * xQuaternion;			
			}
			else
			{			
				rotAverageY = 0f;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				
				rotArrayY.Add(rotationY);
				
				if (rotArrayY.Count >= frameCounter) {
					rotArrayY.RemoveAt(0);
				}
				for(int j = 0; j < rotArrayY.Count; j++) {
					rotAverageY += rotArrayY[j];
				}
				rotAverageY /= rotArrayY.Count;
				
				rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
				
				Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
				transform.localRotation = originalRotation * yQuaternion;
			}
		}
		else
		{
			Screen.lockCursor = false;
			Screen.showCursor = true;
		}
	}
	
	void Start ()
	{			
		if (rigidbody)
			rigidbody.freezeRotation = true;
		originalRotation = transform.localRotation;

		pc = GameObject.Find("Player").GetComponent<PlayerController>();
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}
}