using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	//Gravity
	private float gravity;
	private float zero_height;

	private GameObject gen_o;
	private Generator gen;
	//Cannon Stuff (Cannon = Barrel)
	public float cannon_multiply;
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
	private float money_multiply;

	/////////Shop Stuff
	//Length Space is held down
	private float s_time;
	private float item_time;
	private bool shop_up;
	private Vector2 vel_hold;
	private float exit_timer;
	/// GUI
	public GUITexture background_prefab;
	private GUITexture background;
	public GUITexture item_border_prefab;
	public Texture2D item_border_unselected;
	public Texture2D item_border_selected;
	private GUITexture[] item_border = new GUITexture[4];
	private GUIText[] item_text = new GUIText[4];
	/// Shop Interface
	private int selected_item;
	private Vector2[] system_pos = new Vector2[4];
	/// Prices
	private int[] prices = {80, 100, 130, 120};

	// Use this for initialization
	void Start () {
		gen_o = GameObject.FindWithTag ("Generator");
		gen = gen_o.GetComponent(typeof(Generator)) as Generator;

		gravity = 1;
		zero_height = 1000;

		cannon_multiply = 1.0f;
		money_multiply = 1.0f;

		///Shop Stuff
		shop_setup();
		shop_up = false;
		s_time = 0;
		item_time = 0;
		exit_timer = 0;
		selected_item = 0;

		//Create the initial GUI
		height_gui = Instantiate (gui_text_prefab, new Vector2 (.05f, .97f), Quaternion.identity) as GUIText;
		height_gui.guiText.fontSize = 24;
		fire_count = 0;

		fire_gui = Instantiate (gui_text_prefab, new Vector2 (.05f, .9f), Quaternion.identity) as GUIText;
		fire_gui.guiText.fontSize = 24;

		money_gui = Instantiate (gui_text_prefab, new Vector2 (.05f, .83f), Quaternion.identity) as GUIText;
		money_gui.guiText.fontSize = 24;
		money = 0;

		background = Instantiate (background_prefab, Vector2.zero, Quaternion.identity) as GUITexture;
		background.transform.position = new Vector2 (0, 0);
		background.transform.localScale = new Vector2 (2, 2);
		background.enabled = false;

		//Set values totheir default
		in_barrel = false;
		barrel_fix = false;


		//Draw the GUI (DOne last to make sure it avoids being called
		//Before something is initialized/set
		Update_GUI ();
	}

	// Update is called once per frame
	void Update () {
		height_gui.guiText.text = "Height: " + (int) (transform.position.y);
		if(transform.position.y > 0 && !in_barrel){
			gravity = 1 - (transform.position.y / zero_height);
			rigidbody2D.gravityScale = gravity;
			if(gravity < 0){
				gravity = 0;
			}
		}
		if (in_barrel && !shop_up) {
			transform.position = cannon.transform.position;
			transform.rotation = cannon.transform.rotation;
			transform.Rotate(0, 0, -90);
			rigidbody2D.velocity = new Vector2(0, 0);
		}
		else if(!shop_up && barrel_fix == true && Vector2.Distance(gameObject.transform.position, cannon.transform.position) > 2){
			barrel_fix = false;
		}

		if(!shop_up && Input.GetButton("Jump")){
			s_time += Time.deltaTime;
			if(s_time > .5f){
				shop_on();

			}
		}
		else if(shop_up && Input.GetButton("Jump")){
			if(Input.GetButtonDown("Jump")){
				if(exit_timer == 0){
					exit_timer = Time.time;
				}
				else{
					if(exit_timer + .2f > Time.time){
						shop_off();
					}
					else{
						exit_timer = Time.time;
					}
				}
			}
			s_time += Time.deltaTime;
			item_time = 0;
			if(s_time > .5f){
				if(money > prices[selected_item]){
					bought_item();
					s_time = 0;
				}
			}
		}
		else if(shop_up){
			s_time = 0;
			item_time += Time.deltaTime;
			if(item_time >= 1){
				item_time = 0;
				item_border[selected_item].guiTexture.texture = item_border_unselected;
				selected_item ++;
				if(selected_item == item_text.Length){
					selected_item  = 0;
				}
				item_border[selected_item].guiTexture.texture = item_border_selected;
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
			money += (int) (50 * money_multiply);
			money_gui.guiText.text = "Money: $" + money;
			Destroy(other.gameObject);
		}
		else if(other.CompareTag("Silver_Coin")){
			money += (int) (30 * money_multiply);
			money_gui.guiText.text = "Money: $" + money;
			Destroy(other.gameObject);
		}
		else if(other.CompareTag("Bronze_Coin")){
				money += (int) (10 * money_multiply);
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
		strength *= cannon_multiply;
		//rigidbody2D.isKinematic = false;
		rigidbody2D.gravityScale = gravity;
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
		if(shop_up){
			item_text [0].guiText.text = "Cannon Strength: " + cannon_multiply + " : Cost: " + prices[0];
			item_text [1].guiText.text = "Cannon Density: " + gen.max_cannons + " : Cost: " + prices[1];
			item_text [2].guiText.text = "Money Value: " + money_multiply + " : Cost: " + prices[2];
			item_text [3].guiText.text = "Money Density: " + gen.max_coins + " : Cost: " + prices[3];
		}
	}

	void shop_setup(){
		for(int e = 0; e < item_border.Length; e ++){
			system_pos[e] = new Vector2(.65f, .85f - (.23f * e));
			item_text[e] = Instantiate(gui_text_prefab, system_pos[e], Quaternion.identity) as GUIText;
			item_text[e].guiText.fontSize = 32;
			item_text[e].guiText.anchor = TextAnchor.MiddleCenter;
			item_text[e].guiText.text = "TEST";
			item_border[e] = Instantiate(item_border_prefab, system_pos[e], Quaternion.identity) as GUITexture;
		}

		item_text [0].guiText.text = "Cannon Strength: " + cannon_multiply + " : Cost: " + prices[0];
		item_text [1].guiText.text = "Cannon Density: " + gen.max_cannons + " : Cost: " + prices[1];
		item_text [2].guiText.text = "Money Value: " + money_multiply + " : Cost: " + prices[2];
		item_text [3].guiText.text = "Money Density: " + gen.max_coins + " : Cost: " + prices[3];
		for(int e = 0; e < item_border.Length; e ++){
			item_border[e].enabled = false;
			item_text[e].enabled = false;
		}
	}

	void shop_on(){
		s_time = 0;
		item_time = 0;
		exit_timer = 0;
		shop_up = true;
		vel_hold = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y);
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.isKinematic = true;

		background.enabled = true;
		for(int e = 0; e < item_border.Length; e ++){
			item_border[e].enabled = true;
			item_text[e].enabled = true;
		}
		selected_item = 0;
		item_border [0].guiTexture.texture = item_border_selected;

		if(in_barrel){
			cannon.SendMessage("Shop");
		}
		Update_GUI ();
	}

	void shop_off (){
		s_time = 0;
		item_time = 0;
		exit_timer = 0;
		item_border [selected_item].guiTexture.texture = item_border_unselected;
		selected_item = 0;
		rigidbody2D.velocity = vel_hold;
		rigidbody2D.isKinematic = false;
		shop_up = false;
		if(in_barrel){
			cannon.SendMessage("Shop");
		}
		else{
			rigidbody2D.gravityScale = gravity;
		}

		background.enabled = false;
		for(int e = 0; e < item_border.Length; e ++){
			item_border[e].enabled = false;
			item_text[e].enabled = false;
		}
	}

	void bought_item(){
		money -= prices[selected_item];
		prices[selected_item] += (int) (.2 * prices[selected_item]);

		if(selected_item == 0){
			cannon_multiply += .2f;
		}
		else if(selected_item == 1){
			gen.max_cannons += 3;
		}
		else if(selected_item == 2){
			money_multiply += .2f;
		}
		else if(selected_item == 3){
			gen.max_coins += 2;
		}
		Update_GUI ();
	}
}
