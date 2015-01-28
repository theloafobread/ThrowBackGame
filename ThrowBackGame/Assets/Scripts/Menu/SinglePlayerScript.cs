using UnityEngine;
using System.Collections;

public class SinglePlayerScript : MonoBehaviour 
{
	public bool startScene = false;
	float timer = 0;
	void Start () {
	
	}

	void Update () 
	{
		if(startScene)
		{
			timer += 1*Time.deltaTime;
			if(timer > 1)
				Application.LoadLevel(1);
		}
		else
			timer = 0;
	}
	void OnMouseDown()
	{
		startScene = true;
	}
}
