using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 3;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;

    Animator anim;
    Rigidbody rb;

    public Transform target;
	public Vector3 direction;
	public float velocity;
	public float default_velocity;
	public float accelaration;
	public Vector3 default_direction;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();


        accelaration = 0.1f;
		default_velocity = 0.1f;

        //StartCoroutine(MoveObject());
        anim.SetInteger("Walk", 1);
    }

    void Update () {
        
        this.transform.position = new Vector3 (transform.position.x + (default_direction.x * default_velocity),
			                                       transform.position.y+ (default_direction.y * default_velocity),
			                                       transform.position.z);
        if (this.transform.position.x > 13)
            Destroy(gameObject);
	}

    
}