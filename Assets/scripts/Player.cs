using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Text coinAmount;
    public int currentCoin = 0;
    public int maxHealth = 3;
    public Text health;
    public Animator animator;
    public Rigidbody2D rb;
    public float jumpHeight = 5f;
    public float moveSpeed = 1f; // Velocità fissata a 1
    private bool facingRight = true;

    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    public int jumpCount = 1;
    public int maxJumps = 2;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    private Vector2 touchStartPos;
    private bool isTouching = false;

    private bool isAttacking = false; // Nuova variabile per monitorare se l'attacco è in corso

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (maxHealth <= 0)
        {
            Die();
        }

        coinAmount.text = currentCoin.ToString();
        health.text = maxHealth.ToString();

        // Controllo touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isTouching = true;

                // Se tocca la parte destra dello schermo e non stiamo già attaccando
                if (touch.position.x > Screen.width / 2)
                {
                    Attack();
                }
            }

            if (touch.phase == TouchPhase.Moved && isTouching)
            {
                float touchDeltaX = touch.position.x - touchStartPos.x;
                float touchDeltaY = touch.position.y - touchStartPos.y;

                // Movimento orizzontale
                if (Mathf.Abs(touchDeltaX) > Mathf.Abs(touchDeltaY))
                {
                    float direction = touchDeltaX > 0 ? 1f : -1f;
                    rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

                    // Flip del personaggio
                    if (direction < 0 && facingRight)
                    {
                        Flip();
                    }
                    else if (direction > 0 && !facingRight)
                    {
                        Flip();
                    }
                }

                // Salto solo trascinando verso l'alto
                if (touchDeltaY > 50 && jumpCount < maxJumps)
                {
                    Jump();
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                isTouching = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Ferma il movimento orizzontale
            }
        }

        // Controllo se il player è a terra
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (isGrounded)
        {
            jumpCount = 1;
            animator.SetBool("Jump", false);
        }

        // Aggiornamento animazioni
        animator.SetFloat("Run", Mathf.Abs(rb.linearVelocity.x) > 0.1f ? 1f : 0f);
    }

    void Jump()
    {
        jumpCount++;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        animator.SetBool("Jump", true);
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.eulerAngles = new Vector3(0f, facingRight ? 0f : 180f, 0f);
    }

    public void Attack()
    {
        isAttacking = true; // Impostiamo su true quando l'attacco inizia
        animator.SetTrigger("Attack");

        Collider2D collinfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collinfo && collinfo.gameObject.GetComponent<enemy_script>() != null)
        {
            collinfo.gameObject.GetComponent<enemy_script>().TakeDamage(1);
        }
    }

    // Verifica se l'animazione di attacco è finita
    void OnAttackAnimationEnd()
    {
        isAttacking = false; // Reset isAttacking quando l'animazione finisce
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damage;
    }

    void Die()
    {
        Debug.Log("Player Died.");
        FindObjectOfType<gameManager>().isGameActive = false;
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            currentCoin++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            Destroy(other.gameObject, 1f);
        }
        if (other.gameObject.tag == "victoryPoint"&&currentCoin==10)
        {
            Debug.Log("Victory");
            SceneManager.LoadSceneAsync("end");
        }
    }
}
