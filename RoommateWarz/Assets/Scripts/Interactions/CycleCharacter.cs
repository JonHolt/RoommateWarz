using UnityEngine;
using System.Collections;
using System;

public class CycleCharacter : Interaction {
    Manager manager;
    void Awake() {
        manager = GetComponent<Manager>();
    }

    public override void Act(int playerNum) {
        int newChar = manager.characterSelection[playerNum] = (manager.characterSelection[playerNum] + 1) % manager.characters.Length;
        manager.players[playerNum].GetComponent<Animator>().runtimeAnimatorController = manager.characters[newChar];
        manager.players[playerNum].GetComponent<Control>().bulletType = manager.bullets[newChar];
    }
}
