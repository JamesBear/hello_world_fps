using UnityEngine;
using System.Collections;
using LinearAlgebra.Matricies;

public class PlayerCamera : MonoBehaviour {

	private Vector3 offset;
	public Vector3 Offset;
	public bool test;

	// Use this for initialization
	void Start () {
		test = false;
		offset = new Vector3 (0f, 2f, 0.3f);
	}
	
	// Update is called once per frame
	void Update () {

		if (test) {
			if (Offset.sqrMagnitude < 0.01f)
				Offset = offset;
			offset = Offset;
		}

		var player = CallCenter.instance.player;
		Vector3 targetPos = player.transform.position + offset;
		transform.position = targetPos;
		transform.forward = player.faceDir;
		transform.Rotate (30, 0, 0);
	}

}
