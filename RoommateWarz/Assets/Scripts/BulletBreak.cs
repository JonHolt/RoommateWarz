using UnityEngine;
using System.Collections;

public class BulletBreak : MonoBehaviour {
    private int hits = 0;

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.tag != "Bullet") {
            ++hits;
            if(hits > 5) {
                Destroy(this.gameObject);
            }
        }
    }
}
