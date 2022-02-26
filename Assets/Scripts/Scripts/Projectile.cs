using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Variables

    [SerializeField] private float velocityFront;
    [SerializeField] private float velocityUp;
    [SerializeField] public int dmg;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.AddForce(velocityFront, velocityUp,  0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
