using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public bool in_barrel;
	private GameObject cannon;
	private bool barrel_fix;

	// Use this for initialization
	void Start () {
		in_barrel = false;
		barrel_fix = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (in_barrel) {
			transform.position = cannon.transform.position;
			transform.rotation = cannon.transform.rotation;
			transform.Rotate(0, 0, -90);
			rigidbody2D.velocity = new Vector2(0, 0);
		}
		else if(barrel_fix == true && Vector2.Distance(gameObject.transform.position, cannon.transform.position) > 2){
			barrel_fix = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!barrel_fix && other.CompareTag ("Barrel")) {
			Debug.Log("t1");
			in_barrel = true;
			barrel_fix = true;
			cannon = other.gameObject;
			//rigidbody2D.isKinematic = true;
			rigidbody2D.gravityScale = 0;
			transform.position = cannon.transform.position;
			transform.rotation = cannon.transform.rotation;
		}
	}



	/*void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag ("Barrel")) {
			in_barrel = false;
			//rigidbody2D.isKinematic = true;
		}
	}*/

	void Fire(float strength){
		//rigidbody2D.isKinematic = false;
		rigidbody2D.gravityScale = 1;
		float t1 = Mathf.Sin(transform.rotation.z + 90);
		float t2 = Mathf.Cos(transform.rotation.z + 90);
		rigidbody2D.AddForce(new Vector2(strength * t2, strength * t1));
		in_barrel = false;
	}
}
