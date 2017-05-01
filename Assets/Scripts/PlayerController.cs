using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed = 1.0f;
	public float padding = 1.0f;
	public GameObject laserPrefab;
	public float laserSpeed = 2.0f;
	public float firingRate = 0.2f;
	public float health = 200f;
	public AudioClip fireSound;

	private float Xmin, Xmax;



	private void Awake () {

	}
	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3 (0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3 (1,0,distance));
		Xmin = leftmost.x + padding;
		Xmax = rightmost.x - padding;

	}

	void Fire() {
		Vector3 offset = new Vector3 (0, 1, 0);
		GameObject beam = Instantiate(laserPrefab, transform.position + offset, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector2 (0,laserSpeed);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.000001f, firingRate);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("Fire");
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position += Vector3.left * speed * Time.deltaTime;

		} else if (Input.GetKey (KeyCode.RightArrow)) {
			transform.position += Vector3.right * speed * Time.deltaTime; 
		} 

		float newX = Mathf.Clamp (transform.position.x, Xmin, Xmax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
		}
	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();

		if (missile) {
			Debug.Log ("Player hit!");
			health -= missile.GetDamage ();
			missile.Hit ();
			if (health <= 0) {
				Destroy (gameObject);
				SceneManager.LoadScene ("Win Screen");
				}
			}
		}
	}


