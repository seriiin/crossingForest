using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public int gems;
    public int distance;

    float hAxis;
    float vAxis;
    
    bool wDown;
    bool jDown;
    bool a1Down; //attack1

    bool isJump;
    bool isDead;

    Vector3 moveVec;
    Rigidbody rigid;

    public Camera followCamera;
    public GameManager manager;

    Animator anim;

    void Awake()
    {
        isJump = false;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        Debug.Log(PlayerPrefs.GetInt("MaxScore"));
        //PlayerPrefs.SetInt("MaxScore", 0);
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        distance = (int)(transform.position.z/2);
    }
    void GetInput()
    {
        //입력받은 키보드 값을 대입?
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    } 

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isDead)
            moveVec = Vector3.zero;
        else    
            transform.position += moveVec * speed * Time.deltaTime;
        
        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

    }

    void Turn()
    {
        //#1. 키보드에 의한 회전
        transform.LookAt(transform.position + moveVec);

    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero && !isJump &&!isDead){
            rigid.AddForce(Vector3.up * 13, ForceMode.Impulse);
            isJump = true;
        }
    }

    void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Ground")
			isJump = false;
        else if (collision.gameObject.tag == "Animal"){
            Debug.Log("animal");
            anim.SetTrigger("doDie");
            isDead = true;
            manager.GameOver();
        }
	}


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gems"){
            Gems gem = other.GetComponent<Gems>();
            gems += gem.value;
            Destroy(other.gameObject);
        }
        
        
    }

    private void OnEventFx(GameObject InEffect)
    {
        GameObject newSpell = Instantiate(InEffect, new Vector3(transform.position.x,transform.position.y + 1.7f, transform.position.z),Quaternion.identity);

        Destroy(newSpell, 1.0f);
    }

    
}
