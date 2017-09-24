using UnityEngine;

/*
** Inherit from bullet class.
** Controls the normal bullet behaviour.
*/
public class NormalBullet : Bullet {
    
    #region FIELDS
    public float speed = 5.0f;              // Bullet speed.
    private float distance;                 // Distance between the bullet and the target.
    private float startTime;                // Time transcurred from the beginning of the call.
    
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Used for initialization.
    /// </summary>
	protected override void Start ()
    {
        // Save the time passed from the beginning of the frame.
        startTime = Time.time;
        // Calculate distance between the startPosition of the bullet and the targetPosition.
        distance = Vector3.Distance (startPosition, targetPosition);
	}
	
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
	protected override void Update ()
    {
        // Check if the game is over to stop the bullet behaviour.
        if (!GameManager.SINGLETON.gameOver) {
            // Calculate time passed in relation to speed.
            float timeInterval = (Time.time - startTime) * speed;
            // Update the bullet position, translating his transform to the targetPosition.
            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, timeInterval / distance);
            // If the bullet reaches the targetPosition
            if (gameObject.transform.position.Equals(targetPosition)) {
                // And the target is not null
                if (target != null) {
                    // Update the health of the target.
                    Transform healthBarTransform = target.transform.FindChild("HealthBar");
                    HealthBar healthBar = healthBarTransform.gameObject.GetComponent<HealthBar>();
                    healthBar.currentHealth -= Mathf.Max(damage, 0);
                    // If the target health is less than 0
                    if (healthBar.currentHealth <= 0) {
                        // Destroy target.
                        Destroy(target);
                    }
		    Destroy(gameObject, 1);
                }
                // Destroy the bullet reaches his target.
                Destroy(gameObject);
            }   
        }
	}
    #endregion
}
