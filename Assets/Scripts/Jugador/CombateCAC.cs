using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CombateCAC : MonoBehaviour
{
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float dañoGolpe;
    [SerializeField] private float tiempoEntreAtaques;
    [SerializeField] private float tiempoSiguenteAtaque;
    [SerializeField] private float vida;
    [SerializeField] private float maximoVida;
    [SerializeField] private BarraDeVida barraDeVida;
    [SerializeField] private float tiempoPerdidaControl;
    [SerializeField] private float vidaRecuperadaPorEnemigo;
    [SerializeField] private float porcentajeVidaCuracionJefe = 20f;
    private Animator animator;
    public event EventHandler MuerteJugador;
    private Rigidbody2D rb2D;
    private MovimientoJugador movimientoJugador;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        vida = maximoVida;
        barraDeVida.InicializarBarraDeVida(vida);
        movimientoJugador = GetComponent<MovimientoJugador>();
    }

    private void Update()
    {
        if (tiempoSiguenteAtaque > 0)
        {
            tiempoSiguenteAtaque -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && tiempoSiguenteAtaque <= 0)
        {
            Golpe();
            tiempoSiguenteAtaque = tiempoEntreAtaques;
        }
    }

    private void Golpe()
    {
        animator.SetTrigger("Golpe");

        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        bool curarPorGolpeAlJefe = (vida / maximoVida) <= (porcentajeVidaCuracionJefe / 100f);

        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemigo"))
            {
                Enemigo enemigo = colisionador.transform.GetComponent<Enemigo>();
                if (enemigo != null)
                {
                    enemigo.TomarDaño(dañoGolpe);
                    RecuperarVidaPorEnemigo();
                }
                if (vida / maximoVida <= porcentajeVidaCuracionJefe / 100f)
                {
                    RecuperarVidaPorJefe();
                }
                else
                {
                    colisionador.transform.GetComponent<Jefe>().TomarDaño(dañoGolpe);
                }
            }
            else if (colisionador.CompareTag("Jefe"))
            {
                colisionador.transform.GetComponent<Jefe>().TomarDaño(dañoGolpe);
                RecuperarVidaPorEnemigo();
            }
        }
    }

    public void TomarDaño(float daño, Vector2 posicion)
    {
        vida -= daño;
        barraDeVida.CambiarVidaActual(vida);
        if (vida > 0)
        {
            animator.SetTrigger("Golpe");
            movimientoJugador.Rebote(posicion);
            StartCoroutine(PerderControl());
            StartCoroutine(DesactivarColision());
        }
        else
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetTrigger("Muerte");
            StartCoroutine(EsperarAntesDeCargarEscena());
        }
    }

    private void RecuperarVidaPorEnemigo()
    {
        vida += vidaRecuperadaPorEnemigo;
        vida = Mathf.Clamp(vida, 0f, maximoVida);
        barraDeVida.CambiarVidaActual(vida);
    }

    private void RecuperarVidaPorJefe()
    {
        vida += vidaRecuperadaPorEnemigo;
        vida = Mathf.Clamp(vida, 0f, maximoVida);
        barraDeVida.CambiarVidaActual(vida);
    }

    private IEnumerator DesactivarColision()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        yield return new WaitForSeconds(tiempoPerdidaControl);
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    private IEnumerator PerderControl()
    {
        movimientoJugador.sePuedeMover = false;
        yield return new WaitForSeconds(tiempoPerdidaControl);
        movimientoJugador.sePuedeMover = true;
    }

    private IEnumerator EsperarAntesDeCargarEscena()
    {
        yield return new WaitForSeconds(1.0f);
        Physics2D.IgnoreLayerCollision(6, 7, true);
        SceneManager.LoadScene(3);
    }


    public void Destruir()
    {
        Destroy(gameObject);
    }

    public void MuerteJugadorEvento()
    {
        MuerteJugador?.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
