using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	int speedLimit = 5;
	float speed = 10000;
	public bool doubleJumped = false;
	public bool paused = false;
	public bool grounded = false;
	public bool thirdPerson = true;
	public Text pausedText;
	public Text infoText;
	public Text quitText;
	public Camera cam;

	float timer = 0;
	float rateOfFire = .5f;
	void Start () 
	{
		
	}

	void OnCollisionEnter(Collision c)
	{
		print("In contact with " + c.transform.name);
		if(c.collider.tag == "Ground" || c.collider.tag == "Cube")
		{
			grounded = true;
			doubleJumped = false;
			speed = 10000;
		}
		if(c.collider.tag == "Ramp")
		{
			print ("RAMP");
			grounded = true;
			doubleJumped = false;
			speed = 10000;
		}
//		if(c.collider.tag == "Ledge")
//		{
//			moveUpLedge();
//		}
	}
	void OnCollisionStay (Collision c) 
	{ 
		if(c.gameObject.tag == "MovingPlatform")
		{
			transform.parent = c.transform;
			grounded = true;
			doubleJumped = false;
			speed = 10000;
		}
		else
			transform.parent = null;
	}
	void OnCollisionExit(Collision c)
	{
		if(c.collider.tag == "Ground")
			//grounded = false;
		print("No longer in contact with " + c.transform.name);
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
			thirdPerson = !thirdPerson;

		if(thirdPerson)
			cam.transform.localPosition = new Vector3(0,2.21f,-3.7f);
		else
			cam.transform.localPosition = new Vector3(0, .72f, .27f);

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
			pausedText.gameObject.SetActive(true);
			quitText.gameObject.SetActive(true);
		}
		else
		{
			Time.timeScale = 1;
			pausedText.gameObject.SetActive(false);
			quitText.gameObject.SetActive(false);
			//hide pause menu
		}

		//Shooting-------
		//add timer for shooting delay
		if(Input.GetMouseButton(0))
		{
			timer+=1*Time.deltaTime;
			Debug.DrawRay(cam.transform.position, cam.transform.forward * 100f, Color.red); // for detecting what's in front, pointed forward
			if(timer > rateOfFire)
			{
				RaycastHit hit;
				Debug.DrawRay(cam.transform.position, cam.transform.forward * 100f, Color.green); // for detecting what's in front, pointed forward
				if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, 100 )) 
				{

					print("Hit: "+hit.collider.name + " : "+hit.distance);
					if(hit.collider.tag == "Enemy")
					{
						hit.collider.gameObject.SetActive(false);
					}
				}
				timer = 0;
			}
		}
		if(Input.GetMouseButtonUp(0))
		{
			timer = rateOfFire;
		}
	}
	void FixedUpdate()
	{
 		RaycastHit hit;
		Debug.DrawRay(transform.position, transform.forward * 1.2f, Color.blue); // for detecting what's in front, pointed forward
		
		if (Physics.Raycast (transform.position, transform.forward, out hit, 1.2f )) 
		{
			if(hit.collider.tag == "Ledge")
			{
				print("LEDGE ------ ");
				grounded = true;
				doubleJumped = false;
				speed = 10000;
				//Vector3 ledgeTop = new Vector3(transform.position.x, transform.position.y+25, transform.position.z);
				//transform.Translate(Vector3.up*35*Time.deltaTime);
				rigidbody.velocity = Vector3.zero;
				rigidbody.AddExplosionForce(1500, new Vector3(transform.position.x, transform.position.y-1, transform.position.z), 5);
			}
		}
	}
}
