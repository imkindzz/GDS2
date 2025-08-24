using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLookAtEnemy : MonoBehaviour
{
    public GameObject enemyTarget; //the enemy that the attack looks at

    #region Unity methods
    void Update()
    {
        Vector3 look = transform.InverseTransformPoint(enemyTarget.transform.position);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg - 90;

        transform.Rotate(0, 0, angle);
    }
    #endregion
}
