using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    //Movement
    public float speed;
    public float jump;
    public float ladderSpeed;
    float moveVelocity;

    //Getters
    public Animator anim;
    private Rigidbody2D rb;
    private Collider2D other;
    //Grounded Vars
    bool grounded = true;
    bool ladder = false;

    //State machine
    enum States { idle, run, jump, fall, crouch, climb};
    States state;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = States.idle;
    }

    void Update()
    {
        Movement();
        StateMachine();
        Animation();
    }
    //Check if Grounded
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Floor")
        grounded = true;

        else if(other.tag == "Ladder")
        {
            Debug.Log("ladder");
            ladder = true;
            rb.gravityScale = 0;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Floor")
            grounded = false;

        else if(other.tag == "Ladder")
        {
            Debug.Log("ladder out");
            ladder = false;
            rb.gravityScale = 1;
        }
    }

    void StateMachine()
    {
        if (state == States.jump)
        {
            if (rb.velocity.y < Mathf.Epsilon)
            {
                state = States.fall;
            }
        }

        else if (state == States.fall)
        {
            if (grounded)
            {
                state = States.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 0)
        {
            state = States.run;
        }

        else if (rb.velocity.y < 1)
        {
            state = States.fall;
        }

    }

    void Movement()
    {
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump);
                state = States.jump;
            }
        }

        moveVelocity = 0;

        //Ladder
        if(Input.GetKey(KeyCode.W))
        {
            if(ladder)
            {
                rb.velocity = new Vector2(0, ladderSpeed);
                state = States.climb;
            }
        }

        if(Input.GetKey(KeyCode.S))
        {
            if(ladder)
            {
                rb.velocity = new Vector2(0, -ladderSpeed);
                state = States.climb;
            }

            state = States.crouch;
        }

        //Left Right Movement
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector2(-1, 1);
            moveVelocity = -speed;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector2(1, 1);
            moveVelocity = speed;
        }

        rb.velocity = new Vector2(moveVelocity, rb.velocity.y);

    }

    void Animation()
    {
        anim.SetInteger("state", (int)state);
    }
}

