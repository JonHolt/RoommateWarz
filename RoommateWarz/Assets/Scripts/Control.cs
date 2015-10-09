using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
    public Rigidbody2D body;
    public float speed = 5;
    public float spawndist = .3f;
    public GameObject bulletType;

    private bool didFire = false;

    void FixedUpdate() {
        float x = Input.GetAxis("Horizontal") * speed,
              y = Input.GetAxis("Vertical") * speed;

        //Movement
        if(x != 0 || y != 0) {
            float heading = Mathf.Atan2(-x, y);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, heading * Mathf.Rad2Deg);
            body.velocity = new Vector2(x, y);
            body.angularVelocity *= 0;
        }
        else if(body.velocity.x != 0 || body.velocity.y != 0){
            body.velocity /= 2;
            if(body.velocity.x < .01 && body.velocity.y < .01) {
                body.velocity *= 0;
            }
        }

        //Fireing
        float fire = Input.GetAxis("Fire1");
        if (fire > 0 && !didFire) {
            didFire = true;

            float aimX = Input.GetAxis("AimHorizontal"),
                  aimY = Input.GetAxis("AimVertical");
            
            //determine direction
            if (aimX == 0 && aimY == 0) {
                return;
            }

            Vector3 direction = new Vector3(aimX, aimY, 0);
            direction.Normalize();
            direction *= spawndist;
            GameObject bullet = (GameObject)Instantiate(bulletType, transform.position + direction, new Quaternion());
            Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
            bulletBody.velocity = new Vector2(aimX * speed * 2, aimY * speed * 2);
            bulletBody.angularVelocity = 15;
        }
        else if (fire == 0 && didFire) {
            didFire = false;
        }
    }
}
