using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HobGoblinPhase2 : MonoBehaviour
{
    public Vector2 club1pos;
    public Vector2 club2pos;

    private BossState currentState;

    public Transform clubStartpos;

    private float attackTimer = 0f;

    public enum BossState
    {
        Idle,
        Club1,
        Return,
        Club2,
        Attack
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = BossState.Club1;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) 
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.Club1:
                Club1();
                break;
            case BossState.Return:
                Return();
                break;
            case BossState.Club2:
                Club2();
                break;
            case BossState.Attack:
                Attack();
                break;
        }
    }

    

    private void Attack()
    {
        attackTimer += Time.deltaTime;
        this.GetComponent<BulletEmitter>().enabled = true;

        if (attackTimer > 2f) 
        { 
            currentState = BossState.Return;
        }
        

    }

    private void Club1()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, club1pos, 2f * Time.deltaTime);

        if (Vector3.Distance(transform.position, club1pos) < 0.01f)
        {

            currentState = BossState.Attack;
        }


    }

    private void Return()
    {
        this.GetComponent<BulletEmitter>().enabled = false;
        attackTimer = 0f;
        this.transform.position = Vector2.MoveTowards(this.transform.position, clubStartpos.position, 2f * Time.deltaTime);

        if (Vector3.Distance(transform.position, clubStartpos.position) < 0.01f)
        {
            switch (UnityEngine.Random.Range(0, 2)) 
            {
                case 0:
                currentState = BossState.Club1;
                break;
                case 1:
                currentState = BossState.Club2; 
                break;
            }
            
        }
    }

    private void Club2()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, club2pos, 2f * Time.deltaTime);

        if (Vector3.Distance(transform.position, club2pos) < 0.01f)
        {

            currentState = BossState.Attack;
        }
    }

    private void Idle()
    {
        throw new NotImplementedException();
    }
}
