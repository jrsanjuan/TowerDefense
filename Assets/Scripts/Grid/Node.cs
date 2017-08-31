using UnityEngine;
using System.Collections.Generic;

/*
** Class which controls the nodes of the grid.
*/
public class Node : MonoBehaviour {

    #region FIELDS
    public const int CLEAR = 0;             // Constant variable to store the status of this node.
	public const int OBSTRUCTED = 1;        // Constant variable to store the status of this node.
	public const int START = 2;             // Constant variable to store the status of this node.
	public const int END = 3;               // Constant variable to store the status of this node.
	public bool visited = false;            // Check if the node has been visited.
	public bool path = false;               // Check if the node is in a valid path.
	public List<Node> adjacent = null;      // List with all adjacent nodes of this node.
	public Node parent = null;              // Parent of the node.
    private int status = 0;                 // Status of this node.

    #endregion
    
    #region PROPERTIES
    /// <summary>
    /// Set and get the status of the node.
    /// </summary>
    public int Status  {
		get { return status; }
		set { if(value >= 0 && value < 5) status = value; }
	} 
    /// <summary>
    /// Get the x position of the node.
    /// </summary>
	public int X {
		get { return (int) gameObject.transform.position.x; }
	}
    /// <summary>
    /// Get the z position of the node.
    /// </summary>
	public int Z  {
		get { return (int) gameObject.transform.position.z; }
	}
    /// <summary>
    /// Get the position of the node.
    /// </summary>
    public Vector3 Position {
        get { return new Vector3 (X, 1f, Z); }
    }
    #endregion
    
    #region CUSTOM_METHODS
    /// <summary>
    /// Returns if the node is valid to form a path.
    /// </summary>
	public bool IsValid() { return !visited && (status != Node.OBSTRUCTED); }    
    #endregion
}
