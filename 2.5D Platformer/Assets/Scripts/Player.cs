using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controller;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _gravity = 1.0f;
    [SerializeField]
    private float _jumpHeight = 15.0f;
    private float _yVelocity;
    [SerializeField]
    private bool _doubleJump;
    [SerializeField]
    private int _coins;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _respawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("Ui Manager Null");
        }
        _uiManager.UpdateLivesDisplay(_lives);

    }

    // Update is called once per frame
    void Update()
    {
        

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, 0);
        Vector3 velocity = direction * _speed;

        if (_controller.isGrounded == true)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _doubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _doubleJump == true)
            {
                _yVelocity += _jumpHeight * 1.3f;
                _doubleJump = false;
            }
            _yVelocity -= _gravity;

        }

        velocity.y = _yVelocity;

        _controller.Move(velocity * Time.deltaTime);

        if (transform.position.y < -10.0f)
        {
            CharacterController cc = GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
            }

            transform.position = _respawnPoint.transform.position;

            StartCoroutine(CCEnableRoutine(cc));

            Fallen();
        }


    }
    public void AddCoins()
    {
        _coins++;
        _uiManager.UpdateCoinDisplay(_coins);
    }

    public void Fallen()
    {
        _lives--;

        _uiManager.UpdateLivesDisplay(_lives);

        if (_lives < 1)
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator CCEnableRoutine(CharacterController controller)
    {
        yield return new WaitForSeconds(0.5f);
        controller.enabled = true;
    }
    



}
