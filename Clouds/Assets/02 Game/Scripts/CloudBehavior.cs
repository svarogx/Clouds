using UnityEngine;
using System.Collections;

public class CloudBehavior : MonoBehaviour {

	public int state;
	public int cloudID = -1;
	public Sprite whiteCloud;
	public Sprite grayCloud;
	public Sprite blackCloud;

	private GameObject gameController;
	private SpriteRenderer cloudRender;

	void Awake(){
		cloudRender = GetComponent<SpriteRenderer> ();
	}

	void Start () {
		state = 1;
		turnCloud ();
	}
	
	void Update () {
	}

	public void turnWhite(){
		state = 1;
		cloudRender.sprite = whiteCloud;
		turnCloud ();
	}

	public void turnGray(){
		state = 2;
		cloudRender.sprite = grayCloud;
		turnCloud ();
	}

	public void turnBlack(){
		state = 3;
		cloudRender.sprite = blackCloud;
		turnCloud ();
	}

	public void turnCloud (){
		cloudRender.color = Color.white; 
	}


	public void cloudSelect(){
		cloudRender.color = Color.red;
	}

}
