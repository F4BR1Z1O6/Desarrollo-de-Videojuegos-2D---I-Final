using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float vida;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TomarDaño(float daño)
    {
        vida -= daño;

        if (vida <= 0)
        {
            StartCoroutine(MuerteConAnimacion());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CombateCAC>().TomarDaño(20, other.GetContact(0).normal);
        }
    }

    private System.Collections.IEnumerator MuerteConAnimacion()
    {
        animator.SetTrigger("Muerte");
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
