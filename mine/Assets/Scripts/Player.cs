using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public float speed;

    public int distance;
    float hAxis;
    float vAxis;
    public Text result;
    bool wDown;
    bool jDown;
    bool a1Down; //attack1

    bool isJump;
    bool isDead;
    Vector3 moveVec;
    Rigidbody rigid;

    public Camera followCamera;
    public GameManager manager;
    public Player player;
    public Player player2;
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
        if (player.transform.position.z/2 < player2.transform.position.z/2)
        {
            if ((int)manager.distance < (int)player2.transform.position.z / 2)
            {
                manager.distance = (int)(player2.transform.position.z / 2);
            }
        }
        else
        {
            if ((int)manager.distance < (int)player.transform.position.z / 2)
            {
                manager.distance = (int)(player.transform.position.z / 2);
            }
        }
    }
    void GetInput()
    {
        if (this.gameObject.name == "Player")
        {
            hAxis = Input.GetAxisRaw("Horizontal2");
            vAxis = Input.GetAxisRaw("Vertical2");
            wDown = Input.GetButton("Walk");
            jDown = Input.GetButtonDown("Jump2");
        }
        else if (this.gameObject.name == "Player2")
        {
            hAxis = Input.GetAxisRaw("Horizontal1");
            vAxis = Input.GetAxisRaw("Vertical1");
            wDown = Input.GetButton("Walk");
            jDown = Input.GetButtonDown("Jump1");
        }
        //입력받은 키보드 값을 대입
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
        if(jDown  && !isJump &&!isDead){
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
            result.text = "Game Over!";
            manager.GameOver();
        }
	}


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gems"){
            Gems gem = other.GetComponent<Gems>();
            manager.gems += gem.value;
            Destroy(other.gameObject);
        }
        else if (other.tag == "Finish")
        {
            manager.getin++;
            if (this.tag == "Player")
            {
                player.gameObject.SetActive(false);
            }
            else if (this.tag == "Player2")
            {
                player2.gameObject.SetActive(false);
            }
            if (manager.getin == 2)
            {
                result.text = "Game Clear!";
                manager.getin = 0;
                manager.GameOver();
            }
        }
        
    }

    private void OnEventFx(GameObject InEffect)
    {
        GameObject newSpell = Instantiate(InEffect, new Vector3(transform.position.x,transform.position.y + 1.7f, transform.position.z),Quaternion.identity);

        Destroy(newSpell, 1.0f);
    }

    
}
