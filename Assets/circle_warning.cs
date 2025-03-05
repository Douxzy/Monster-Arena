using System.Collections;
using UnityEngine;

public class circle_warning : MonoBehaviour
{
    [SerializeField] private GameObject attack_circle_generator; // Préfab de l'attaque
    [SerializeField] private float lifetime = 1f; // Durée du cercle d'avertissement

    private Vector2 spawnPosition; // Stocker la position correcte

    void Start()
    {
        spawnPosition = transform.position;
        StartCoroutine(SpawnAttackCircle());
    }
    void SetLifetime(float lifetime){
        this.lifetime = lifetime;
    }
    IEnumerator SpawnAttackCircle()
    {
        yield return new WaitForSeconds(lifetime);

        if (attack_circle_generator != null)
        {
            
            Instantiate(attack_circle_generator, spawnPosition, Quaternion.identity);
        }
        
        Destroy(gameObject); // Détruire le cercle de warning
    }
}
