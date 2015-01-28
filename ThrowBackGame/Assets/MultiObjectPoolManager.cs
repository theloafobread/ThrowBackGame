using UnityEngine;
using System.Collections;
using System.Collections.Generic; 		// must be added to create Lists ex:  List<GameObject> objects;

public class MultiObjectPoolManager : MonoBehaviour {

	//I shortened variable names to leave room for commenting

	public items[] objectsToPool = new items[2];

	public GameObject pooledObjectsContainer;			//empty game object that will act as a parent/container for the object pool

	public int objectsSpawned = 0;
	public int objectsReturned = 0;
	int fps = 0;
	int fpsCounter = 0;
	int maxFPS = 0;
	int minFPS = 1000;
	int maxInScene = 0;
	int amountToSpawn = 1;
	int spawnObjectInt = 0;
	int qualitySetting = 0;

	public float objectSpeed = 5;
	float timer = 0;
	float fpsTimer = 0;

	string info;
	string temp;

	public GUIStyle infoStyle;

	[System.Serializable]
	public class items
	{
		public GameObject objectToPool;
		public int Number;
		public string objectName;
		public GameObject parent;
		public string parentName;
		public List<GameObject> objects;
		int internalNumber;
		public bool createMore = true;
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0,0,100,50), info, infoStyle);
	}

	void Start () 
	{
		for(int t = 0; t < objectsToPool.Length; t++)
		{
			objectsToPool[t].parent = new GameObject();//Creates an empty GameObject to parent the pooled objects to
			objectsToPool[t].parent.name = objectsToPool[t].parentName;
			objectsToPool[t].parent.transform.parent = pooledObjectsContainer.transform;

			for(int i = 0; i < objectsToPool[t].Number; i++)		//for loop that creates the objects and adds them to the pool
			{
				GameObject obj = (GameObject)Instantiate(objectsToPool[t].objectToPool);//Creates GameObject to be pooled
				obj.SetActive(false); 						// disables the game object
				obj.name = objectsToPool[t].objectName+i; 					// names the object
				obj.transform.parent = objectsToPool[t].parent.transform; 	// parents the object for better scene organization
				objectsToPool[t].objects.Add(obj); 	
			}
		}
	}

	void Update () 
	{
		if((objectsSpawned-objectsReturned) > maxInScene)
			maxInScene = objectsSpawned-objectsReturned;
		info =  "Timer: "+timer+
				"\n Objects Spawned: "+objectsSpawned+
				"\n Objects Returned: "+objectsReturned+
				"\n In the Scene: "+(objectsSpawned-objectsReturned)+" Max: "+maxInScene+
				"\n FPS: "+fps+ " Max: "+maxFPS+" Min: "+minFPS+
				"\n Amount To Spawn: "+amountToSpawn+
				"\n Object Speed: "+objectSpeed;

		fpsTimer+=1*Time.deltaTime;
		fpsCounter++;
		if(fpsTimer>=1)
		{
			fps = fpsCounter;
			if(fps < minFPS)
				minFPS = fps;
			if(fps > maxFPS)
				maxFPS = fps;
			fpsCounter = 0;
			fpsTimer = 0;
			//amountToSpawn++;
		}
		timer+= 1 * Time.deltaTime;
		if(timer >= 1.5f)
		{
			timer = 0;
			try
			{
				Spawn (0);
				Spawn (1);
			}
			catch
			{
				print ("FAILED");
			}
		}
		if(Input.GetKeyDown(KeyCode.W))
			amountToSpawn++;
		if(Input.GetKeyDown(KeyCode.S))
			amountToSpawn--;
		if(Input.GetKeyDown(KeyCode.A))
			objectSpeed--;
		if(Input.GetKeyDown(KeyCode.D))
			objectSpeed++;
		if(Input.GetKeyDown(KeyCode.Q))
		{
			if(qualitySetting == 0)
			{
				qualitySetting = 1;
				RenderSettings.fog = false;
			}
			else if(qualitySetting == 1)
			{
				qualitySetting = 2;
				QualitySettings.SetQualityLevel(0, true);
			}
			else if(qualitySetting == 2)
			{
				qualitySetting = 0;
				RenderSettings.fog = true;
				QualitySettings.SetQualityLevel(5, true);
			}
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			Spawn(0);
		}
	}

	public void Spawn(int o) //Spawn method with an int variable to specify which object to spawn
	{
		objectsSpawned++;
		int counter = 0;
		for(int i = 0; i < objectsToPool[o].Number; i++) //check to make sure there's no bug with this when adding new objects
		{
			if(!objectsToPool[o].objects[i].activeInHierarchy)
			{
				objectsToPool[o].objects[i].transform.position = new Vector3(Random.Range(-24, 24), 1, Random.Range(-24, 24));
				objectsToPool[o].objects[i].SetActive(true);
				return;						//exits from the method when an object is enabled in the pool
			}
			//Debug.Log("OUT OF ENEMIES");
//			if(objectsToPool[o].createMore)
//			{
//				counter++;
//				if(counter == objectsToPool[o].Number)
//				{
//					GameObject obj = (GameObject)Instantiate(objectsToPool[o].objectToPool);
//					obj.SetActive(true);
//					obj.name = (i+1).ToString ();
//					obj.transform.parent = objectsToPool[o].parent.transform;
//
//					//obj.transform.position = new Vector3(0, 1, 0); // problem
//
//					objectsToPool[o].objects.Add(obj);
//					objectsToPool[o].Number++;
//				}
//			}
		}
	}
}
