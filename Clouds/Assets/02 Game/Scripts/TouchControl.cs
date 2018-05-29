using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TouchControl : MonoBehaviour {

	public BonusControl bonusControl;

	private gameControl GameControl;
	private bool bonusTouch = false;

	void Awake(){
		GameControl = GetComponent<gameControl> ();
	}

	void Start(){
		bonusTouch = false;
	}

	void Update () {
		RaycastHit hit;
		Ray ray;
		CloudBehavior casual;
		int indx;
		foreach (Touch touch in Input.touches) {
			indx = (int)touch.fingerId;
			ray = Camera.main.ScreenPointToRay (touch.position);
			switch (touch.phase) {
			case TouchPhase.Began:
				if (Physics.Raycast (ray, out hit, 20.0f)) {
					switch (hit.collider.tag) {
					case "Cloud":
						casual = hit.collider.gameObject.GetComponent<CloudBehavior>();
						if ((casual.state != 1) && bonusTouch) {
							casual.turnWhite ();
							bonusTouch = false;
							GameControl.ClearWayCloud ();
						} else {
							casual.cloudID = indx;
							casual.cloudSelect ();
						}
						break;
					case "Bonus":
						if (bonusControl.BonusStat ()) {
							bonusTouch = true;
							bonusControl.EmptyBonus ();
						}
						break;
					}
				}
				break;
			case TouchPhase.Canceled:
				GameControl.CloudVerify ();
				break;
			case TouchPhase.Ended:
				GameControl.CloudVerify ();
				break;
			case TouchPhase.Moved:
				if (Physics.Raycast (ray, out hit, 20.0f)) {
					if (hit.collider.tag == "Cloud") {
						casual = hit.collider.gameObject.GetComponent<CloudBehavior>();
						casual.cloudID = indx;
						casual.cloudSelect ();
					}
				}
				break;
			case TouchPhase.Stationary:
				if (Physics.Raycast (ray, out hit, 20.0f)) {
					if (hit.collider.tag == "Cloud") {
						casual = hit.collider.gameObject.GetComponent<CloudBehavior>();
						casual.cloudID = indx;
						casual.cloudSelect ();
					}
				}
				break;
			}
		}
			
	}
}
