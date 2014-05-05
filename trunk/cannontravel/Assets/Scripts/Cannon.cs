using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	public bool has_player;
	private GameObject player;
	private int dir;
	private float rotate_speed;
	private float strength;
	private bool in_shop;

	private bool fire_after_shop_fix;

	// Use this for initialization
	void Start () {
		Random.seed = (int) (System.DateTime.Now.Ticks);
		has_player = false;
		dir = (int) Mathf.Round(Random.Range (0, 1));
		rotate_speed = Random.Range (80, 150);
		strength = Random.Range(700, 1250);
		in_shop = false;
		fire_after_shop_fix = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!in_shop && fire_after_shop_fix){
			fire_after_shop_fix = false;
		}
		else if (has_player && !in_shop) {
			if(dir == 1){
				transform.Rotate(0, 0, rotate_speed * Time.deltaTime);
			}
			else{
				transform.Rotate(0, 0, -rotate_speed * Time.deltaTime);
			}
			if(Input.GetButtonDown("Jump")){
				has_player = false;
				player.SendMessage("Fire", strength);
			}
			if(Vector2.Distance(gameObject.transform.position, player.transform.position) > 2){
				has_player = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			has_player = true;
			player = other.gameObject;
		}
	}

	void Shop(){
		in_shop = !in_shop;
		if(in_shop){
			fire_after_shop_fix = true;
		}
	}
}
