using UnityEngine;

public class PlayerAuraCenter : MonoBehaviour
{
    #region Public Fields

    public Transform player;
    [Range(0.0f, 1.0f)]
    public float interested; //how interested the follower is in the thing it's following :)
    #endregion

    private Transform _transform;
    
    #region Unity Methods
    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        // Follow Player, but rotate on it's own 
        // Can create a delayed following using the "interested" var set proportion to the distance of the player
        // closer : slower following
        _transform.position = Vector3.MoveTowards(transform.position, player.transform.position, interested);
    }
    #endregion

    #region Private Methods
    #endregion
}
