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


	private float draw_distance;
	private float re_draw;
	private float cannon_chance;
	private int max_cannons;
	

	// Use this for initialization
	void Start () {
		size = 0;
		draw_distance = 20f;
		re_draw = 10f;
		cannon_chance = .8f;
		max_cannons = 4;
		Begin ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void Begin(){
		Random.seed = 13 + (int)(Mathf.PI * Random.Range (3, 13) * Time.time);
		int spawn = 4;
		cannons [size] = Instantiate (cannon, new Vector2 (0, 0), Quaternion.identity) as GameObject;
		Instantiate (player_prefab, new Vector2 (0, 2), Quaternion.identity);
		size ++;
		for(int e = 1; e < spawn; e ++){
			Random.seed = 13 * e + (int)(Mathf.PI * Random.Range (3, 13) * Time.time);
			Vector2 temp;
			bool run = false;
			do{
				run = false;
				temp = new Vector2(Random.Range(-8, 8), Random.Range(-4, 4));
				for(int a = 1; a <= e; a ++){
					if(Vector2.Distance(temp, cannons[size - a].transform.position) < 2 && Vector2.Distance(temp, player.transform.position) < 2){
						run = true;
					}
				}
			}
			while(run);
			cannons[size] = Instantiate (cannon, temp, Quaternion.identity) as GameObject;
			size ++;
		}

	}

	void Gen(){
		Random.seed = 13 + (int)(Mathf.PI * Random.Range (3, 13) * Time.time);
		int spawn = 0;
		for (int e = 0; e < max_cannons; e ++) {
			if(cannon_chance > Random.Range(0, 1)){
				spawn ++;
			}
		}
	}

	void inc_array(){
		GameObject[] temp = new GameObject[cannons.Length + 100];
		for(int e = 0; e < cannons.Length; e ++){
			temp[e] = cannons[e];
		}
		cannons = temp;
	}
}
