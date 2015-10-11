using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
    public Rigidbody2D body;
    public float speed = 5;
    private float spawndist = .3f;
    public GameObject bulletType;
    public int playerNum;
    private float health = 100;
    private bool didFire = false;

    void FixedUpdate() {
        float x = Input.GetAxis("Horizontal"+playerNum),
              y = Input.GetAxis("Vertical"+playerNum);

        //Movement
        if(x != 0 || y != 0) {
            float heading = Mathf.Atan2(-x, y);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, heading * Mathf.Rad2Deg);
            body.velocity = new Vector2(x, y) * speed;
            body.angularVelocity *= 0;
        }
        else if(body.velocity.x != 0 || body.velocity.y != 0){
            body.velocity /= 2;
            if(body.velocity.x < .01 && body.velocity.y < .01) {
                body.velocity *= 0;
            }
            body.angularVelocity *= 0;
        }

        //Fireing
        float fire = Input.GetAxis("Fire"+playerNum);
        if (fire > 0 && !didFire) {
            didFire = true;

            float aimX = Input.GetAxis("AimHorizontal" + playerNum),
                  aimY = Input.GetAxis("AimVertical" + playerNum);
            
            //don't fire without direction
            if (aimX == 0 && aimY == 0) {
                return;
            }

            Vector3 direction = new Vector3(aimX, aimY, 0);
            direction.Normalize();
            direction *= spawndist;
            GameObject bullet = (GameObject)Instantiate(bulletType, transform.position + direction, new Quaternion());

            bullet.tag = "Bullet:" + playerNum;

            Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
            bulletBody.velocity = new Vector2(aimX * speed * 2, aimY * speed * 2) + body.velocity;
            bulletBody.angularVelocity = 15;
        }
        else if (fire == 0 && didFire) {
            didFire = false;
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;
        string[] tags = collider.tag.Split(':');
        if (tags.Length > 1 && tags[0] == "Bullet" && tags[1] != playerNum.ToString()) {
            float speed = collider.GetComponent<Rigidbody2D>().velocity.magnitude;
            if(speed > 5) {
                health -= speed;
                if(health <= 0) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
