using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public Transform following;
    public float buffer = 5;
	
	// Update is called once per frame
	void Update () {
        float xDiff = transform.position.x - following.position.x,
              yDiff = transform.position.y - following.position.y;
        if(xDiff < -buffer) {
            xDiff = -buffer;
        }
        else if(xDiff > buffer) {
            xDiff = buffer;
        }

        if(yDiff < -buffer) {
            yDiff = -buffer;
        }
        if(yDiff > buffer) {
            yDiff = buffer;
        }
        transform.position = new Vector3(following.position.x + xDiff, following.position.y + yDiff, -10);
    }
}
