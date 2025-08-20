using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float damage; //the amount of damage that the player makes
    [SerializeField] private float damageRadius; //the radius distance that the player can attack to

    private CircleCollider2D col;

    private List<StatusBase> reachableStatus; //the enemy or boss statuses that are within the damageRadius

    #region Unity methods
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        col.radius = damageRadius;

        reachableStatus = new List<StatusBase>();
    }

    private void OnDrawGizmosSelected()
    {
        //shows the damage radius range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;

        StatusBase status = collision.GetComponent<StatusBase>();
        if (status) reachableStatus.Add(status);

        Debug.LogWarning("reachableStatus");
        Debug.Log(reachableStatus.Count);
        foreach (StatusBase statusBase in reachableStatus) Debug.Log(statusBase.gameObject.name);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;

        StatusBase status = collision.GetComponent<StatusBase>();
        if (status) reachableStatus.Remove(status);

        Debug.LogWarning("reachableStatus");
        Debug.Log(reachableStatus.Count);
        foreach (StatusBase statusBase in reachableStatus) Debug.Log(statusBase.gameObject.name);
    }
    #endregion
}
