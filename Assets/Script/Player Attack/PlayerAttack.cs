using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //to change the enemy detection range for the damage, change the size of the trigger collider

    [SerializeField] private float damage = 5f; //the amount of damage that the player makes
    [SerializeField] private float damageDelay = 0.25f; //the time taken for the damage to be in effect

    private List<StatusBase> reachableStatus; //the enemy or boss statuses that are within the damageRadius

    private float damageTimer = 0f;

    #region Unity methods
    void Start()
    {
        reachableStatus = new List<StatusBase>();
    }

    void Update()
    {
        damageTimer += Time.deltaTime;

        if (damageTimer > damageDelay)
        {
            List<int> unreachableStatus = new List<int>(); //stores the destroyed/dead enemies by the index

            //makes damage to the enemies
            for (int i = 0; i < reachableStatus.Count; i++)
            {
                StatusBase status = reachableStatus[i];

                status.TakeDamage(damage);
                if (status.noHealth) unreachableStatus.Add(i);
            }

            //removes the destroyed/dead enemies from the reachableStatus List
            for (int i = unreachableStatus.Count - 1; i >= 0; i--) reachableStatus.RemoveAt(unreachableStatus[i]);

            damageTimer = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Soul")) return;

        StatusBase status = collision.GetComponent<StatusBase>();
        if (status) reachableStatus.Add(status);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Soul")) return;

        StatusBase status = collision.GetComponent<StatusBase>();
        if (status) reachableStatus.Remove(status);
    }
    #endregion
}
