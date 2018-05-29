using UnityEngine;
using System.Collections;

public class PlaneControl : MonoBehaviour {

	public float xOffset;
	public float yInit;
	public float speed = 5.0f;

	// Use this for initialization
	void Start () {
		float xInit = Random.Range (-xOffset, xOffset);
		this.transform.position = new Vector3 (xInit, yInit, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos;
		newPos = this.transform.position;
		newPos.y += speed * Time.deltaTime;
		this.transform.position = newPos;
	}

	void OnBecameInvisible(){
		Destroy (gameObject);
	}
}
