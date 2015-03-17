using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {
	public int message=1;

	private float accum   = 0f;
	private int   frames  = 0; 
	private string sFPS   = ""; 


	void Start()
	{
		StartCoroutine( FPS() );
	}


	void Update()
	{
		accum += Time.timeScale/ Time.deltaTime;
		++frames;
	}


	void OnGUI(){
		if (message==1) GUI.Window(3, new Rect( 110, 10, 200, 50 ), MyWindow, "Simple Example");
		if (message==2) GUI.Window(3, new Rect( 110, 10, 200, 50 ), MyWindow, "Dynamic Obstacle Example");
		if (message==3) GUI.Window(3, new Rect( 110, 10, 200, 50 ), MyWindow, "Patrol Example");
		if (message==4) GUI.Window(3, new Rect( 110, 10, 200, 70 ), MyWindow, "Stress Example");
		if (message==5) GUI.Window(3, new Rect( 110, 10, 200, 50 ), MyWindow, "Terrain Example");
		if (message==6) GUI.Window(3, new Rect( 110, 10, 200, 50 ), MyWindow, "");
		if (message==7) GUI.Window(3, new Rect( 110, 10, 200, 70 ), MyWindow, "Stress Example");

		if (Time.realtimeSinceStartup > 0.7f) GUI.Window(2, new Rect( 10, 10, 75, 50 ), MyWindowFPS, "");
	}


	void MyWindow(int windowID){
		if (message==1) GUI.Label( new Rect( 10, 15, 180, 40 ),"Hit left mouse button to assign target position");
		if (message==2) GUI.Label( new Rect( 10, 15, 180, 40 ),"Hit left mouse button to assign target position");
		if (message==3) GUI.Label( new Rect( 10, 15, 180, 40 ),"Agent is patrolled the points assigned");
		if (message==4) GUI.Label( new Rect( 10, 15, 180, 50 ),"500 agents patrolling with dynamic obstacles and local avoidance");
		if (message==5) GUI.Label( new Rect( 10, 15, 180, 40 ),"Hit left mouse button to assign target position");
		if (message==6) GUI.Label( new Rect( 25, 20, 180, 40 ),"Local Avoidance Example");
		if (message==7) GUI.Label( new Rect( 10, 15, 180, 50 ),"50 agents patrolling with dynamic obstacles and local avoidance");
	}

	
	IEnumerator FPS()
	{
		while( true )
		{
			sFPS = " "+(int)(accum/frames);
			accum = 0.0f;
			frames = 0;
			yield return new WaitForSeconds( 0.5f );
		}
	}
	

	void MyWindowFPS(int windowID){
		GUI.Label( new Rect( 10, 20, 75, 30 ), sFPS + " FPS");
		accum = 0.0F;
		frames = 0;
	}
}
