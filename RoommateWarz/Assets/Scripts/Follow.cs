using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public Camera cam;
    public float MINIMUM_SIZE = 2.5f;
    public Transform[] following;
    private float buffer = 2f;
	
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

        float width = maxX - minX;
        float widthHeight = width / cam.aspect;
        float height = maxY - minY;
        if(widthHeight > height)
        {
            cam.orthographicSize = Mathf.Max((widthHeight / cam.aspect) + buffer , MINIMUM_SIZE);
        }
        else
        {
            cam.orthographicSize = Mathf.Max((height/2) + buffer, MINIMUM_SIZE);
        }

        
        transform.position = new Vector3((maxX + minX)/2,(maxY + minY)/2, -10);
    }
}
