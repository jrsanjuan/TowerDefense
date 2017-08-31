using UnityEngine;

/*
** Abstract bullet class which encapsulates
** all the bullet behaviour. 
*/
public abstract class Bullet : MonoBehaviour {
    
    #region FIELDS
    public float damage;                                        // Bullet damage to target.
    [HideInInspector]
    public GameObject target;                                   // Bullet target to collisionate with.
    [HideInInspector]
    public Vector3 startPosition;                               // Bullet start position. 
    [HideInInspector]
    public Vector3 targetPosition;                              // Bullet end position.
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Used for initialization.
    /// </summary>
    protected abstract void Start ();
	
	/// <summary>
    /// Update is called once per frame.
    /// </summary>
	protected abstract void Update ();
    #endregion
}
