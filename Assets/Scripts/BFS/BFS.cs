using System.Collections.Generic;

/*
** Class with the Breadth-First Search algorithm.
*/
public class BFS {
    
    #region FIELDS
    private List<Node> _path = new List<Node>();        // The path result from this algorithm.
	private Queue<Node> _queue = new Queue<Node>();     // Stack of nodes.    
    #endregion
	
    #region CUSTOM_METHODS
    /// <summary>
    /// Finds a valid path in the grid nodes.
    /// <param name="start">The start position to begin the search.</param>
    /// </summary>
    public bool Find(Node start) 
    {
        // Set the first node to visited.
		start.visited = true;
        // Enqueue this node.
		_queue.Enqueue(start);
        // While the queue is not 0
		while(_queue.Count > 0) {
            // Get the node from the top.        
			Node n = _queue.Dequeue();
            // If is the end node
			if (n.Status == Node.END) {
                // Add it to the path.
				Node t = n;
				while(t != null) {
					_path.Add(t);
                    // Update its field.
					t.path = true;
                    // Set the parent.
					t = t.parent;
				}
                // Reverse the list and return true
				_path.Reverse();
				return true;
			}
            // If is not the end node, loop in the adjacent nodes.
			for (int i = 0; i < n.adjacent.Count; i++) {
                // If is not obstructed or visited
				if (n.adjacent [i].IsValid ()) {
                    // Set the fields of the node.
					n.adjacent[i].parent = n;
					n.adjacent[i].visited = true;
                    // Enqueue this node and continue.
					_queue.Enqueue(n.adjacent[i]);
				}
			}
		}
        // No valid path found.
		return false;
	}
	
    /// <summary>
    /// Returns the path.
    /// </summary>
	public List<Node> GetPath() { return _path; }
    #endregion
}
