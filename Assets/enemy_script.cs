using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class enemy_script : MonoBehaviour
{  
    public int maxHealth=5;
    public bool facingLeft = true;
     public float moveSpeed=2f;
    public Transform checkPoint;
    public float distance=1f;
    public LayerMask layerMask;
    public bool inRange = false;
    public Transform player;
    public float attackRange = 10f;
    public float retrieveDistance=2.5f;
    public float chaseSpeed = 4f;
    public Animator animator;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<gameManager>().isGameActive==false){
            return;
        }
        if(maxHealth<=0){
            Die();
        }
        if(Vector2.Distance(transform.position, player.position) <= attackRange){
            inRange=true;
       }
        else{
            inRange=false;      
        }
        if (inRange){
            if(player.position.x>transform.position.x && facingLeft==true){
                transform.eulerAngles=new Vector3(0,-180,0);
                facingLeft=false;
            }
            else if(player.position.x<transform.position.x && facingLeft==false){
                transform.eulerAngles=new Vector3(0,0,0);
                facingLeft=true;

            }
            //Debug.Log("Chase Player");
           if(Vector2.Distance(transform.position,player.position)> retrieveDistance){
            animator.SetBool("Attack", false); // Disattiva l'attacco

              transform.position = Vector2.MoveTowards(transform.position,player.position,chaseSpeed * Time.deltaTime);
           }
           else{
                animator.SetBool("Attack", true); // Disattiva l'attacco

           }


        }else{
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);
        
        RaycastHit2D hit= Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

        if(hit==false && facingLeft){
            transform.eulerAngles= new Vector3(0,-180,0);
            facingLeft=false;
            Debug.Log("Flip Enemy");
        }else if(hit==false && facingLeft==false){
             transform.eulerAngles= new Vector3(0,0,0);
            facingLeft=true;

        }

        }

    
        
        
    }
    public void Attack(){
        Collider2D collInfo=Physics2D.OverlapCircle(attackPoint.position, attackRadius,attackLayer);
        if(collInfo){
            //Debug.Log(collInfo.transform.name);
            if(collInfo.gameObject.GetComponent<Player>()!=null){
                collInfo.gameObject.GetComponent<Player>().TakeDamage(1);
            }
        }
    }
    public void TakeDamage(int damage){
        if(maxHealth<=0){
            return;
        }
        maxHealth -= damage;
    }
    private void OnDrawGizmosSelected(){
        if(checkPoint == null){
            return;
        }
        Gizmos.color=Color.blue;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance );
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        if(attackPoint==null) return;
        Gizmos.color=Color.red;
        Gizmos.DrawSphere(attackPoint.position,attackRadius);


    }
    void Die(){
        Debug.Log(this.transform.name + "Died.");
        Destroy(this.gameObject);
    }
}
