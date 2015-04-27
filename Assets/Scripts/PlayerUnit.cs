using UnityEngine;
using System.Collections;
using LinearAlgebra.Matricies;

public class PlayerUnit : MonoBehaviour {

	CharacterController ctrl;
	public Vector3 moveDir;
	public Vector3 faceDir;
	private float mouseMoveFactor = 1f;
	private float upAngle = 0f;

	void Awake() {
		ctrl = GetComponent<CharacterController> ();
		moveDir = Vector3.zero;
		faceDir = transform.forward;
		mouseMoveFactor = Mathf.Deg2Rad * 3;
		upAngle = 0f;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float vx = Input.GetAxis ("Horizontal");
		float vy = Input.GetAxis ("Vertical");
		float velocity = 3f;
		moveDir = vy * transform.forward + vx * transform.right;
		moveDir.y = 0f;
		if (moveDir.sqrMagnitude > 0.01f)
			moveDir.Normalize ();
		Vector3 targetV = moveDir * 3f;
		ctrl.SimpleMove (targetV);

		float mx = Input.GetAxis ("Mouse X");
		float my = Input.GetAxis ("Mouse Y");

		my = my * mouseMoveFactor;
		upAngle += my;
		upAngle = Mathf.Clamp (upAngle, -89f * Mathf.Deg2Rad, 89f * Mathf.Deg2Rad);
		//RotateUp (my);
		ApplyUpAngle (upAngle);
		mx = mx * mouseMoveFactor;
		RotateLeft (mx);
	}

	void RotateLeft(float rotationAngle)
	{
		var matrix = RotationMatrices.GetLeftHandMatrix (1, 0, 1, rotationAngle, 0f, 0f);
		faceDir = transform.forward;
		faceDir.z *= -1;
		faceDir = MatrixDotVector (matrix, faceDir);
		faceDir.z *= -1;
		transform.forward = faceDir;
	}

	void ApplyUpAngle(float angle)
	{
		faceDir = transform.forward;
		faceDir.y = 0f;
		transform.forward = faceDir.normalized;
		RotateUp (angle);
	}
	
	void RotateUp(float rotationAngle)
	{
		faceDir = transform.forward;
		Vector3 projFaceDir = faceDir;
		projFaceDir.y = 0f;
		float angle = GetRotationAngle (new Vector3 (0, 0, -1), projFaceDir) * Mathf.Deg2Rad;
		var matrix = RotationMatrices.GetLeftHandMatrix (1, 0, 1, -angle, rotationAngle, angle);
		//var matrix1 = RotationMatrices.GetLeftHandMatrix (1, 0, 1, angle, 0, 0);
		//var matrix2 = RotationMatrices.GetLeftHandMatrix (1, 0, 1, 0, rotationAngle, 0);
		//var matrix3 = RotationMatrices.GetLeftHandMatrix (1, 0, 1, 0, 0, -angle);
		faceDir.z *= -1;
		faceDir = MatrixDotVector (matrix, faceDir);
		//faceDir = MatrixDotVector (matrix1, faceDir);
		//faceDir = MatrixDotVector (matrix2, faceDir);
		//faceDir = MatrixDotVector (matrix3, faceDir);
		faceDir.z *= -1;
		transform.forward = faceDir;
	}
	
	Vector3 MatrixDotVector(DoubleMatrix matrix, Vector3 v)
	{
		DoubleMatrix vMat = new DoubleMatrix (new double[,]{{v.x},{v.y},{v.z}});
		var result = matrix * vMat;
		var vResult = new Vector3 ((float)result [0, 0],(float) result [0, 1],(float) result [0, 2]);
		return vResult;
	}

	
	float GetRotationAngle(Vector3 v1, Vector3 v2) {
		float a1 = Mathf.Atan2 (v1.z, v1.x) * Mathf.Rad2Deg;
		float a2 = Mathf.Atan2 (v2.z, v2.x) * Mathf.Rad2Deg;
		
		return Mathf.DeltaAngle(a1, a2);
	}
}
