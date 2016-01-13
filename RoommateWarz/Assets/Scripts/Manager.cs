using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
    public GameObject player;
    public RuntimeAnimatorController animation1;
    public RuntimeAnimatorController animation2;
    public Follow cam;
    GameObject player1, player2, player3, player4;
	
	// Update is called once per frame
	void Update () {
        #region startPressed
        if (Input.GetButton("Start1"))
        {
            if(player1 == null)
            {
                player1 = Instantiate(player);
                player1.GetComponent<Control>().playerNum = 1;
                player1.GetComponent<Animator>().runtimeAnimatorController = animation1;
                cam.following[0] = player1.transform;
            }
            else if (!player1.activeSelf)
            {
                player1.SetActive(true);
                player1.GetComponent<Control>().ResetHealth();
                cam.following[0] = player1.transform;
            }
        }
        if (Input.GetButton("Start2"))
        {
            if (player2 == null)
            {
                player2 = Instantiate(player);
                player2.GetComponent<Control>().playerNum = 2;
                player2.GetComponent<Animator>().runtimeAnimatorController = animation2;
                cam.following[1] = player2.transform;
            }
            else if (!player2.activeSelf)
            {
                player2.SetActive(true);
                player2.GetComponent<Control>().ResetHealth();
                cam.following[1] = player2.transform;
            }
        }
        #endregion
        #region notActive
        if (player1 && !player1.activeSelf)
        {
            cam.following[0] = null;
        }
        if (player2 && !player2.activeSelf)
        {
            cam.following[1] = null;
        }
        #endregion
    }
}
