using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PereyeControl : MonoBehaviour {

	public float initOffsetY = 3.3f;
	public float initOffsetX = 2.5f;
	public float walkSpeed = 5.0f;
	public float jumpSpeed = 5.0f;

	private int direction = 0;
	private bool onAnimation = false;
	private Animator pereyeAnim;

	private int Jumps;
	private float initXCloud;
	private float initYCloud;
	private float deltaYCloud;
	private float goalTime;
	private float initJumpY;
	private float endJumpY;
	private int cloudCounter = 0;

	void Awake(){
		pereyeAnim = GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
		this.transform.position = new Vector3 (-initOffsetX, -initOffsetY, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos;
		switch (direction) {
		case 0:
			if (onAnimation)
				return;
			this.transform.eulerAngles = new Vector3 (0, 0, -90);
			newPos = this.transform.position;
			newPos.x = newPos.x + walkSpeed * Time.deltaTime;
			this.transform.position = newPos;
			if (newPos.x >= initOffsetX) {
				direction = 1;
				this.transform.eulerAngles = new Vector3 (0, 0, 0);
				pereyeAnim.SetTrigger ("look");
				onAnimation = true;
				Invoke ("EndAnimation", 1.042f);
			}
			break;
		case 1:
			if (onAnimation)
				return;
			this.transform.eulerAngles = new Vector3 (0, 0, 90);
			newPos = this.transform.position;
			newPos.x = newPos.x - walkSpeed * Time.deltaTime;
			this.transform.position = newPos;
			if (newPos.x <= -initOffsetX) {
				direction = 0;
				this.transform.eulerAngles = new Vector3 (0, 0, 0);
				pereyeAnim.SetTrigger ("look");
				onAnimation = true;
				Invoke ("EndAnimation", 1.042f);
			}
			break;
		case 2:
			float delta = Time.deltaTime;
			if (initXCloud > this.transform.position.x) {
				delta = jumpSpeed * delta;
				this.transform.eulerAngles = new Vector3 (0, 0, -90);
			} else {
				delta = -jumpSpeed * delta;
				this.transform.eulerAngles = new Vector3 (0, 0, 90);
			}
			newPos = this.transform.position;
			newPos.x = newPos.x + delta;
			this.transform.position = newPos;
			if (Mathf.Abs (newPos.x - initXCloud) < 0.1f) {
				this.transform.eulerAngles = new Vector3 (0, 0, 0);
				direction = 3;
				pereyeAnim.SetTrigger ("jump");
				goalTime = Time.time + 1.0f;
				initJumpY = this.transform.position.y;
				endJumpY = initYCloud;
			} 
			break;
		case 3:
			if (Time.time <= goalTime) {
				newPos = this.transform.position;
				newPos.y = Mathf.Lerp(endJumpY, initJumpY, goalTime - Time.time);
				this.transform.position = newPos;
			} else {
				cloudCounter += 1;
				pereyeAnim.SetTrigger ("jump");
				goalTime = Time.time + 1.0f;
				initJumpY = this.transform.position.y;
				endJumpY = (cloudCounter * deltaYCloud) + initYCloud;
				if (cloudCounter > Jumps)
					direction = 4;
			}
				
			break;
		case 4:
			SceneManager.LoadScene ("prototype");
			break;
		}
}

	private void EndAnimation(){
		onAnimation = false;
	}

	public void PereyeFinalStage(int cloudNumber, float initX, float initY, float deltaY){
		direction = 2;
		Jumps = cloudNumber;
		initXCloud = initX;
		initYCloud = initY;
		deltaYCloud = deltaY;
		pereyeAnim.SetBool ("isWalking", false); 
	} 
}
