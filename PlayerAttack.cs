using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private Animator anim;
    private PlayerMovement playerMovement;
    [SerializeField] private float attackCooldown;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float skill1Cooldown;
    private float skillTimer = Mathf.Infinity;
    private BoxCollider2D attackHitBox;


    // Start is called before the first frame update    
    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        attackHitBox = transform.Find("Attack Hitbox").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButton(0)) && cooldownTimer > attackCooldown && playerMovement.canAttack()){
            Attack();
        }
        else if((Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButton(0)) && cooldownTimer > attackCooldown && playerMovement.canjumpAttack()){
            jumpAttack();
        }
        else if((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButton(1)) && skillTimer > skill1Cooldown && playerMovement.canAttack()){
            Skill1();
        }
        cooldownTimer+=Time.deltaTime;
        skillTimer+=Time.deltaTime;
    }

    private void Attack(){
        anim.SetTrigger("Attack");
        cooldownTimer = 0;
        Invoke("ActivateHitbox", 0.2f); // Activate hitbox after 0.2 seconds.
        Invoke("DeactivateHitbox", 0.4f); // Deactivate hitbox after 0.4 seconds.
    }

    private void jumpAttack(){
        anim.SetTrigger("Jump Attack");
        cooldownTimer = 0;
        Invoke("ActivateHitbox", 0.2f); // Activate hitbox after 0.2 seconds.
        Invoke("DeactivateHitbox", 0.4f); // Deactivate hitbox after 0.4 seconds.
    }

    private void Skill1(){
        anim.SetTrigger("Skill1");
        skillTimer = 0;
    }
    void ActivateHitbox()
    {
        attackHitBox.gameObject.SetActive(true);
    }

    void DeactivateHitbox()
    {
        attackHitBox.gameObject.SetActive(false);
    }

}
