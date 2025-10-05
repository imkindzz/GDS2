using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //to change the enemy detection range for the damage, change the size of the trigger collider

    [SerializeField] private float damage = 5f; //the amount of damage that the player makes
    [SerializeField] private float damageDelay = 0.25f; //the time taken for the damage to be in effect

    [Header("Attack Sprites")]
    [SerializeField] private GameObject damageDirectionGO; //the gameObject that shows the attack being made
    [SerializeField] private GameObject damageMadeGO; //the gameObject that shows the attack being made
    [SerializeField] private float distanceOfDamageMadeGO; //the distance from and at an enemy's position for the damageMade gameobject

    private List<StatusBase> reachableStatus; //the enemy or boss statuses that are within the damageRadius
    private List<GameObject> damageDirections; //the gameobjects that show the direction of the damage being made
    private List<GameObject> damageCreated; //the gameobjects that shows the damage made

    private float damageTimer = 0f;

    private AudioSource attackLoopSource;

    #region Unity methods
    void Start()
    {
        reachableStatus = new List<StatusBase>();
        damageDirections = new List<GameObject>();
        damageCreated = new List<GameObject>();
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
            for (int i = unreachableStatus.Count - 1; i >= 0; i--)
            {
                int index = unreachableStatus[i];

                reachableStatus.RemoveAt(index);
                
                Destroy(damageDirections[index]);
                damageDirections.RemoveAt(index);
                
                Destroy(damageCreated[index]);
                damageCreated.RemoveAt(index);

            }

            damageTimer = 0f;

            //when there are no reachable statuses to play the audio
            if (reachableStatus.Count == 0 && attackLoopSource != null)
            {
                SoundManager.instance.StopSoundLoop(attackLoopSource);
                attackLoopSource = null;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Soul")) return;

        StatusBase status = collision.GetComponent<StatusBase>();
        if (status && !reachableStatus.Contains(status))
        {
            reachableStatus.Add(status);

            // --- Spawn the beam (damageDirectionGO) ---
            GameObject beamGO = Instantiate(damageDirectionGO, Vector3.zero, Quaternion.identity); // world-space
            AttackLookAtEnemy beamScript = beamGO.GetComponent<AttackLookAtEnemy>();
            beamScript.soul = transform;               // assign the player
            beamScript.enemy = collision.transform;   // assign the enemy
            damageDirections.Add(beamGO);

            // --- Spawn the impact/damage indicator (damageMadeGO) ---
            GameObject dm = Instantiate(damageMadeGO, collision.transform.position, Quaternion.identity, collision.transform);
            damageCreated.Add(dm);

            //makes sound
            if (attackLoopSource == null) attackLoopSource = SoundManager.instance.PlaySound(SfxSoundName.GhostAttack, transform, true);
        }
    }



    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Soul")) return;

        StatusBase status = collision.GetComponent<StatusBase>();
        if (status)
        {
            int statusIndex = reachableStatus.IndexOf(status);
            if (statusIndex != -1)
            {
                //destroys gameObjects on scene
                Destroy(damageDirections[statusIndex]);
                Destroy(damageCreated[statusIndex]);
                
                //removes the non-existing gameobjects
                damageDirections.RemoveAt(statusIndex);
                damageCreated.RemoveAt(statusIndex);
                reachableStatus.RemoveAt(statusIndex);
            }
        }
    }
    #endregion
}
