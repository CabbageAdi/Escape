using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cabbage.Helpers;
using Ludiq;
using MiscUtil.Collections.Extensions;
using Photon.Pun;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class PlayerMovement : MonoBehaviour
{
    public float HoldTime = 1;
    public float HoldSpeed = 1;

    public TileBase FloorTile;
    
    private PowerBlock[] Blocks;
    private Gate[] Gates; 
    
    private Tilemap _tilemap;
    private PowerCalculation _powerCalculation;
    private UIManager _uiManager;

    private PhotonView _photonView;
    private GameUI _gameUIManager;

    private AudioSource _audioSource;

    void Start()
    {
        this.Blocks = FindObjectsOfType<PowerBlock>();
        this.Gates = FindObjectsOfType<Gate>();
        foreach (var block in Blocks)
        {
            block.transform.Find("Selected").gameObject.SetActive(false);
        }

        _tilemap = FindObjectOfType<Tilemap>();
        _powerCalculation = FindObjectOfType<PowerCalculation>();
        _uiManager = FindObjectOfType<UIManager>();

        _photonView = GetComponent<PhotonView>();
        _gameUIManager = FindObjectOfType<GameUI>();

        _audioSource = GetComponent<AudioSource>();
    }

    private float horizontalTime;
    private float verticalTime;

    private float timeSinceHorizontalMove;
    private float timeSinceVerticalMove;
    
    private PowerBlock prevBlock;
    void Update()
    {
        if (!_photonView.IsMine) return;
        if (_gameUIManager.Paused) return;
        
        if (Input.GetAxis("Horizontal") != 0)
            horizontalTime += Time.deltaTime;
        else horizontalTime = 0;
        if (Input.GetAxis("Vertical") != 0)
            verticalTime += Time.deltaTime;
        else verticalTime = 0;

        var horizontal = this.GetAxisDown(Axis.Horizontal);
        var vertical = this.GetAxisDown(Axis.Vertical);

        // if (horizontalTime > HoldTime)
        // {
        //     horizontal = Input.GetAxis("Horizontal");
        // }
        //
        // if (verticalTime > HoldTime)
        // {
        //     vertical = Input.GetAxis("Vertical");
        // }

        int? rotation = null;
        
        if (horizontal == 1)
            rotation = -90;
        else if (horizontal == -1)
            rotation = 90;
        else if (vertical == 1)
            rotation = 0;
        else if (vertical == -1)
            rotation = 180;
        else
            rotation = (int)this.transform.rotation.eulerAngles.z;
        
        this.transform.rotation = Quaternion.Euler(0, 0, rotation.Value);

        var facingPosition = this.transform.position;
        
        switch (rotation)
        {
            case 0:
                facingPosition += new Vector3(0, 1);
                break;
            case 180:
                facingPosition += new Vector3(0, -1);
                break;
            case 270:
            case -90:
                facingPosition += new Vector3(1, 0);
                break;
            case 90:
                facingPosition += new Vector3(-1, 0);
                break;
        }
        
        if (prevBlock != null)
            prevBlock.transform.Find("Selected").gameObject.SetActive(false);

        var facingBlock = Blocks.FirstOrDefault(b => b.transform.position == facingPosition);
        
        var canMove = true;
        if (facingBlock != null) canMove = false;
        if (Gates.Any(g => g.transform.position == facingPosition && !g.Open)) canMove = false;
        var facingName = _tilemap.GetTile(facingPosition.ToInt(-1, -1))?.name.ToLower();
        if (facingName != null && (facingName.Contains("tree") || facingName.Contains("rock"))) canMove = false;

        if (canMove)
        {
            var actualHorizontal = horizontal;
            var actualVertical = vertical;
            /*if (horizontalTime > HoldTime)
            {
                if (timeSinceHorizontalMove >= (1 / HoldSpeed))
                {
                    timeSinceHorizontalMove = 0;
                }
                else
                {
                    actualHorizontal = 0;
                }
                timeSinceHorizontalMove += Time.deltaTime;
            }
            if (verticalTime > HoldTime)
            {
                if (timeSinceVerticalMove >= (1 / HoldSpeed))
                {
                    timeSinceVerticalMove = 0;
                }
                else
                {
                    actualVertical = 0;
                }
                timeSinceVerticalMove += Time.deltaTime;
            }*/
            this.transform.position += new Vector3(actualHorizontal, actualVertical);
        }
        else if (facingBlock != null)
        {
            facingBlock.transform.Find("Selected").gameObject.SetActive(true);
        }
        prevBlock = facingBlock;

        if (Input.GetKeyDown(KeyCode.R) && facingBlock != null)
        {
            facingBlock.Rotate();
            _audioSource.Play();
            _powerCalculation.CalculateEndpoints(facingBlock);
        }

        if (_tilemap.GetTile(this.transform.position.ToInt(-1, -1))?.name == "orb")
        {
            _uiManager.AddCollectable();
            _tilemap.SetTile(this.transform.position.ToInt(-1, -1), FloorTile);
        }

        if (transform.position.x > 17 && transform.position.y < 4 && transform.position.y > -3)
            SceneManager.LoadScene(3);
    }
}