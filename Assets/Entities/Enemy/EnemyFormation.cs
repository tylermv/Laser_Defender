using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFormation : MonoBehaviour {
	public int scoreEnemy = 150;
	public float health = 150f;
	public float laserSpeed = 10f;
	public float shotsPerSecond = 0.5f;
	public GameObject laserPrefab;
	public AudioClip enemyFire;
	public AudioClip deathSound;

	private ScoreKeeper scoreKeeper;


	void Start () {
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
	}
	void Update () {
		float probability = Time.deltaTime * shotsPerSecond;

		if (Random.value < probability) {
			Fire ();
		}
	}
	

	void Fire() {
		Vector3 startPosition = transform.position + new Vector3 (0, -1, 0);
		GameObject missile = Instantiate(laserPrefab, startPosition, Quaternion.identity)as GameObject;
		missile.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, -laserSpeed);
		AudioSource.PlayClipAtPoint (enemyFire, transform.position);

	}

	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();

		if (missile) {
			health -= missile.GetDamage ();
			missile.Hit ();
			if (health <= 0) {
				Die ();
			}
		}
	}

	void Die () {
		Destroy (gameObject);
		AudioSource.PlayClipAtPoint (deathSound, transform.position);
		scoreKeeper.Score(scoreEnemy);
	}
}
