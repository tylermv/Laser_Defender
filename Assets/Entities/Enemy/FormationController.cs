using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {

	public GameObject enemyPrefab;

	public float width = 10f;
	public float height = 5f;
	public float speed = 5f;
	public float spawnDelay = 0.5f;

	private bool movingRight = true;
	private float Xmin;
	private float Xmax;



	// Use this for initialization
	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundry = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
		Vector3 rightBoundry = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));
		Xmax = rightBoundry.x;
		Xmin = leftBoundry.x;
		SpawnUntilFull ();
		}

	public void OnDrawGizmos () {
		Gizmos.DrawWireCube(transform.position, new Vector3 (width, height));
	}

	// Update is called once per frame
	void Update () {
		if (movingRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}

		float rightEdgeOfFormation = transform.position.x + (0.5f*width);
		float leftEdgeOfFormation = transform.position.x - (0.5f*width);
		if (leftEdgeOfFormation < Xmin) {
			movingRight = true;
		} else if (rightEdgeOfFormation > Xmax) {
			movingRight = false;
		}
		if (AllMembersDead()) {
			SpawnUntilFull ();
		}
	}

	bool AllMembersDead () {
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
				return false;
			}
		}
		return true;
	}

	Transform NextFreePosition () {
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0)
				return childPositionGameObject;
			}
		return null;
	}

	void SpawnUntilFull () {
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if (NextFreePosition()){
		Invoke ("SpawnUntilFull", spawnDelay);
		}
	}

	public void SpawnEnemies () {
		foreach (Transform child in transform) {
			
		}
	}
}
