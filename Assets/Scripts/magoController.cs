using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class magoController : MonoBehaviour
{
    private float speed, raio = 0.2f;
    private bool viradoDireita, isGrounded, doubleJump;
    private int moeda = 0, totalMoedas;

    private Rigidbody2D rb;
    private Animator anim;
    public Transform groundCheck;
    private BoxCollider2D bc;
    public GameObject aberta, fechada;

    public LayerMask whatIsGround;
    private Scene cena;

    // GameObject[] coins = GameObject.FindGameObjectsWithTag("Moeda");

    public Text texto;

    void UpdateCoinText ()
    {
        texto.text = "Tens " + moeda + "/" + totalMoedas + " Moedas";
    }

    // Start is called before the first frame update
    void Start()
    {
        totalMoedas = GameObject.FindGameObjectsWithTag("Moeda").Length;
        UpdateCoinText();

        /*

        text = FindFirstObjectByType<TextMeshPro>();
        UpdateText("asfkasjdfksdnfks");
        */
        // coinCount = coins.Length;
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


        UpdateCoinText();

        anim.SetBool("DuploSalto", false);

        

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
            if (!isGrounded)
            {
               
                anim.SetBool("DuploSalto", true);
               // doubleJump = false;
            } 
            rb.AddForce(new Vector2(0.0f, 500.0f));
        }
            
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, raio,whatIsGround);
        float mov = Input.GetAxis("Horizontal");
        if (isGrounded)
        {
            doubleJump = true;
            
        }
        

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
            moeda++;
            if(moeda == totalMoedas)
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
