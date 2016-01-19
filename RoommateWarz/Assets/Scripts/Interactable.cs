using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {
    GameObject text;
    public Interaction interaction;

    void Awake() {
        text = transform.Find("prompt").gameObject;
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            text.SetActive(true);
        }
    }
    void OnTriggerStay2D(Collider2D other) {
        if (other.tag != "Player")
            return;
        int num = other.gameObject.GetComponent<Control>().playerNum;
        if (Input.GetButtonDown("Interact1") && num == 1) {
            interaction.Act(num - 1);
        }
        if (Input.GetButtonDown("Interact2") && num == 2) {
            interaction.Act(num - 1);
        }
        if (Input.GetButtonDown("Interact3") && num == 3) {
            interaction.Act(num - 1);
        }
        if (Input.GetButtonDown("Interact4") && num == 4) {
            interaction.Act(num - 1);
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            text.SetActive(false);
        }
    }
}
