using UnityEngine;
using System.Collections.Generic;

/*
** Abstract turret class which encapsulates
** all the turret behaviour. 
*/
public abstract class Turret : MonoBehaviour {
    
    #region FIELDS
    public GameObject bulletPrefab;                                     // Prefab of the bullet the turret shots.
    public float fireRate;                                              // Firerate of the turret.
    public List<GameObject> enemiesInRange = new List<GameObject>();    // List containing all enemies the turret has in range.
    protected GameObject target;                                        // The target of the turret.
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Used for initialization.
    /// </summary>
    protected abstract void Start();
    
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected virtual void Update() 
    {
        // Set the target to null every frame.
        target = null;
        // Create a variable which stores the mininum distance form an enemy to be detected.
        float minimalEnemyDistance = float.MaxValue;
        // For each enemiesInRange
        foreach (GameObject enemy in enemiesInRange) {
            // Store the distance between enemy and target.
            float distanceToTarget = enemy.GetComponent<Enemy>().DistanceToTarget();
            // Check if the distance is near to the target.
            if (distanceToTarget < minimalEnemyDistance) {
                // Make the target the enemy which is closest to the target.
                target = enemy;
                // Update the new mininum distance.
                minimalEnemyDistance = distanceToTarget;
            }
        }
    }
    
    /// <summary>
    /// Called when the Collider other enters the trigger.
    /// <param name="other">The collider which enters the trigger.</param>
    /// </summary>
    private void OnTriggerEnter (Collider other) 
    {
        // Check if the gameObject which enters the trigger is an enemy.
        if (other.gameObject.tag.Equals("Enemy")) {
            // If so, add the enemy to the enemiesInRange
            enemiesInRange.Add(other.gameObject);
            // Make sure to inform all listeners that the enemy is being destroyed and to not fire it.
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate += OnEnemyDestroy;
        }
    }

    /// <summary>
    /// Called when the Collider other leaves the trigger.
    /// <param name="other">The collider which leaves the trigger.</param>
    /// </summary>
    private void OnTriggerExit (Collider other)
    {
        // Check if the gameObject which enters the trigger is an enemy.
        if (other.gameObject.tag.Equals("Enemy")) {
             // If so, remove the enemy to the enemiesInRange
            enemiesInRange.Remove(other.gameObject);
            // Make sure to inform all listeners that the enemy is being destroyed and to not fire it.
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate -= OnEnemyDestroy;
        }
    }
    #endregion
    
    #region CUSTOM_METHODS
    /// <summary>
    /// Removes the enemy from enemiesInRange.
    /// <param name="enemy">The enemy to remove from enemiesInRange.</param>
    /// </summary>
    private void OnEnemyDestroy (GameObject enemy) 
    {
        enemiesInRange.Remove (enemy); 
    }
    
    /// <summary>
    /// Shoots a bullet to the target.
    /// <param name="target">The target where the bullet is being shooted.</param>
    /// </summary>
    protected abstract void Shoot(Collider target);
    #endregion
    
}