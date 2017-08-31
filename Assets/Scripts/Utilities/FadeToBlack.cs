using UnityEngine;

/*
** Class which fades to black the game when its restarted.
*/
public class FadeToBlack : MonoBehaviour {
    #region FIEDLS
    public Texture2D fadeOutTexture;        // The texture used to fade the scene.            
	public float fadeSpeed;                 // Speed of the fade.
	private int _drawDepth = -1000;         // Depth of the draw.
	private float _alpha = 1.0f;            // Alpha color of the texture.
	private int _fadeDir = -1;              // Direction of the fade.
    #endregion
    
    #region UNITY_METHODS
    /// <summary>
    /// Called after a new level was loaded.
    /// </summary>
	void OnLevelWasLoaded() { BeginFade(-1); }
    
    /// <summary>
    ///  Called for rendering and handling GUI events.
    /// </summary>
    private void OnGUI() 
    {
        // Change the alpha.
		_alpha += _fadeDir * fadeSpeed * Time.deltaTime;
		_alpha = Mathf.Clamp01(_alpha);
        // Draw the texture and set the color.
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
		GUI.depth = _drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
	}   
    #endregion
    
    #region CUSTOM_METHODS
    /// <summary>
    ///  Begins the fade of the scene.
    /// </summary>
    public float BeginFade(int direction)
    {
		_fadeDir = direction;
		return (fadeSpeed);
	}
    #endregion
}
