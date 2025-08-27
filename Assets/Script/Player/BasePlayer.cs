using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    private Rigidbody2D _rb;
    private Vector2 _input; //the input that the player makes for the movement

    #region Properties
    public Vector2 input { get => _input; set => _input = value; }
    public Rigidbody2D rb { get => _rb; private set => _rb = value; }
    #endregion

    #region Unity methods
    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        GetPlayerInput();

        MovePlayer();
    }
    #endregion

    #region Player input methods
    //gets the player's input
    private void GetPlayerInput()
    {
        input = new Vector2(
            Input.GetAxisRaw(horizontalAxis), 
            Input.GetAxisRaw(verticalAxis)
            ).normalized;
    }

    //moves the player
    public virtual void MovePlayer()
    {
        rb.velocity = input * speed;
    }
    #endregion
}
