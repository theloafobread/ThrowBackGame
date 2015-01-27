using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	int speedLimit = 5;
	float speed = 10000;
	float tilt = 0;
	public bool doubleJumped = false;
	public bool paused = false;
	public bool grounded = false;
	public bool thirdPerson = false;
	public Text pausedText;
	public Text infoText;
	public Camera cam;

	void Start () 
	{
		
	}

	void OnCollisionEnter(Collision c)
	{
		print("In contact with " + c.transform.name);
		if(c.collider.tag == "Ground")
		{
			grounded = true;
			doubleJumped = false;
			speed = 10000;
		}
	}
	void OnCollisionExit(Collision c)
	{
		if(c.collider.tag == "Ground")
			grounded = false;
		print("No longer in contact with " + c.transform.name);
	}
	void OnCollisionStay (Collision c) 
	{ 
		if(c.gameObject.tag == "Cube")
		{
			//transform.position = new Vector3(c.collider.transform.position.x, c.collider.transform.position.y+1, c.collider.transform.position.z);
			transform.parent = c.transform;
		}
		else
			transform.parent = null;
	}
	void Update () 
	{
		infoText.text = rigidbody.velocity.magnitude.ToString("f2")+"\n ";

		if(Input.GetKey(KeyCode.LeftShift))
		{
			speedLimit = 10;
		}
		else
		{
			speedLimit = 5;
		}

		if(Input.GetKey(KeyCode.W) && grounded)
			rigidbody.AddForce(transform.forward*speed*Time.deltaTime);
		if(Input.GetKey(KeyCode.A) && grounded)
			rigidbody.AddForce(transform.right*-speed*Time.deltaTime);
		if(Input.GetKey(KeyCode.S) && grounded)
			rigidbody.AddForce(transform.forward*-speed*Time.deltaTime);
		if(Input.GetKey(KeyCode.D) && grounded)
			rigidbody.AddForce(transform.right*speed*Time.deltaTime);

		if((Input.GetKeyDown(KeyCode.Space) && grounded) || (Input.GetKeyDown(KeyCode.Space) && !grounded && !doubleJumped))
		{
			if(!grounded && !doubleJumped)
				doubleJumped = true;
			rigidbody.AddExplosionForce(1800, new Vector3(transform.position.x, transform.position.y-1, transform.position.z), 5);
			speed = 4000;
		}
		if(Input.GetKeyDown(KeyCode.T))
			//cam.transform.localPosition = new Vector3(1,0,0);
			thirdPerson = !thirdPerson;
		if(thirdPerson)
		{
			//cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x,cam.transform.localEulerAngles.y,cam.transform.localEulerAngles.z);
			cam.transform.localPosition = new Vector3(0,2.21f,-3.7f);
		}
		else
		{
			//cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x,cam.transform.localEulerAngles.y,cam.transform.localEulerAngles.z);
			cam.transform.localPosition = new Vector3(0, .72f, .27f);
		}


			//rigidbody.AddForce(transform.up*speed*Time.deltaTime);

//		if (rigidbody.velocity.magnitude > speedLimit)
//			rigidbody.velocity = rigidbody.velocity.normalized * speedLimit;

		Vector3 clampVelx = rigidbody.velocity;
		clampVelx.x = Mathf.Clamp(clampVelx.x, -speedLimit, speedLimit);
		rigidbody.velocity = clampVelx;
		Vector3 clampVelz = rigidbody.velocity;
		clampVelz.z = Mathf.Clamp(clampVelz.z, -speedLimit, speedLimit);
		rigidbody.velocity = clampVelz;

		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
			paused = !paused;
		if(paused)
		{
			Time.timeScale = 0;
			//show pause menu
			pausedText.text = "Paused";
		}
		else
		{
			Time.timeScale = 1;
			pausedText.text = "";
			//hide pause menu
		}
	}
	void FixedUpdate()
	{
		RaycastHit hit;
		Debug.DrawRay(transform.position, -transform.up * 1.2f, Color.green);
		if (Physics.Raycast (transform.position,  -transform.up, out hit, 1.2f )) 
		{
			//Debug.Log(hit.collider.tag+" : "+hit.distance+" : "+grounded);
			if(hit.collider.tag == "Cube")
			{
				grounded = true;
				doubleJumped = false;
				speed = 10000;
				transform.parent = hit.transform;
				print("Parented with: "+transform.parent.name);
				//transform.position = Vector3.MoveTowards(transform.position, new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.transform.position.y+1, hit.collider.gameObject.transform.position.z), 5*Time.deltaTime);
			}
		}
	}
}
