using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy1 : MonoBehaviour
{
    public GameObject fireBall;
    public float startTimeBTWShots;
    private Animator animator;
    public int health;

   // private Animator animator;
    private float timeBTWShots;    // Start is called before the first frame update
    void Start()
    {
        timeBTWShots = 2 + Random.Range(-1f, 1f);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBTWShots <= 0)
        {
            Vector2 pos = transform.position;
            pos.x -= 0.5f;
            animator.SetTrigger("Attack");
            Instantiate(fireBall, pos, Quaternion.identity);
            timeBTWShots = startTimeBTWShots + Random.Range(-1f, 1f);

        }
        else
        {
            timeBTWShots -= Time.deltaTime;
        }
    }
}
