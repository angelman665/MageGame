using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class magoController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;
    private bool viradoDireita;
    private Animator anim;
    public bool isGrounded;
    public Transform groundCheck;
    public float raio = 0.2f;
    public LayerMask whatIsGround;
    private BoxCollider2D bc;
    private bool doubleJump;
    public GameObject aberta, fechada;
    private int moeda = 2;
    private Scene cena;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        speed = 10.0f;
        viradoDireita = true;
        aberta.SetActive(false);
        fechada.SetActive(true);
        cena = SceneManager.GetActiveScene();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            anim.SetBool("Baixar", true);
            bc.offset = new Vector2(0.03f, -0.1f);
            bc.size = new Vector2(0.3f, 0.20f);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            anim.SetBool("Baixar", false);
            bc.offset = new Vector2(0.03f, 0.0f);
            bc.size = new Vector2(0.3f, 0.32f);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && (isGrounded || doubleJump))
        {
            if (!isGrounded) doubleJump = false;
            rb.AddForce(new Vector2(0.0f, 500.0f));
            anim.SetBool("DuploSalto", true);
        }
            
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, raio,whatIsGround);
        float mov = Input.GetAxis("Horizontal");
        if (isGrounded) doubleJump = true;
        anim.SetFloat("HSpeed", Mathf.Abs(mov));
        rb.velocity = new Vector2(mov * speed, rb.velocity.y);
        if ((viradoDireita && mov < 0) || (!viradoDireita && mov >0)) Flip();
    }

    void Flip()
    {
        viradoDireita = !viradoDireita;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Moeda"))
        {
            Destroy(collision.gameObject);
            moeda--;
            if(moeda == 0)
            {
                fechada.SetActive(false);
                aberta.SetActive(true);
            }
        }
        if (collision.gameObject.CompareTag("Porta"))
        {
            if (cena.buildIndex == 2) SceneManager.LoadScene(0);
            else SceneManager.LoadScene(cena.buildIndex + 1);
        }
    }
}
