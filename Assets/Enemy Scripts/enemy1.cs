using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy1 : MonoBehaviour
{
    public GameObject fireBall;
    public Transform fireExit;
    public float startTimeBTWShots;
    private Animator animator;
    public int health;

   // private Animator animator;
    private float timeBTWShots;    // Start is called before the first frame update
    void Start()
    {
        timeBTWShots = 2 + Random.Range(-0.5f, 0.5f);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBTWShots <= 0)
        {
           // Vector2 pos = transform.position;
           // pos.x -= 0.5f;
            animator.SetTrigger("Attack");
            Instantiate(fireBall, fireExit);
            timeBTWShots = startTimeBTWShots + Random.Range(-0.5f, 0.5f);

        }
        else
        {
            timeBTWShots -= Time.deltaTime;
        }
    }
}
