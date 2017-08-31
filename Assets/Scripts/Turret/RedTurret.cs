using UnityEngine;

/*
** Inherit from turret class.
** Controls the blue turret behaviour.
*/
public class RedTurret : Turret 
{
    #region FIELDS
    private GameObject newBullet;           // Bullet.
    private LaserBullet _bulletComp;        // Bullet the turret is going to shoot.
    #endregion

    #region UNITY_METHODS
    /// <summary>
    /// Used for initialization.
    /// </summary>
    protected override void Start()
    {
        // Store the start position of the bullet.
        Vector3 startPosition = gameObject.transform.position;
        // Create a new GameObject from the bulletPrefab.
        newBullet = (GameObject)Instantiate (bulletPrefab);
        // Update its startPosition.
        newBullet.transform.position = startPosition;
        // Get an instance of the bullet this turret is going to shoot.
        _bulletComp = newBullet.GetComponent<LaserBullet>();
        // Set the bullet component fields.
        _bulletComp.startPosition = startPosition;
        _bulletComp.isEnabled = false;
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
                // Shoot a bullet to the target
                Shoot(target.GetComponent<Collider>());
            }
            else {
                // Disabled the bullet.
                _bulletComp.isEnabled = false;
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
        // Update the bullet component fields.
        _bulletComp.target = target.gameObject;
        _bulletComp.targetPosition = target.transform.position;
        _bulletComp.isEnabled = true;
    }
    #endregion
}
