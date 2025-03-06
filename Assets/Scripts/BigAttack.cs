using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAttack : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 50;  // Dégâts infligés par le cercle
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
}
