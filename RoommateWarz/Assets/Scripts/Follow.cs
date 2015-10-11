using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public Camera cam;
    public float MINIMUM_SIZE = 2.5f;
    public Transform[] following;
    public float buffer = 2;
	
	// Update is called once per frame
	void Update () {
        float minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
        //find the boundaries
        foreach (Transform t in following) {
            if (t == null)
                continue;
            Vector2 pos = t.position;
            minX = Mathf.Min(pos.x, minX);
            maxX = Mathf.Max(pos.x, maxX);
            minY = Mathf.Min(pos.y, minY);
            maxY = Mathf.Max(pos.y, maxY);
        }

        cam.orthographicSize = Mathf.Max(Mathf.Max(maxX - minX, maxY - minY) /2 + buffer, MINIMUM_SIZE);
        transform.position = new Vector3((maxX + minX)/2,(maxY + minY)/2, -10);
    }
}
