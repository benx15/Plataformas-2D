using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pincho : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Jugador jugador = other.GetComponent<Jugador>();
            if (jugador != null)
            {
                jugador.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                jugador.transform.position = jugador.spawnPoint.position;
            }
        }
    }
}