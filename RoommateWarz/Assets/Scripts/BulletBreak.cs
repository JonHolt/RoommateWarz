using UnityEngine;
using System.Collections;

public class BulletBreak : MonoBehaviour {
    public int MAX_HITS = 3;

    private int hits = 0;

    void OnCollisionEnter2D(Collision2D collision) {
        ++hits;
        if(hits > MAX_HITS) {
            Destroy(this.gameObject);
        }
    }
}
