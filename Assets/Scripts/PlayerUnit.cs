using UnityEngine;
using System.Collections;

public class PlayerUnit : MonoBehaviour {

	CharacterController ctrl;
	public Vector3 moveDir;
	public Vector3 faceDir;

	void Awake() {
		ctrl = GetComponent<CharacterController> ();
		moveDir = Vector3.zero;
		faceDir = transform.forward;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float vx = Input.GetAxis ("Horizontal");
		float vy = Input.GetAxis ("Vertical");
		float velocity = 3f;
		moveDir = (new Vector3 (vx, 0f, vy));
		if (moveDir.sqrMagnitude > 0.01f)
			moveDir.Normalize ();
		Vector3 targetV = moveDir * 3f;
		ctrl.SimpleMove (targetV);
	}
}
