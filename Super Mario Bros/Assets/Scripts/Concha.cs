using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concha : MonoBehaviour {
    public float fuerzaRebote = 500f;
    public float velocidadConcha;
    public float impulso;
    public bool rebote;
    public Transform posicionRebote;
    public float radioConcha;
    public LayerMask suelo;
    public LayerMask objetoRebote;

    public GameObject Mario;
    public GameObject pieMario;

    public Transform pieConcha;
    public bool enSueloConcha;

    public bool cogeConcha;

     Animator animator;
    Rigidbody2D rb;
	
	void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	
	void FixedUpdate () {

        velocidadConcha = rb.velocity.x; // guardamos la velocidad
        rebote = Physics2D.OverlapCircle(posicionRebote.position, radioConcha, objetoRebote);
	}
}
