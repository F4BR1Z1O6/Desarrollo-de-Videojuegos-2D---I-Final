using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jefe : MonoBehaviour
{
    private Animator animator;
    public Rigidbody2D rb2D;
    public Transform jugador;
    private bool mirandoDerecha = true;

    [Header("Vida")]
    [SerializeField] private float vida;
    [SerializeField] private float maximoVida;
    [SerializeField] private BarraDeVidaJefe barraDeVidaJefe;

    [Header("Ataque")]
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float dañoAtaque;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        vida = maximoVida;
        barraDeVidaJefe.InicializarBarraDeVida(vida);
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        float distanciaJugador = Vector2.Distance(transform.position, jugador.position);
        animator.SetFloat("distanciaJugador", distanciaJugador);
    }
    public void TomarDaño(float daño)
    {
        vida -= daño;
        barraDeVidaJefe.CambiarVidaActual(vida);
        if (vida <= 0)
        {
            StartCoroutine(MuerteConAnimacion());
        }
    }

    private void Ataque()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque);
        foreach (Collider2D collision in objetos)
        {
            if (collision.CompareTag("Player"))
            {
                Vector2 posicionDelAtaque = controladorAtaque.position; 
                collision.GetComponent<CombateCAC>().TomarDaño(dañoAtaque, posicionDelAtaque);
            }
        }
    }

    public void MirarJugador()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
    }

    private System.Collections.IEnumerator MuerteConAnimacion()
    {
        animator.SetTrigger("Muerte");
        yield return new WaitForSeconds(1.0f);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Ganaste");
    }
}
