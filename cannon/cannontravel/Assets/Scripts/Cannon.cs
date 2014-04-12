using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	public bool has_player;
	private GameObject player;
	private int dir;
	private float rotate_speed;
	private float strength;

	// Use this for initialization
	void Start () {
		has_player = false;
		dir = (int) Mathf.Round(Random.Range (0, 1));
		rotate_speed = Random.Range (20, 50);
		strength = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (has_player) {
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
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			Debug.Log("t2");
			has_player = true;
			player = other.gameObject;
		}
	}
}
