using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warnBigAttack : MonoBehaviour
{
    [SerializeField] private GameObject BigAttack; // Préfab de l'attaque
    [SerializeField] private float lifetime = 3f; // Durée du cercle d'avertissement

    private Vector2 spawnPosition; // Stocker la position correcte

    void Start()
    {
        spawnPosition = transform.position;
        StartCoroutine(SpawnAttackCircle());
    }
    IEnumerator SpawnAttackCircle()
    {
        yield return new WaitForSeconds(lifetime);

        if (BigAttack != null)
        {
            
            Instantiate(BigAttack, spawnPosition, Quaternion.identity);
        }
        
        Destroy(gameObject); // Détruire le cercle de warning
    }
}
