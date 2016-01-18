using UnityEngine;
using System.Collections;

public class RespawnPoint : MonoBehaviour {
	void Start() {
		// Remove the sprite
		Destroy(GetComponent(typeof(SpriteRenderer)));
	}
}
