using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    [SerializeField]
    float tolerance = 0;
    [SerializeField]
    float baseSpeed = 0;
    [SerializeField]
    float gravity = 1;


    public enum PhysicsState {
        Grounded,
        Jumping,
        Falling,
    }
    public PhysicsState State = PhysicsState.Falling;

    private Rigidbody2D rb;
    private Collider2D collider;

    private RainbowAnim groundRainbow;

    private float fallCastDist = 0.05f;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        // Detect obstacles below
        // Detect obstacles in front
        // Save current velocity
        // Compute walking contribution
        // Compute jumping contribution
        // Compute gravity contribution


		if(State == PhysicsState.Grounded) {
            float horizontal = Input.GetAxis("Horizontal");
            if (Mathf.Abs(horizontal) > tolerance) {
                float sign = Mathf.Sign(horizontal);
                float magnitude = Mathf.Abs(baseSpeed * horizontal);
                rb.velocity = baseSpeed * horizontal * Vector2.right;
                // Anticipate collisions
                RaycastHit2D[] hits = new RaycastHit2D[1];
                // Ignore sibling colliders
                //collider.Cast(rb.velocity.normalized, hits, rb.velocity.magnitude * Time.deltaTime, true);
                //foreach (RaycastHit2D hit in hits) {
                //    rb.velocity -= (Vector2.Dot(hit.normal, rb.velocity) * hit.normal);
                //}
                if (collider.Cast(rb.velocity.normalized, hits, rb.velocity.magnitude * Time.deltaTime, true) > 0) {
                    Vector2 surfaceDir = new Vector2(-hits[0].normal.y, hits[0].normal.x);
                    rb.velocity = -sign * surfaceDir * magnitude;
                    Debug.DrawLine(hits[0].point, hits[0].point + surfaceDir, Color.red, 10);
                } else {
                    rb.velocity = rb.velocity.normalized * magnitude;
                }


            } else if (Input.GetButtonDown()) {

            } else {
                rb.velocity = Vector2.zero;
            }
            // Fall if nothing under character
            RaycastHit2D[] fallHits = new RaycastHit2D[1];            
            if (collider.Cast(Vector2.down, fallHits, fallCastDist, true) == 0) {
                State = PhysicsState.Falling;
            }
        } else if(State == PhysicsState.Jumping) {
            // TODO
            
        } else if(State == PhysicsState.Falling) {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            if (collider.Cast(Vector2.down, hits, fallCastDist, true) > 0) {
                State = PhysicsState.Grounded;
            } else {
                Vector2 fallVel = Vector2.down * gravity * Time.deltaTime;
                rb.velocity += fallVel;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        // Remove inciding components from rigidbody velocity
        foreach (ContactPoint2D contact in col.contacts) {            
            rb.velocity -= (Vector2.Dot(contact.normal, rb.velocity) * contact.normal);
        }
        RainbowAnim other = col.gameObject.GetComponent<RainbowAnim>();
        if (other) {
            groundRainbow = other;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        RainbowAnim other = col.gameObject.GetComponent<RainbowAnim>();
        if (other != null && other == groundRainbow) {
            groundRainbow = null;
        }
    }
}
