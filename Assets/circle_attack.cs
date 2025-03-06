using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle_attack : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 10;  // Dégâts infligés par le cercle
    [SerializeField]
    private float lifetime = 1f;
    // Start is called before the first frame update
   
    public int GetDamage()
    {
        return damageAmount;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
        Debug.Log(gameObject.name + " sera détruit dans " + lifetime + " secondes.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Le joueur a été touché et reçoit " + damageAmount + " dégâts.");
            // Appeler ici une fonction pour infliger des dégâts au joueur
        }
    }
}
