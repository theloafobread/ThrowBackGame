using UnityEngine;
using System.Collections;

public class backAndForthPlatform : MonoBehaviour {

	// Use this for initialization
	Vector3 endPos;
	bool goLeft = false;
	void Start () {
		endPos = new Vector3(-13, 1, -9);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = Vector3.MoveTowards(transform.position, endPos, 5*Time.deltaTime);
		if(transform.position == endPos)
		{
			goLeft = !goLeft;
		}
		if(goLeft)
			endPos = new Vector3(-13,1, -9);
		else
			endPos = new Vector3(13, 1, -9);
	}
}
