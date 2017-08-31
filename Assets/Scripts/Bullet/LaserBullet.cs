using UnityEngine;

/*
** Inherit from bullet class.
** Controls the laser bullet behaviour.
*/
public class LaserBullet : Bullet {
    
    #region FIELDS
    [HideInInspector]
    public bool isEnabled = false;          // Checks is the bullet laser is enabled.
    private LineRenderer line;              // The line renderer we need to throw.
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Used for initialization.
    /// </summary>
	protected override void Start ()
    {
        // Get the line renderer component.
        line = GetComponent<LineRenderer>();
        // Enable/disable the line based on the public variable.
        line.enabled = isEnabled;
	}
	
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
	protected override void Update () 
    {
        // Check if the game is over to stop the bullet behaviour.
        if (!GameManager.SINGLETON.gameOver) {
            // Enable/disable the line based on the public variable.
            line.enabled = isEnabled;
            // If is enabled
            if (isEnabled) {
                // Create a ray with the startPosition and direction of targetPosition.
                Ray ray = new Ray(startPosition, targetPosition);
                // Set the line positions.
                line.SetPosition(0, ray.origin);
                line.SetPosition(1, targetPosition);
                // Update the health of the target.
                Transform healthBarTransform = target.transform.FindChild("HealthBar");
                HealthBar healthBar = healthBarTransform.gameObject.GetComponent<HealthBar>();
                healthBar.currentHealth -= Mathf.Max(damage/50, 0);
                // If the target health is less than 0
                if (healthBar.currentHealth <= 0) {
                    // Destroy target.
                    Destroy(target);
                }
            }
        }
	}
    #endregion
}
