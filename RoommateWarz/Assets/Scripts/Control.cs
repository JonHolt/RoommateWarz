using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour
{
    private Animator anim;

    public Rigidbody body;
    public int playerNum;
    public float speed = 5;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float x = -1 * Input.GetAxis("Horizontal" + playerNum),
              y = -1 * Input.GetAxis("Vertical" + playerNum);

        // Animations
        if (x == 0 && y == 0)
        {
            anim.SetBool("walking", false);
        }
        else {
            if (!anim.GetBool("walking"))
            {
                anim.SetBool("walking", true);
            }
            if (Mathf.Abs(y) > Mathf.Abs(x))
            {
                if (y > 0 && anim.GetInteger("direction") != 0)
                {
                    anim.SetInteger("direction", 0);
                }
                else if (y < 0 && anim.GetInteger("direction") != 2)
                {
                    anim.SetInteger("direction", 2);
                }
            }
            else if (x < 0 && anim.GetInteger("direction") != 1)
            {
                anim.SetInteger("direction", 1);
            }
            else if (x > 0 && anim.GetInteger("direction") != 3)
            {
                anim.SetInteger("direction", 3);
            }
        }

        // Movement
        if (x != 0 || y != 0)
        {
            body.velocity = new Vector3(x, 0, y) * speed;
        }
        else if (body.velocity.x != 0 || body.velocity.z != 0)
        {
            body.velocity /= 2;
            if (body.velocity.x < .01 && body.velocity.z < .01)
            {
                body.velocity *= 0;
            }
        }
    }
}
