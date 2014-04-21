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

	///Stats/GUI Objects
	//Times fired from a cannon
	private GUIText fire_gui;
	public int fire_count;
	//How High you are
	private GUIText height_gui;
	private GUIText money_gui;
	//Money you have
	private int money;

	/////////Shop Stuff
	//Length Space is held down
	private float s_time;
	private bool shop_up;
	/// GUI
	public Texture2D background;
	/// Shop Interface


	// Use this for initialization
	void Start () {
		//Create the initial GUI
		height_gui = Instantiate (gui_text_prefab, new Vector2 (.05f, .97f), Quaternion.identity) as GUIText;
		height_gui.guiText.fontSize = 24;
		fire_count = 0;

		fire_gui = Instantiate (gui_text_prefab, new Vector2 (.05f, .9f), Quaternion.identity) as GUIText;
		fire_gui.guiText.fontSize = 24;

		money_gui = Instantiate (gui_text_prefab, new Vector2 (.05f, .83f), Quaternion.identity) as GUIText;
		money_gui.guiText.fontSize = 24;
		money = 0;

		//Set values totheir default
		in_barrel = false;
		barrel_fix = false;

		///Shop Stuff
		shop_up = false;
		s_time = 0;

		//Draw the GUI (DOne last to make sure it avoids being called
		//Before something is initialized/set
		Update_GUI ();
	}

	// Update is called once per frame
	void Update () {
		height_gui.guiText.text = "Height: " + (int) (transform.position.y);
		if (in_barrel && !shop_up) {
			transform.position = cannon.transform.position;
			transform.rotation = cannon.transform.rotation;
			transform.Rotate(0, 0, -90);
			rigidbody2D.velocity = new Vector2(0, 0);
		}
		else if(!shop_up && barrel_fix == true && Vector2.Distance(gameObject.transform.position, cannon.transform.position) > 2){
			barrel_fix = false;
		}

		if(Input.GetButton("Jump")){
			s_time += Time.deltaTime;
			if(s_time > .5f){
				s_time = 0;
				shop_up = true;
				shop_on();
			}
		}
		else{
			s_time = 0;
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
		if(other.CompareTag("Gold_Coin")){
			money += 50;
			money_gui.guiText.text = "Money: $" + money;
			Destroy(other.gameObject);
		}
		else if(other.CompareTag("Silver_Coin")){
			money += 30;
			money_gui.guiText.text = "Money: $" + money;
			Destroy(other.gameObject);
		}
		else if(other.CompareTag("Bronze_Coin")){
			money += 10;
			money_gui.guiText.text = "Money: $" + money;
			Destroy(other.gameObject);
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
		height_gui.guiText.text = "Height: " + (int) (transform.position.y);
		fire_gui.guiText.text = "Times Fired: " + fire_count;
		money_gui.guiText.text = "Money: $" + money;
	}

	void shop_on(){
		//GUI.color = new Color(0, 0, 0, 1);
		GUI.depth = -1000;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

	}

	void shop_off (){

	}
}
