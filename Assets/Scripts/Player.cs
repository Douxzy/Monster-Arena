using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f; // Vitesse de déplacement

    private Rigidbody2D m_Rigidbody;

    [SerializeField]
    private GameObject Sword;

    private bool canUseSword = true;

    [SerializeField]
    private Transform LifeBar;

    [SerializeField]
    private int HP = 100;

    void Start()
    {

        Sword.SetActive(false);
        // Récupérer le composant Rigidbody2D une seule fois au démarrage
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    IEnumerator AttackDelay()
    {
        Sword.SetActive(true);

        yield return new WaitForSeconds(1);
        
        Sword.SetActive(false);

        yield return new WaitForSeconds(1);


        canUseSword = true;
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space)) {
            if(canUseSword) {
                canUseSword = false;
                StartCoroutine(AttackDelay());
            }
        }

        // Récupérer l'entrée utilisateur
        Vector2 m_Input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Déplacer le Rigidbody2D à la nouvelle position
        m_Rigidbody.MovePosition((Vector2)m_Rigidbody.position + m_Input * moveSpeed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.O)) {
            HP -= 25;
            Vector3 scale = LifeBar.localScale;
            scale.x = LifeBar.localScale.x - LifeBar.localScale.x / HP;
            LifeBar.localScale = scale;
        }

        

    }

    

    
}