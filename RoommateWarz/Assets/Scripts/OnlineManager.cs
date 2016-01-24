using UnityEngine;
using System.Collections;

public class OnlineManager : MonoBehaviour {
    public GameObject[] characters;
    public Follow cam;
    public GameObject[] players;
    public int[] characterSelection;
    const int MAX_PLAYERS = 4;

    void Awake() {
        players = new GameObject[MAX_PLAYERS];
        characterSelection = new int[MAX_PLAYERS];
    }
    // Update is called once per frame
    void Update() {
        #region startPressed
        if (Input.GetButton("Start1")) {
            ActivatePlayer(0);
        }
        if (Input.GetButton("Start2")) {
            ActivatePlayer(1);
        }
        if (Input.GetButton("Start3")) {
            ActivatePlayer(2);
        }
        if (Input.GetButton("Start4")) {
            ActivatePlayer(3);
        }
        #endregion
        #region notActive
        for (int i = 0; i < MAX_PLAYERS; ++i) {
            if (players[i] && !players[i].activeSelf) {
                cam.following[i] = null;
            }
        }
        #endregion
    }

    void ActivatePlayer(int playerNum) {
        if (players[playerNum] == null) {
            // Initialize player
            characterSelection[playerNum] = playerNum % characters.Length;
            players[playerNum] = Instantiate(characters[characterSelection[playerNum]]);
            players[playerNum].GetComponent<Control>().playerNum = playerNum + 1;
            players[playerNum].transform.position = GetInitialSpawnPoint(players[playerNum].transform.position, playerNum+1);
            cam.following[playerNum] = players[playerNum].transform;
        }
        else if (!players[playerNum].activeSelf) {
            // Respawn player 1
            players[playerNum].SetActive(true);
            players[playerNum].GetComponent<Control>().ResetHealth();
            players[playerNum].transform.position = GetSpawnPoint(players[playerNum].transform.position);
            cam.following[playerNum] = players[playerNum].transform;
        }
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
    Vector3 GetInitialSpawnPoint(Vector3 playerPosition, int playerNum) {
        Vector3 spawnPoint = Vector3.zero;
        var spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        foreach(GameObject obj in spawnPoints) {
            string spawnNum = obj.name.Substring(obj.name.Length - 1);
            if(spawnNum == playerNum.ToString()) {
                spawnPoint = obj.transform.position;
                spawnPoint.z = playerPosition.z;
                return spawnPoint;
            }
        }
        throw new System.Exception("spawn position for player " + playerNum + " not found.");
    }
}
