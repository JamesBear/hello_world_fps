using UnityEngine;
using System.Collections;

public class CallCenter : MonoBehaviour
{
	public static CallCenter instance { get; private set;}

	public PlayerUnit player { get; private set;}

	void Awake() {
		instance = this;
		player = gameObject.GetComponentInChildren<PlayerUnit> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
