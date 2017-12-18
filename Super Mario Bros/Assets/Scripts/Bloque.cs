using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloque : MonoBehaviour {


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Mario") //si lo que choca con el bloque es Mario...
        {
            this.gameObject.layer = 8; //cambiamos su capa a la 8 que es Suelo
        } else
        {
            this.gameObject.layer = 13;//cambiamos su capa a la 13 que es Items
        }
    }
}
