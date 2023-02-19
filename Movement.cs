using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// import statements

// MonoBehaviour allows us to attach a script to the object
public class Movement : MonoBehaviour {
    public Rigidbody2D rb;
    public DistanceJoint2D rope;
    public LineRenderer line;
    // gives the sprite physics attributes (velocity, acceleration, gravity)

    public LayerMask ropeLayerMask;
    // What is the thing. Literally. What can the raycast hit

    public float jumpPower = 1f;
    public float speed = 10f;
    public float distance = 90f;
    bool hookExtended = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find the component of RigidBody in the "circle object"
        rb = GetComponent<Rigidbody2D>(); 
        rope = GetComponent<DistanceJoint2D>();
        // we have a rigi d pole between two objects and the pole is a certain legth and that is not going to change and these 
        // objects can move however much they want as long as the poles distance do not change.
        // component called spring joint
        line = GetComponent<LineRenderer>();
        // draws a line between two points can also draw any curve you want

        rope.enabled = false;
        line.enabled = false;
    }

    // Update is called once per frame
    void Update() {

        // check if object is on the ground
        float xMove = Input.GetAxisRaw("Horizontal");
        // initializes the a and d keys
        // float beacase a key has value -1, d value has value 1
        float yMove = Input.GetAxisRaw("Vertical");
        // float because w key has value 1, s value has value -1
        // initializes the w and s keys
        rb.velocity = new Vector2(xMove * speed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        line.SetPosition(0, transform.position);
        // 0 is start of line, where we want the start line to be
        // but transform.position is in Unity (the center of the character)
        // line[0] = transform.position; but transform.position refers to where the character is

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Input.MousePosition is the screen coordinates (in real life) converts it to mousePositoin (coordinates of the game)
        // Input.mousePosition comes from the camera.
        // gives coordinates of the mouse in the world (game)
        Vector2 lookDirection = mousePosition - (Vector2) transform.position;
        // this is a vector from the transfomr.position to mousePosition (Think vector AB, B - A)
        // based on what mousePosition gave us (coordinates of the game), subtracting the center of the character, we are looking
        // in the direction of where that cursor (in the game) happens to be.

        // 0 is left click, 1 is right click, 2 is the scroller
        if (Input.GetMouseButtonDown(0) && (!hookExtended)) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDirection, distance, ropeLayerMask);
            // Imagine a first person shooter game where you shoot a bullets thats REALLY REALLY REALLY FAST. It refers to the point at which you hit something
            // This function returns the point you hit, what you hit, the distance from where you hit and more

            // if its not nothing, that means something so this runs if you hit something
            if (hit.collider != null) {
                hookExtended = true;
                EnableRope(hit);
            }
        } else if (Input.GetMouseButtonDown(0) && hookExtended) {
            hookExtended = false;
            DisableRope();
        }
    }

    void EnableRope(RaycastHit2D hit) {
        rope.enabled = true;
        rope.connectedAnchor = hit.point;
        // the end point of the rope in which it hits something (a point)
        // the end point changes whenever you grab something new
        // the grappling hook's starting point is you so it never changes

        line.enabled = true;
        line.SetPosition(1, hit.point);
    }

    void DisableRope() {
        rope.enabled = false;
        line.enabled = false;
    }
}