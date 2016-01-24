using UnityEngine;
using System.Collections;
using System;

public class CycleCharacter : Interaction {
    OnlineManager manager;
    void Awake() {
        manager = GetComponent<OnlineManager>();
    }

    public override void Act(int playerNum) {
        int newChar = manager.characterSelection[playerNum] = (manager.characterSelection[playerNum] + 1) % manager.characters.Length;
        Vector3 pos = manager.players[playerNum].transform.position;
        Destroy(manager.players[playerNum]);
        manager.players[playerNum] = Instantiate(manager.characters[newChar]);
        manager.players[playerNum].transform.position = pos;
        manager.players[playerNum].GetComponent<Control>().playerNum = playerNum + 1;
        manager.cam.following[playerNum] = manager.players[playerNum].transform;
    }
}
