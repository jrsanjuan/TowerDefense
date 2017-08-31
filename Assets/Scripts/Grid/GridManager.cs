using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/*
** Class which manages the grid behaviour.
** Responsible for the creation and reseting of the grid.
*/
public class GridManager : MonoBehaviour {

    #region FIELDS
    public int columns = 6;                                                 // Grid number of columns.
	public int rows = 6;                                                    // Grid number of rows.
	public GameObject tilePrefab;                                           // Prefab of the tiles on the grid.
	public GameObject crystalPrefab;                                        // Prefab of the crystals.
	private Transform _gridHolder;                                          // GameObject containing the grid tiles.
	private List<Vector3> _crystalsPositions = new List <Vector3>();        // List with all the posible positions of the crystals.
	private List<Vector3> _actualCrystalPositions = new List <Vector3>();   // List with the positions where the crystals are. 
	private List<Node> _path = new List<Node>();                            // The path the enemies have to follow.
	private GameObject[,] _tiles;                                           // Array with all the positions of the tiles (x,z)
	private BFS _breadth = new BFS();                                       // And instance of the BFS class which stores the BFS algorithm.                             
	private Node _start;                                                    // The start position of the enemy.
	private Node _end;                                                      // The end position of the enemy.
    public Wave[] waves;                                                    // The waves our game will have.
    public int timeBetweenWaves = 5;                                        // Time passed between waves.
    private float lastSpawnTime;                                            // Last time an enemy was spawned.
    private int enemiesSpawned = 0;                                         // Enemies spawned at the moment.
    /*
    ** Serializable class with the parameters of the wave.
    */
    [System.Serializable]
    public class Wave {
        public GameObject enemyPrefab;                                      // The enemy prefab of the wave.                                
        public float spawnInterval = 2;                                     // Spawn between enemies of the wave.
        public int maxEnemies = 10;                                         // Max of enemies in the wave.
    }
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Used for initialization.
    /// </summary>
	private void Start () 
    {
        // Set the lastSpawnTime to the time since the beginning of the frame.
        lastSpawnTime = Time.time; 
    }
	
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
	private void Update () 
    {
        // If the player starts the game and is not gameOver
		if (Input.GetKey(KeyCode.P) && !GameManager.SINGLETON.gameOver) {
            // The state of the game changes to playing.
            GameManager.SINGLETON.isPlaying = true;   
        }
        // If the state is playing
        if (GameManager.SINGLETON.isPlaying) {
            // Get the current wave of the game.
            int currentWave = GameManager.SINGLETON.Wave;
            // If that is not the last wave.
            if (currentWave < waves.Length) {
                // Set the tieme between the last spawn
                float timeInterval = Time.time - lastSpawnTime;
                float spawnInterval = waves[currentWave].spawnInterval;
                // If there are enemies to spawn and the time to spawn them its correct
                if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) ||
                    timeInterval > spawnInterval) && 
                    enemiesSpawned < waves[currentWave].maxEnemies) {
                        // Update the time of lastSpawn
                        lastSpawnTime = Time.time;
                        // Spawn a new enemy
                        SpawnEnemy(currentWave);
                        // Update the number of enemies spawned.
                        enemiesSpawned++;
                }
                // If there are not enmies left to spawn and the wave is over    
                if (enemiesSpawned == waves[currentWave].maxEnemies &&
                    GameObject.FindGameObjectWithTag("Enemy") == null) {
                    // Next wave incoming.
                    GameManager.SINGLETON.Wave++;
                    // Update the variables for next waves.
                    enemiesSpawned = 0;
                    lastSpawnTime = Time.time;
                }
            }
            // No more waves to spawn, the game is over, you win! 
            else {
                GameManager.SINGLETON.gameOver = true;
                Debug.Log("GAMEOVER. YOU WIN!");
            }   
        }
    }
    #endregion
    
    #region CUSTOM_METHODS
    /// <summary>
    /// Initializes the grid based on the columns and rows.
    /// Gives all the nodes who form the grid a status and calculates its neighbours.
    /// </summary>
    public void InitGrid()
	{
        // Encapsulate the grid in this GameObject.
		_gridHolder = new GameObject("Grid").transform;
        // Create an array of columns and rows
		_tiles = new GameObject[columns,rows];
		// Loop through x axis (columns).
	    for(int x = 0; x < columns; x++)
	    {
	        // Within each column, loop through y axis (rows).
	        for(int z = 0; z < rows; z++)
	        {
                // Create the position this tile is going to have.
	            Vector3 tilePosition = new Vector3(x, 0f, z);
	            // Instantiate a tile in the position previously calculated.
	            GameObject tile = (GameObject) Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                // Set the tile parent to be stored.                
	            tile.transform.SetParent(_gridHolder);
                // Give it a name for better recognition.
	            tile.name = "Tile" + x + z;
                // Assign the tile position to the previously calculated.
        		tile.transform.position = tilePosition;
                // Add this tile the array.
    			_tiles[x,z] = tile;
                // Get the node at this position and initialize its params.
	            Node node = tile.GetComponent<Node>();
        		node.visited = false;
        		node.path = false;
        		node.Status = Node.CLEAR;
                // If its the last columns, reserve that position for crystals.
        		if (x == columns-1)
        			_crystalsPositions.Add(tilePosition);
	        }
	    }
        // Caculate the neighbours.
        AssociateGridNodes( );
	}

    /// <summary>
    /// Loops through the grid and calculate the neighbours of each node.
    /// </summary>
	private void AssociateGridNodes()
	{
        for(int x = 0; x < columns; x++) {
			for(int z = 0; z < rows; z++) {	
				Node node = _tiles[x, z].GetComponent<Node>();
				// Up neighbour
				if( (z + 1) < rows )  {
					node.adjacent.Add(GetNodeAt(x, z+1));
				}
				// Right neighbour
				if( (x + 1) < columns) {
					node.adjacent.Add( GetNodeAt(x+1, z) );
				}
				//  Down neighbour
				if( (z - 1) >= 0 ) {
					node.adjacent.Add( GetNodeAt(x ,z-1) );
				}
				// Left neighbour
				if( (x - 1) >= 0 ) {
					node.adjacent.Add( GetNodeAt(x-1, z) );
				}
				// Diagonal UpLeft neighbour
		        if( (x - 1) >= 0 && (z + 1) < rows ) {
		        	node.adjacent.Add( GetNodeAt(x-1, z+1));
		        }
		        // Diagonal UpRight neighbour
				if( (x + 1) < columns && (z + 1) < rows ) {
					node.adjacent.Add( GetNodeAt(x+1, z+1) );
				}
				// Diagonal DownLeft neighbour
				if( (x - 1) >= 0 && (z - 1) >= 0 ) {
					node.adjacent.Add( GetNodeAt(x-1, z-1) );
				}
				// Diagonal DownRight neighbour
				if( (x + 1) < columns && (z - 1) >= 0 ) {
					node.adjacent.Add( GetNodeAt(x+1, z-1) );
				}
			}
		}
	}
    
    /// <summary>
    /// Returns the node in the position given.
    /// <param name="x">The x position to search the node.</param>
    /// <param name="z">The z position to search the node.</param>    
    /// </summary>
    public Node GetNodeAt(int x, int z)
	{
		return _tiles[x, z].GetComponent<Node>();
	}
    
    /// <summary>
    /// Creates the crystals on the right side of the grid.
    /// </summary>
    public void CreateCrystals()
    {   
        // Get a random number between 1 and rows.
		int numberOfCrystals = Random.Range (1, rows+1);
        // Instantiate the random number of crystals.
		for (int i = 0; i < numberOfCrystals; i++) {
            // Instantiate the crystal in a valid random position.
			Instantiate(crystalPrefab, GetRandomCrystal(), Quaternion.identity);
		}
	}

    /// <summary>
    /// Returns a random position for the crystal to be instantiate.
    /// </summary>
	private Vector3 GetRandomCrystal() 
	{
        // Random index between 0 and the posibles positions of the crystals.
		int randomIndex = Random.Range(0, _crystalsPositions.Count);
        // Get the randomPosition.
		Vector3 randomPosition = _crystalsPositions[randomIndex] + new Vector3(0.0f, 1.0f, 0.0f);
        // Add it to the actual crystal positions.
		_actualCrystalPositions.Add(randomPosition);
        // Remove this possible position from the list.
        _crystalsPositions.RemoveAt(randomIndex);
    	return randomPosition;
	}
    
    /// <summary>
    /// Spawns an enemy based on the current wave.
    /// <param name="currentWave">Current wave of the game.</param>
    /// </summary>
    private void SpawnEnemy(int currentWave)
    {
        // Get a random row for the enemy to spawn.
        int randomRow = Random.Range(0, rows);
        // Set the start position of the enemy to that random row.
        _start = GetNodeAt(0, randomRow);
        _start.Status = Node.START;
        // Set the end position of the enemy to a crystal.
        int randomCrystal = Random.Range(0, _actualCrystalPositions.Count);
        // Get a random crystal from the actual crystals list.
        _end = GetNodeAt((int)_actualCrystalPositions[randomCrystal].x, (int)_actualCrystalPositions[randomCrystal].z);
        _end.Status = Node.END;
        bool found = false;
        // Call the BFS algorithm.
        found = _breadth.Find(_start);
        // Get the path that the enemy will follow.
        _path = _breadth.GetPath();
        // If not path cant be found, restart the game.
        if (!found) {
            GameManager.SINGLETON.gameOver = true;
            Debug.Log("GAMEOVER. No valid path was found for the enemy, try to not block all possible paths ;).");
        }
        else  {
            // Path was found, instantiate the enemy and reset the grid for the next enemy.
            Instantiate(waves[currentWave].enemyPrefab).GetComponent<Enemy>().waypoints = _path;
            _start.Status = Node.CLEAR;
            _end.Status = Node.CLEAR;
            ResetGrid();
        }
    }
    
    /// <summary>
    /// Resets the grid.
    /// </summary>
    private void ResetGrid()
    {
        // Create a new path and BFS algorithm.
        _path = new List<Node>();	
		_breadth = new BFS();
        // Loop through the grid and restart the node values for the next enemy.
		for(int x = 0; x < columns; x++) {
			for(int z = 0; z < rows; z++) {
				Node n = GetNodeAt(x, z);
				n.visited = false;
				n.parent = null;
                n.path = false;
			}
		}
    }
    #endregion
}