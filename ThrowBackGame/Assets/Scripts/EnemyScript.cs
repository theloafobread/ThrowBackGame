using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	Vector3 endPos;
	public float speed = 5.0f;
	
	void Start () 
	{
		endPos = transform.position;
	}
	
	void Update () 
	{
		transform.position = Vector3.MoveTowards(transform.position, endPos, 3*Time.deltaTime);
		if(transform.position == endPos)
		{
			endPos = new Vector3(Random.Range(-25, 25),1, Random.Range (-25, 25));
		}
		
		if (rigidbody.velocity.magnitude > speed)
			rigidbody.velocity = rigidbody.velocity.normalized * speed;
	}
}
