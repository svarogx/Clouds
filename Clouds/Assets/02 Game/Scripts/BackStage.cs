using UnityEngine;
using System.Collections;

public class BackStage : MonoBehaviour {

	public GameObject planePrefab;
	public float minTime;
	public float maxTime;

	// Use this for initialization
	void Start () {
		Invoke ("PlaneBirth", Random.Range (minTime, maxTime));	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void PlaneBirth(){
		Instantiate (planePrefab);
		Invoke ("PlaneBirth", Random.Range (minTime, maxTime));	
	}
}
