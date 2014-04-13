using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//Cannon Stuff (Cannon = Barrel)
	public bool in_barrel;
	private GameObject cannon;
	private bool barrel_fix;

	//GUI Prefabs
	public GUIText gui_text_prefab;
	public GUITexture gui_texture_prefab;

	//Stats/GUI Objects
	private GUIText fires;
	public int fire_count;
	private GUIText height_gui;

	// Use this for initialization
	void Start () {
		fire_count = 0;
		fires = Instantiate (gui_text_prefab, new Vector2 (.05f, .9f), Quaternion.identity) as GUIText;
		fires.guiText.fontSize = 24;
		height_gui = Instantiate (gui_text_prefab, new Vector2 (.05f, .97f), Quaternion.identity) as GUIText;
		height_gui.guiText.fontSize = 24;
		in_barrel = false;
		barrel_fix = false;
		Update_GUI ();
	}

	// Update is called once per frame
	void Update () {
		height_gui.guiText.text = "Height: " + (int) (transform.position.y);
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
		float t1 = Mathf.Sin((transform.eulerAngles.z + 90) * Mathf.PI / 180f);
		float t2 = Mathf.Cos((transform.eulerAngles.z + 90) * Mathf.PI / 180f);

		rigidbody2D.AddForce(new Vector2(strength * t2, strength * t1));
		in_barrel = false;
		fire_count ++;
		Update_GUI ();
	}

	void Update_GUI(){
		fires.guiText.text = "Times Fired: " + fire_count;
		height_gui.guiText.text = "Height: " + (int) (transform.position.y);
	}
}
