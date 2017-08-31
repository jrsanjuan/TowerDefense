using UnityEngine;

/*
** Class which notifies to turrets when an enemy leaves his collider.
*/
public class EnemyDestructionDelegate : MonoBehaviour {
    #region FIELDS
    public delegate void EnemyDelegate (GameObject enemy);  // Delegate container for a function that can be passed like a variable.
    public EnemyDelegate enemyDelegate;                     // Instance of enemyDelegate.
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy () 
    {
        if (enemyDelegate != null) { enemyDelegate (gameObject); }
    }
    #endregion
}
