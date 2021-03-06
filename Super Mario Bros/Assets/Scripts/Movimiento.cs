﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour {

    //VARIABLES DE MOVIMIENTO
    public float velX = 0.03f; //velocidad en x
    public float movX; //movimiento en x
    public float inputX;
    public bool mirandoDerecha;

    //VARIABLES DE SALTO
    public float fuerzaSalto = 300f;
    public Transform pie; //para saber cuando esta tocando el suelo
    public float radioPie; //para saber cuando esta tocando el suelo
    public LayerMask suelo;//para saber cuando esta tocando el suelo
    public bool enSuelo;//para saber cuando esta tocando el suelo

    //VARIABLES DE AGACHADO
    public bool agachado;

    //VARIABLES MIRAR ARRIBA
    public bool mirarArriba;

    //VARIABLES DE CAIDA
    Rigidbody2D rb;
    public float caida; //variable para medir la velocidad de caida

    //VARIABLES DE DERRAPE
    public int derrape;
    public int derecha;
    public int izquierda;

    //VARIABLES CORRER
    public bool correr;

    //VARIABLES TURBO
    public bool turbo;
    public int contandoTurbo;

    //VARIABLES TURBOSALTO
    public bool turboSalto;

    //VARIABLES CONCHA

    public float patada = 500f;
    public Transform mano; //para saber cuando esta tocando el suelo
    public float radioMano; //para saber cuando esta tocando el suelo
    public LayerMask concha;//para saber cuando esta tocando el suelo
    public bool cogerConcha;//para saber c
    public GameObject Concha;
    public GameObject Mario;

    //VARIABLES DE ANIMACIONES
    Animator animator;


    private void Awake()//igual que Start pero se ejecuta antes de que empiece el juego
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start() {

    }

    void FixedUpdate()
    { //igual que Update, pero se ejecuta cada 2 centesimas de segundos, en vez de cada frame


        //MOVIMIENTO

        inputX = Input.GetAxis("Horizontal"); //Almacena el movimiento en el eje X.
        movX = transform.position.x + (inputX * velX); //movX será igual a mi posicion en X + el movimiento en el eje X * velX 

        if (!agachado && !mirarArriba) //solo se podrá mover el personaje cuando no estemos agachados y no estemos mirando hacia arriba
        {
            if (inputX > 0) //si el personaje se mueve para la derecha..
            {
                transform.position = new Vector2(movX, transform.position.y); //lo movemos
                transform.localScale = new Vector2(1, 1); // que mire hacia la derecha
                mirandoDerecha = true;
                animator.SetBool("derrape", false);
            }

            if (inputX < 0) //si el personaje se mueve para la izquierda
            {
                transform.position = new Vector2(movX, transform.position.y); //lo movemos
                transform.localScale = new Vector2(-1, 1); // con la escala hacemos que cuando se mueva para la izquierda, el personaje mire en esa direccion.
                mirandoDerecha = false; //cuando sea false, estará mirando hacia la izquierda
                animator.SetBool("derrape", false);
            }


            if (inputX != 0 && enSuelo) //si nos estamos moviendo y estamos en el suelo...
            {
                animator.SetFloat("velX", 1); //llamamos a la variable velX que creamos en el animator de Unity y le ponemos 1 para que pase a la animacion de andar
            }
            else
            {
                animator.SetFloat("velX", 0); //llamamos a la variable velX que creamos en el animator de Unity y le ponemos 0 para que pase a la animacion de estar quieto
            }
        }

        //SALTO

        enSuelo = Physics2D.OverlapCircle(pie.position, radioPie, suelo); //enSuelo será True cuando el personaje esté tocando el suelo.

        if (enSuelo) //si esta pisando el suelo...
        {
            animator.SetBool("enSuelo", true); //Le digo al animador que estoy en el suelo
            animator.SetBool("turboSalto", false);

            if (Input.GetKeyDown(KeyCode.Space) && !agachado) //si pulsamos espacio y no estamos agachados...
            {
                rb.AddForce(new Vector2(0f, fuerzaSalto)); //Accedo a la velocidad del Rigidbody2D y le añado la fuerza vertical establecida en fuerzaSalto
                animator.SetBool("enSuelo", false); //ya no estaremos en el suelo
            }


        }
        else
        {
            animator.SetBool("enSuelo", false);
        }



        //AGACHADO
        if (enSuelo && Input.GetKey(KeyCode.S)) //si el personaje está en el suelo y pulsamos "S"..
        {
            animator.SetBool("agachado", true);
            agachado = true;

        }
        else
        {
            animator.SetBool("agachado", false);
            agachado = false;
        }

        //MIRAR ARRIBA
        if (inputX == 0) //solo mirará hacia arriba cuando el personaje esté quieto
        {
            if (enSuelo && Input.GetKey(KeyCode.W)) //si estamos en el suelo y pulsamos W...
            {
                animator.SetBool("mirarArriba", true);
                mirarArriba = true;
            }
            else
            {
                animator.SetBool("mirarArriba", false);
                mirarArriba = false;
            }
        }

        //CAIDA
        caida = rb.velocity.y; //le asignamos la velocidad de caida en el eje Y

        if (caida != 0 || caida == 0) //Tanto si caida es igual o distinta de 0 le paso la velocidad al animador
        {
            animator.SetFloat("velY", caida);
        }

        //DERRAPE

        if (inputX == 0)// Si no me muevo o estoy cambiando de direccion

        {
            StartCoroutine(TiempoEspera()); //llamo a la corrutina "Tiempo de espera" para que me resetee todos los valores de derrape
        }

        //DERRAPE DE DERECHA A IZQUIERDA

        if (inputX > 0.5f)
        {
            derecha = 1;
            derrape = 1;
        }

        if (derrape == 1 && Input.GetKey(KeyCode.A))
        {
            animator.SetBool("derrape", true);
            animator.SetBool("turbo", false);
            StartCoroutine(TiempoEspera());
            StopCoroutine(Turbo());
        }

        //DERRAPE DE IZQUIERDA A DERECHA
        if (inputX < 0f)
        {
            izquierda = 1;
            derrape = -1;
        }

        if (derrape == -1 && Input.GetKey(KeyCode.D))
        {
            animator.SetBool("derrape", true);
            animator.SetBool("turbo", false);
            StartCoroutine(TiempoEspera());
            StopCoroutine(Turbo());
        }

        //CORRER
        if (inputX != 0) //Si me estoy moviendo
        {
            if (Input.GetKey(KeyCode.Z) && enSuelo) //y pulso Z y estoy en el suelo...
            {
                correr = true;
                velX = 0.06f;
                animator.SetBool("correr", true);


            }
            else
            {
                velX = 0.03f;
                correr = false;

                animator.SetBool("correr", false);
                contandoTurbo = 0; //Reinicio el contador de turbo

            }
        }
        else // si no me estoy moviendo...

        {
            correr = false;
            animator.SetBool("correr", false);
            animator.SetBool("derrape", false);
            contandoTurbo = 0; //Reinicio el contador de turbo

        }

        //TURBO
        if (Input.GetKey(KeyCode.Z) && correr && enSuelo) //Si estoy pulsando Z, estoy corriendo y estoy en el suelo
        {
            StartCoroutine(Turbo());//Llamo a la corrutina Turbo()

        }
        else
        {

            turbo = false;
            animator.SetBool("turbo", false);
            StopAllCoroutines();//Detengo las corrutinas

        }


        //TURBOSALTO
        if (inputX != 0 && turbo) //si me estoy moviendo...
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("turboSalto", true);


            }


            else
            {
                animator.SetBool("turboSalto", false);
            }
        }

            //CONCHA
            cogerConcha = Physics2D.OverlapCircle(mano.position, radioMano, concha);
            if (cogerConcha && (mirandoDerecha || !mirandoDerecha))
            {
                if (Input.GetKey(KeyCode.Z))
                {
                    Concha.transform.parent = Mario.transform; // los movimientos de la concha y mario seran los mismos
                    Concha.GetComponent<Rigidbody2D>().gravityScale = 0; //para que no le afecte la graveda
                    Concha.GetComponent<Rigidbody2D>().isKinematic = true; //para que no le afecte las fuerzas

                }
                else
                {
                    if (mirandoDerecha)
                    {
                        Concha.GetComponent<Rigidbody2D>().AddForce(new Vector2(patada, 0));

                    }
                    else
                    {
                        Concha.GetComponent<Rigidbody2D>().AddForce(new Vector2(-patada, 0));

                    }
                    Concha.transform.parent = null; //para que deje de ser hija de Mario
                    Concha.GetComponent<Rigidbody2D>().gravityScale = 2; //para que no le afecte la graveda
                    Concha.GetComponent<Rigidbody2D>().isKinematic = false; //para que no le afecte las fuerzas
                }
            }

        }

    

    //FIN del FixedUpdate

    public IEnumerator TiempoEspera() //este metodo se conoce como una corrutina.
    {
        yield return new WaitForSeconds(0.3f); //el codigo de abajo se ejecutará cuando pase 0.3 
        derrape = 0;
        derecha = 0;
        izquierda = 0;
        animator.SetBool("derrape", false);


    }
    public IEnumerator Turbo()
    {
       
        
       yield return new WaitForSeconds(0.8f);//espera 1.5 "segundos"
           
        

        turbo = true;//Activo turbo
        animator.SetBool("turbo", true);//Le digo al animador que estoy en turbo
        velX = 0.12f;//Establezco la velocidad en X a 0.12f
        


    }

}