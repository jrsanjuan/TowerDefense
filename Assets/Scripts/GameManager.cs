using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/*
** Class which manages the game.
*/
public class GameManager : MonoBehaviour {
    
    #region FIELDS
    public List<TurretType> turrets;                // List with the turrets the game has.
    [HideInInspector]
	public bool gameOver;                           // Variable which stores the status of the game.
    [HideInInspector]
    public bool isPlaying;                          // Variable which stores the status of the game.
    private GridManager _gridScript;                // Reference to the grid manager.
    private int wave;                               // Current wave of the game.
    /*
    ** Serializable class with the turretPrefabs.
    */
    [System.Serializable]
	public class TurretType {
	    public GameObject turretPrefab;             // The turretPrefab of the game.
   	}
    #endregion
    
    #region PROPERTIES
    public int Wave {                               // Returns and sets the current game wave.
        get { return wave; }
        set { wave = value; }
    }
    #endregion
    
	#region SINGLENTON
	private static GameManager _instance = null;    // Singleton instance of the game manager.
    
	/// <summary>
	/// Gives access to the singleton.
	/// </summary>
	public static GameManager SINGLETON
	{
		get
		{
			if (_instance == null) {
				Debug.Log("Game Manager not initialized.");      
            }
			return _instance;
		}
	}
	#endregion

	#region UNITY_METHODS
	/// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake()
    {
        // Set the values of the main params.
        _instance = this;
        _gridScript = GetComponent<GridManager>();
        gameOver = false;
        isPlaying = false;
    }

    /// <summary>
    /// Used for initialization.
    /// </summary>
    private void Start()
    {
        // Start the game and set the current wave to 0.
        Wave = 0;
    	StartGame();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // Check if the game is over and raises the reset method.
        if (gameOver) { GameOver(); }	
    }
    
    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    private void OnDestroy()
    {
        _instance = null;
    }
    #endregion
    
    #region CUSTOM_METHODS
    /// <summary>
    /// Starts the game, creating the grid and the crystals.
    /// </summary>
    private void StartGame()
    {
    	_gridScript.InitGrid();
    	_gridScript.CreateCrystals();
    }
    
    /// <summary>
    /// Returns a instance to the grid manager.
    /// </summary>
    public GridManager GetGrid()
    {
    	return _gridScript;
    }
    
    /// <summary>
    /// Raises the gameOver and resets the scene.
    /// </summary>
    private void GameOver()
    {
        // Change the state of the game.
        isPlaying = false;
        // Start the coroutine
        StartCoroutine(ReloadScene());
    }
    
    /// <summary>
    /// Coroutine to make the fade to black effect and reset the scene.
    /// </summary>
    IEnumerator ReloadScene() {
		float fadeTime = this.GetComponent<FadeToBlack>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
        // Reset the scene.
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
    #endregion
}