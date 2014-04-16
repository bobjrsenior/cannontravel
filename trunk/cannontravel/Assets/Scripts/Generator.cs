using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

	//Prefab Reference
	public GameObject cannon;
	public GameObject player_prefab;

	//In-game Player
	private GameObject player;

	//Holds Cannons
	private GameObject[] cannons = new GameObject[100];
	//How many cannons are actually in the array
	private int size;

	//Procedural Stuff
	private Vector2[] drawn = new Vector2[50];
	private int d_size;
	private int cur_x;
	private int cur_y;
	private float draw_distance;
	private float re_draw;

	private float cannon_chance;
	private int max_cannons;
	

	// Use this for initialization
	void Start () {
		Random.seed = (int)(System.DateTime.Now.Ticks);
		size = 0;
		d_size = 0;
		draw_distance = 20f;
		re_draw = 10f;
		cannon_chance = .8f;
		max_cannons = 15;
		Begin ();
	}
	
	// Update is called once per frame
	void Update () {
		int x_pos = (int) (player.transform.position.x);
		int y_pos = (int) (player.transform.position.y);
		if(x_pos % 8 == 0 && x_pos % 16 != 0 && (x_pos - (8 * Mathf.Sign (x_pos))) % 32 == 0){
			cur_x = x_pos;
		}
		if(y_pos % 8 == 0 && y_pos % 16 != 0 && (y_pos - (8 * Mathf.Sign(y_pos))) % 32 == 0){
			cur_y = y_pos;
		}
		if (Mathf.Abs(x_pos) > Mathf.Abs(cur_x + (32 * Mathf.Sign (cur_x)))) {
			cur_x += 32;
		}
		if (Mathf.Abs(y_pos) > Mathf.Abs(cur_y + (32 * Mathf.Sign(cur_y)))) {
			cur_y += 32;
		}
		if(cur_x % 8 == 0 && cur_x % 16 != 0 && (cur_x - (8 * Mathf.Sign (cur_x))) % 32 == 0){
			if(x_pos != 0 || cur_y != 0){
				Vector2 temp = new Vector2(cur_x + Mathf.Sign(cur_x) * 24, cur_y + Mathf.Sign(cur_y) * 24);
				bool check = true;
				//Debug.Log("Check X: " + temp.x + " : " + temp.y);
				for(int e = drawn.Length - 1; e >= 0; e --){
					if(drawn[e].Equals(temp)){
						check = false;
						break;
					}
				}
				if(check){
					Gen (temp);
				}
			}
		}
		if(cur_y % 8 == 0 && cur_y % 16 != 0 && (cur_y - (8 * Mathf.Sign(cur_y))) % 32 == 0){
			if(cur_y != 0 || cur_x != 0){
				Vector2 temp = new Vector2(cur_x + Mathf.Sign(cur_x) * 24, cur_y + Mathf.Sign(cur_y) * 24 );
				bool check = true;
				//Debug.Log("Check Y: " + temp.x + " : " + temp.y);
				for(int e = drawn.Length - 1; e >= 0; e --){
					if(drawn[e].Equals(temp)){
						check = false;
						break;
					}
				}
				if(check){
					Gen (temp);
				}
			}
		}

	}


	void Begin(){
		drawn[d_size] = new Vector2 (0, 0);
		d_size ++;
		cur_x = 0;
		cur_y = 0;
		int spawn = 15;
		player = Instantiate (player_prefab, new Vector2 (0, 2), Quaternion.identity) as GameObject;
		cannons [size] = Instantiate (cannon, new Vector2 (0, 0), Quaternion.identity) as GameObject;
		size ++;
		for(int e = 1; e < spawn; e ++){
			Vector2 temp;
			bool run = false;
			do{
				run = false;
				temp = new Vector2(Random.Range(-15, 15), Random.Range(-15, 15));
				if(Vector2.Distance(temp, player.transform.position) < 2){
					run = true;
				}
				else{
					for(int a = 1; a <= e; a ++){
						if(Vector2.Distance(temp, cannons[size - a].transform.position) < 2){
							run = true;
						}
					}
				}
			}
			while(run);
			cannons[size] = Instantiate (cannon, temp, Quaternion.identity) as GameObject;
			size ++;
		}

	}

	void Gen(Vector2 point){
		//Debug.Log ("Gen: " + point.x + " : " + point.y);
		drawn [d_size] = point;
		d_size ++;
		if(d_size == drawn.Length){
			inc_draws();
		}
		int dist = (int) Vector2.Distance (Vector2.zero, point);
		float chance = .8f - (.05f * dist / 16);
		if(chance < .1){
			chance = .1f;
		}
		int spawn = 0;
		for (int e = 0; e < max_cannons; e ++) {
			if(chance > Random.Range(0, 1)){
				spawn ++;
			}
		}
		if(spawn == 0){
			spawn = 1;
		}

		for(int e = 0; e < spawn; e ++){
			Vector2 temp;
			bool run = false;
			do{
				run = false;
				temp = new Vector2(Random.Range(point.x - 15, point.x + 15), Random.Range(point.y - 15, point.y + 15));
				for(int a = 1; a <= e; a ++){
					if(Vector2.Distance(temp, cannons[size - a].transform.position) < 3){
						run = true;
						break;
					}
				}
			}
			while(run);
			cannons[size] = Instantiate (cannon, temp, Quaternion.identity) as GameObject;
			size ++;
			if(size == cannons.Length){
				inc_cannons();
			}
		}
	}

	void inc_cannons(){
		GameObject[] temp = new GameObject[cannons.Length + 100];
		for(int e = 0; e < cannons.Length; e ++){
			temp[e] = cannons[e];
		}
		cannons = temp;
	}

	void inc_draws(){
		Vector2[] temp = new Vector2[cannons.Length + 50];
		for(int e = 0; e < drawn.Length; e ++){
			temp[e] = drawn[e];
		}
		drawn = temp;
	}
}
