﻿using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	static class HealthBar {
		public static float WIDTH {
			get { return 100f; }
		}
		public static float HEIGHT {
			get { return 5f; }
		}
		public static float X_OFFSET {
			get { return 50f; }
		}
		public static float Y_OFFSET {
			get { return 0.6f; }
		}
	}
	private const float MAX_HEALTH = 100f;
	private Animator    anim;
	private bool        didFire = false;
	private float       health;
	private Transform   reticlePosition;
	private float       spawndist = .3f;

	public Rigidbody2D body;
	public GameObject  bulletType;
	public int         cooldown;
	public int         playerNum;
	public float       speed = 5;

    void Awake() {
		health = MAX_HEALTH;
        anim = GetComponent<Animator>();
		foreach (Transform child in transform) {
			if (child.name == "Reticle") {
				reticlePosition = child;
				break;
			}
		}
    }

	void OnGUI() {
		#region Health Bar
		// Find the players screen position
		Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + HealthBar.Y_OFFSET, transform.position.z);
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

		// Adjust for zoom
		float screenOffset = 2.7f / Camera.main.orthographicSize;
		Rect healthBarPosition = new Rect() {
			x = screenPosition.x - HealthBar.X_OFFSET * screenOffset,
			y = Screen.height - screenPosition.y - HealthBar.Y_OFFSET * screenOffset,
			width = HealthBar.WIDTH * screenOffset,
			height = HealthBar.HEIGHT
		};

		// Change color from green to red as player loses health
		float green = health / MAX_HEALTH;
		float red = 1f - green;
        GUI.color = new Color(red, green, 0f);
		
		// Draw to screen
		GUI.HorizontalScrollbar(healthBarPosition, 0, health, 0, MAX_HEALTH);
		#endregion
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

        // Animations
        if (x == 0 && y == 0) {
            anim.SetBool("walking", false);
        }
        else {
            if (!anim.GetBool("walking")) {
                anim.SetBool("walking", true);
            }
            if (Mathf.Abs(y) > Mathf.Abs(x)) {
                if (y < 0 && anim.GetInteger("direction") != 0) {
                    anim.SetInteger("direction", 0);
                }
                else if (y > 0 && anim.GetInteger("direction") != 2) {
                    anim.SetInteger("direction", 2);
                }
            }
            else if (x < 0 && anim.GetInteger("direction") != 1) {
				anim.SetInteger("direction", 1);
            }
			else if (x > 0 && anim.GetInteger("direction") != 3) {
				anim.SetInteger("direction", 3);
			}
		}

        // Movement
        if (x != 0 || y != 0) {
            body.velocity = new Vector2(x, y) * speed;
        }
        else if (body.velocity.x != 0 || body.velocity.y != 0) {
            body.velocity /= 2;
            if (body.velocity.x < .01 && body.velocity.y < .01) {
                body.velocity *= 0;
            }
        }

        // Firing
        float fire = Input.GetAxis("Fire"+playerNum);
        if (fire > 0 && (!didFire || cooldown == 0)) {
            didFire = true;

            float aimX = Input.GetAxis("AimHorizontal" + playerNum),
                  aimY = Input.GetAxis("AimVertical" + playerNum);

			// don't fire without direction
			if (aimX == 0 && aimY == 0) {
                return;
            }

            Vector3 direction = new Vector3(aimX, aimY, 0);
            direction.Normalize();
            direction *= spawndist;
            GameObject bullet = (GameObject)Instantiate(bulletType, transform.position + direction, new Quaternion());
			bullet.transform.rotation = Quaternion.LookRotation(transform.forward, direction);
            bullet.tag = "Bullet:" + playerNum;

            Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
            bulletBody.velocity = new Vector2(aimX * speed * 2, aimY * speed * 2) + body.velocity;
            bulletBody.angularVelocity = 15;
            cooldown = 20;
        }
        else if (fire == 0 && didFire) {
            didFire = false;
        }
        else if (didFire && cooldown > 0) {
            --cooldown;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;
		// Handle bullet collisions
        string[] tags = collider.tag.Split(':');
		if (tags.Length <= 1) {
			return;
		}

		// Avoid collisions with own bullets
		bool isBullet = (tags[0] == "Bullet");
		bool isPlayersBullet = (tags[1] == playerNum.ToString());
		if (isBullet && !isPlayersBullet) { 
            float speed = collider.GetComponent<Rigidbody2D>().velocity.magnitude;
            if(speed > 5) {
                health -= speed;
                if(health <= 0) {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void ResetHealth() {
        health = MAX_HEALTH;
    }
}
