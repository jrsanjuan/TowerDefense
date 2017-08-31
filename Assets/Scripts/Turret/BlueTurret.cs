using UnityEngine;

/*
** Inherit from turret class.
** Controls the blue turret behaviour.
*/
public class BlueTurret : Turret {

    #region FIELDS
    private float _lastShotTime;            // The last time the turret shooted a bullet.
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Used for initialization.
    /// </summary>
    protected override void Start()
    {
        // Make _lastShotTime the time from the complete of the last frame.
    	_lastShotTime = Time.deltaTime;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected override void Update()
    {
        // Check if the game is over to stop the turret behaviour.
        if (!GameManager.SINGLETON.gameOver) {
            // Call the parent Update function.
            base.Update();
            // If target is not null
            if (target != null) {
                // And the time passed is less than the fireRate
                if (Time.time - _lastShotTime > fireRate) {
                    // Shoot a bullet to the target
                    Shoot(target.GetComponent<Collider>());
                    // Update _lastShotTime
                    _lastShotTime = Time.time;
                }
            }
        }
    }
    #endregion
    
    #region CUSTOM_METHODS
    /// <summary>
    /// Shoots a bullet to the target.
    /// <param name="target">The target where the bullet is being shooted.</param>
    /// </summary>
    protected override void Shoot(Collider target)
	{
        // Store the start and end position of the bullet.
        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        // Create a new GameObject from the bulletPrefab.
        GameObject bullet = (GameObject)Instantiate (bulletPrefab);
        // Update its startPosition.
        bullet.transform.position = startPosition;
        // Get an instance of the bullet this turret is going to shoot.
        NormalBullet bulletComp = bullet.GetComponent<NormalBullet>();
        // Set the bullet component fields.
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;
    }
    #endregion
}