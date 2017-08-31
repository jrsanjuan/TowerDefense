using UnityEngine;

/*
** Class to add a turret to the grid.
*/
public class AddTurret : MonoBehaviour {

    #region FIELDS
    private GameObject _turret;             // GameObject turret.
    private GameObject _turretPrefab;       // Prefab of the turret.
	private GridManager _gridScript;        // Instance of the GridManager script.
	private Color _startcolor;              // Color of the grid.
    private float _positionX;               // The column in which we add a turret.        
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Used for initialization.
    /// </summary>
    private void Start()
	{
        // Get a instance of the GridManager script.
		_gridScript = GameManager.SINGLETON.GetGrid();
        // Set the column on which the turret is going to be instantiated.
        _positionX = transform.position.x;
	}
    /// <summary>
    /// Called when the mouse enters the GUIElement or Collider.
    /// </summary>
    void OnMouseEnter()
 	{
         // Set the initial color of the grid.
         _startcolor = GetComponent<Renderer>().material.color;
         // If the player can add a turret in this position, change the color of the grid.
         if (CanAddTurret())
            GetComponent<Renderer>().material.color = Color.cyan;
    }
    
    /// <summary>
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    void OnMouseOver()
	{
        // Depending on which mouse button the player press, spawn a blue or red turret.
		if (Input.GetMouseButton(0)) 
			Add((GameObject) GameManager.SINGLETON.turrets[0].turretPrefab);
		else if (Input.GetMouseButton(1)) {
			Add ((GameObject) GameManager.SINGLETON.turrets[1].turretPrefab);
		}
	}
    
    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit()
 	{
         // Return the initial color to the grid.
         GetComponent<Renderer>().material.color = _startcolor;
 	}
    #endregion
    
    #region CUSTOM_METHODS
    /// <summary>
    /// Returns if the player can add a turret to the grid.
    /// </summary>
    private bool CanAddTurret() 
	{
        // If a turret already is in that position, or the columns are for spawn crystals/enemies
        // or the player is playing or the game is over, return false.
		return (_turret == null && _positionX != 0 && 
                _positionX != _gridScript.columns-1 && !GameManager.SINGLETON.isPlaying
                && !GameManager.SINGLETON.gameOver);
	}
    
    /// <summary>
    /// Add a turret to the grid in this position.
    /// <param name="turret">The turret to be added.</param>
    /// </summary>
	void Add(GameObject turret)
	{
        // If CanAddTurret to this grid
		if (CanAddTurret()) {
            // Set the position of the turret to this grid, plus 1 in the y axis. (Our floor is 0)
			Vector3 turretPosition = transform.position + new Vector3 (0.0f, 1.0f, 0.0f);
            // Get the node at this position and update its status
			Node node = _gridScript.GetNodeAt((int)turretPosition.x, (int)turretPosition.z);
			node.Status = Node.OBSTRUCTED;
            // Instantiate the turret.
			_turret = (GameObject) Instantiate(turret, turretPosition, Quaternion.identity);
		}
	}
    #endregion
}