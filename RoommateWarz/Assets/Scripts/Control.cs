﻿using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
    public Rigidbody2D body;
    public float speed = 5;
    private float spawndist = .3f;
    public GameObject bulletType;
    public int playerNum;
    private float health = 100;
    private bool didFire = false;
    private Animator anim;
	private Transform reticlePosition;
    public int cooldown;

    void Awake()
    {
        anim = GetComponent<Animator>();
		foreach (Transform child in transform) {
			if (child.name == "Reticle") {
				reticlePosition = child;
				break;
			}
		}
    }

	void Update() {
		float x = transform.position.x,
			  y = transform.position.y,
			  aimX = Input.GetAxis("AimHorizontal" + playerNum),
			  aimY = Input.GetAxis("AimVertical" + playerNum);
		Vector2 aimRadius = new Vector2(aimX, aimY);
		aimRadius.Normalize();

		// Set the reticle position
		reticlePosition.position = new Vector3(x + aimRadius.x, y + aimRadius.y, reticlePosition.position.z);
	}

    void FixedUpdate() {
        float x = Input.GetAxis("Horizontal"+playerNum),
              y = Input.GetAxis("Vertical"+playerNum);

        //animations
        if(x == 0 && y == 0)
        {
            anim.SetBool("walking", false);
        }
        else
        {
            if (!anim.GetBool("walking"))
            {
                anim.SetBool("walking", true);
            }
            if(Mathf.Abs(y) > Mathf.Abs(x))
            {
                if(y < 0 && anim.GetInteger("direction") != 0)
                {
                    anim.SetInteger("direction", 0);
                }
                else if(y > 0 && anim.GetInteger("direction") != 2)
                {
                    anim.SetInteger("direction", 2);
                }
            }
            else
            {
                if (x < 0 && anim.GetInteger("direction") != 1)
                {
                    anim.SetInteger("direction", 1);
                }
                else if(x > 0 && anim.GetInteger("direction") != 3)
                {
                    anim.SetInteger("direction", 3);
                }
            }
        }

        //Movement
        if(x != 0 || y != 0) {
            body.velocity = new Vector2(x, y) * speed;
        }
        else if(body.velocity.x != 0 || body.velocity.y != 0){
            body.velocity /= 2;
            if(body.velocity.x < .01 && body.velocity.y < .01) {
                body.velocity *= 0;
            }
        }

        //Fireing
        float fire = Input.GetAxis("Fire"+playerNum);
        if (fire > 0 && (!didFire || cooldown == 0)) {
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
            cooldown = 20;
        }
        else if (fire == 0 && didFire) {
            didFire = false;
        }
        else if (didFire && cooldown > 0)
        {
            --cooldown;
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
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void ResetHealth()
    {
        health = 100;
    }
}
