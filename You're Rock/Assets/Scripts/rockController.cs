using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockController : MonoBehaviour {
    private Rigidbody2D rb2d;
    public float moveForce = 365f;
    public float maxSpeed = 15f;
    // Use this for initialization
    void Start () 
	{
		
	}
    void FixedUpdate()
    {

    }
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float h = Input.GetAxis("Horizontal");

        if (h * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * h * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

    }
}
