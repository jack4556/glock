using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Variables

    [SerializeField] private float velocity;
    

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.AddForce(0, 0,  velocity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
