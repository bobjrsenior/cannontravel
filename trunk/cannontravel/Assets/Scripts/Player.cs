using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public bool in_barrel;

	// Use this for initialization
	void Start () {
		in_barrel = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Barrel")) {
			in_barrel = true;
		}
	}
}
