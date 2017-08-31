using UnityEngine;
using System.Collections.Generic;

/*
** Class which encapsulates
** all the enemy behaviour. 
*/
public class Enemy : MonoBehaviour {
    
    #region FIELDS
    public float speed = 1.0f;                          // Speed of the enemy.
    public int health = 100;                            // Health of the enemy.
     [HideInInspector]
    public List<Node> waypoints = new List<Node>();     // List with all the waypoints this enemy has between his target.
    private int _currentWaypoint = 0;                   // Current waypoint of the list.
    private float _lastWaypointSwitchTime;              // Stores the time when the enemy passed a waypoint.
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Used for initialization.
    /// </summary>
	void Start () 
    {
        // Set _lastWaypointSwitchTime to the time from the start of the frame.
        _lastWaypointSwitchTime = Time.time;
	}
    
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
	void Update () 
    {
        // Check if the game is over to stop this behaviour.
        if (!GameManager.SINGLETON.gameOver) {
            // Set the start and position of the enemy-
            Vector3 startPosition = waypoints [_currentWaypoint].Position;
            Vector3 endPosition = waypoints [_currentWaypoint + 1].Position;
            // Calculate the distance between this two vectors.
            float pathLength = Vector3.Distance (startPosition, endPosition);
            // Set the time top reach the endPosition.
            float totalTimeForPath = pathLength / speed;
            // Update the current time on the waypoint.
            float currentTimeOnPath = Time.time - _lastWaypointSwitchTime;
            // Interpolate the enemy to the next waypoint.
            gameObject.transform.position = Vector3.Lerp (startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
            // If the enemy has reached the endPosition
            if (gameObject.transform.position.Equals(endPosition)) {
                // And still has waypoints, continue.
                if (_currentWaypoint < waypoints.Count - 2) {
                    // Update current waypoint and time. 
                    _currentWaypoint++;
                    _lastWaypointSwitchTime = Time.time;
                // If has reached the endPosition (next waypoint) but also the last, gameover.
                } else {
                    // Enemy has reached a crystal, restart the game.
                    GameManager.SINGLETON.gameOver = true;
                    Debug.Log("GAMEOVER. Enemy has reached one crystal!");
                }
            }   
        }
	}
    #endregion
    
    #region CUSTOM_METHODS
    /// <summary>
    /// Returns the distance of the enemy to his target.
    /// </summary>
    public float DistanceToTarget()
    {
        // Store a distance.
        float distance = 0;
        // Update it with the enemy transform position and next waypoint.
        distance += Vector3.Distance(gameObject.transform.position, waypoints [_currentWaypoint + 1].Position);
        // Loop in all the waypoints this enemy has.
        for (int i = _currentWaypoint + 1; i < waypoints.Count - 1; i++) {
            Vector3 startPosition = waypoints [i].Position;
            Vector3 endPosition = waypoints [i + 1].Position;
            // Update the distance
            distance += Vector3.Distance(startPosition, endPosition);
        }
        return distance;
    }
    #endregion
}
