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
		int y_pos = (int)(player.transform.position.y);
		if(x_pos % 16 == 0 && !(x_pos == 0 && y_pos == 0)){
			Vector2 temp = new Vector2(x_pos, cur_y);
			bool check = true;
			foreach(Vector2 e in drawn){
				if(e.Equals(temp)){
					check = false;
					break;
				}
			}
			if(check){
				Gen (temp);
			}
		}
	}


	void Begin(){
		drawn[d_size] = new Vector2 (0, 0);
		d_size ++;
		cur_x = 0;
		cur_y = 0;
		int spawn = 15;
		cannons [size] = Instantiate (cannon, new Vector2 (0, 0), Quaternion.identity) as GameObject;
		player = Instantiate (player_prefab, new Vector2 (0, 2), Quaternion.identity) as GameObject;
		size ++;
		for(int e = 1; e < spawn; e ++){
			Vector2 temp;
			bool run = false;
			do{
				run = false;
				temp = new Vector2(Random.Range(-16, 16), Random.Range(-16, 16));
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
		int spawn = 0;
		for (int e = 0; e < max_cannons; e ++) {
			if(cannon_chance > Random.Range(0, 1)){
				spawn ++;
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
