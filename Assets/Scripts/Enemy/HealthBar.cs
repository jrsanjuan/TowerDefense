using UnityEngine;

/*
** Class which updates the enemy healthBar.
*/
public class HealthBar : MonoBehaviour {
    
    #region FIELDS
    public float currentHealth;             // Current health of the enemy.
    private float _maxHealth;               // Max health this enemy can have.
    private float _originalScale;           // The originalScale the gameObject has.
    #endregion

    #region UNITY_METHODS
    /// <summary>
    /// Used for initialization.
    /// </summary>
	void Start () 
    {
        // Set the maxhealth to the enemy health.
        _maxHealth = transform.GetComponentInParent<Enemy>().health;
        // Set currenthealth to maxhealth.
        currentHealth = _maxHealth;
        // Store the scale of the gameObject.
        _originalScale = gameObject.transform.localScale.x;
	}
	
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
	void Update () {
        // Store the scale of the gameObject to not break the actual.
        Vector3 tmpScale = gameObject.transform.localScale;
        // Transform the originalScale of the gameObject in the x axis.
        tmpScale.x = currentHealth / _maxHealth * _originalScale;
        // Return the scale to the original.
        gameObject.transform.localScale = tmpScale;
	}
    #endregion
}
