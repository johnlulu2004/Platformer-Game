using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        //grab references
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {   

        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        //flip player when moving left or right to face the direction of movement
        if(horizontalInput>0.01f){
            transform.localScale = new Vector3(1,1,1);
        }
        else if(horizontalInput<-0.01f){
            transform.localScale = new Vector3(-1,1,1);
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        //set parameters for animator
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("onWall", onWall() && !isGrounded());

        //wall jump logic
        if(wallJumpCooldown > 0.2f){

            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)){
            Jump();
            }

            body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

            if(onWall() && !isGrounded()){
                body.gravityScale = 1.5f;
                body.velocity = Vector2.zero;

            }
            else{
            body.gravityScale = 2.5f;
            }
        }
        else{
            wallJumpCooldown += Time.deltaTime;
        }


    }

    private void Jump(){
        if(isGrounded()){
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("Jump");
        }

        else if (onWall() && !isGrounded()){

            if(horizontalInput == 0){

                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x),transform.localScale.y,transform.localScale.z);

            }
            else{
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            // anim.SetTrigger("Jump");
            }
            wallJumpCooldown = 0;
        }

    }



    private bool isGrounded(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(transform.localScale.x,0), .1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack(){
        return !onWall() && isGrounded();
    }

    public bool canjumpAttack(){
        return !onWall() && !isGrounded();
    }
}
