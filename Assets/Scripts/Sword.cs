using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    [SerializeField]
    private Transform Player;

    [SerializeField]
    private Vector3 m_Offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

        transform.position = Player.position + m_Offset;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Hit");
    }
}
