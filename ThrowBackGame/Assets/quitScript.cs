using UnityEngine;
using System.Collections;

public class quitScript : MonoBehaviour {
	
	void Start () 
	{
		
	}

	void Update () 
	{
		
	}
	void OnMouseDown()
	{
		print ("Quiting");
		Application.Quit();
	}
}
