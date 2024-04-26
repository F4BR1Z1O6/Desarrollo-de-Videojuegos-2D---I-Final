using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float distanciaDeteccion;
    [SerializeField] private float distanciaParada;

    private Rigidbody2D rb;
    private Transform jugador;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    private void FixedUpdate()
    {
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador < distanciaDeteccion)
        {
            MoverHaciaJugador();
        }
        else
        {
            Detener();
        }
    }

    private void MoverHaciaJugador()
    {
        Vector2 direccion = (jugador.position - transform.position).normalized;
        rb.velocity = direccion * velocidad;
    }

    private void Detener()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);
    }
}

