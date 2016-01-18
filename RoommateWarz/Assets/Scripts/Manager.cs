using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
    public GameObject player;
    public RuntimeAnimatorController animation1;
    public RuntimeAnimatorController animation2;
    public Follow cam;
    GameObject player1, player2, player3, player4;
	
	// Update is called once per frame
	void Update() {
        #region startPressed
        if (Input.GetButton("Start1")) {
            if (player1 == null) {
				// Initialize player 1
                player1 = Instantiate(player);
                player1.GetComponent<Control>().playerNum = 1;
                player1.GetComponent<Animator>().runtimeAnimatorController = animation1;
				player1.transform.position = GetSpawnPoint(player1.transform.position);
                cam.following[0] = player1.transform;
            }
            else if (!player1.activeSelf) {
				// Respawn player 1
                player1.SetActive(true);
                player1.GetComponent<Control>().ResetHealth();
				player1.transform.position = GetSpawnPoint(player1.transform.position);
				cam.following[0] = player1.transform;
            }
        }
        if (Input.GetButton("Start2")) {
            if (player2 == null) {
				// Initialize player 2
				player2 = Instantiate(player);
                player2.GetComponent<Control>().playerNum = 2;
                player2.GetComponent<Animator>().runtimeAnimatorController = animation2;
				player2.transform.position = GetSpawnPoint(player2.transform.position);
				cam.following[1] = player2.transform;
            }
            else if (!player2.activeSelf) {
				// Respawn player 2
				player2.SetActive(true);
                player2.GetComponent<Control>().ResetHealth();
				player2.transform.position = GetSpawnPoint(player2.transform.position);
				cam.following[1] = player2.transform;
            }
        }
        #endregion
        #region notActive
        if (player1 && !player1.activeSelf) {
            cam.following[0] = null;
        }
        if (player2 && !player2.activeSelf) {
            cam.following[1] = null;
        }
        #endregion
    }

	/// <summary>
	/// Find a respawn point for the given player
	/// </summary>
	/// <param name="playerPosition"></param>
	/// <returns></returns>
	Vector3 GetSpawnPoint(Vector3 playerPosition) {
		Vector3 spawnPoint = Vector3.zero;
		var spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
		int index = (int)(Random.value * spawnPoints.Length);
		while (index == spawnPoints.Length) {
			index = (int)(Random.value * spawnPoints.Length);
		}
		spawnPoint = spawnPoints[index].transform.position;

		// Ensure the player appears at the correct z position
		spawnPoint.z = playerPosition.z;
		return spawnPoint;
	}
}
