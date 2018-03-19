﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Ennemy : MovingObject
{

    #region Basic components and scripts retrived with scripting

    // Current components of the gameobject
    private Animator m_ownAnimator;
    private Rigidbody2D m_ownRigidbody;
    private BoxCollider2D m_boxCollider;

    // Steering behavior script that make him move
    private EnnemySteeringBehavior steeringBehavior;

    #endregion

    // Will contains all the player present in the game
    public List<MovingObject> Players { get; set; }

    public int damages; // les dommages infligés au joueur en une attaque

    [SerializeField]
    private GenerateGrid m_grid;

    #region tweakable variables 

    [Header("Tweakable variables")]
    [SerializeField]
    private float maxSpeed = 1f;
    [SerializeField]
    private float harmDistance = 0f;

    [Header("Actifs forces")]
    [SerializeField]
    private bool onSeek;
    [SerializeField]
    private bool onArrive;
    [SerializeField]
    private bool onFollowPath;
    [SerializeField]
    private bool onRandom;
    [SerializeField]
    private bool onWallAvoidance;

    #endregion

    // Use this for initialization
    void Start()
    {
        //Get gameobject components
        m_boxCollider = GetComponent<BoxCollider2D>();
        m_ownRigidbody = GetComponent<Rigidbody2D>();
        m_ownAnimator = GetComponent<Animator>();

        // Create usefull scripts
        steeringBehavior = new EnnemySteeringBehavior(this, m_grid);

        // Retrieve all the players currently playing
        Players = new List<MovingObject>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            // Retrieve the script that take care of players position.
            Players.Add(player.GetComponent<MovingObject>());
        }

        // Find his position on the grid
        InitializePosition(m_grid);
    }

    // Update is called once per frame
    void Update()
    {

        // Update the movement of the bot.
        steeringBehavior.FixedUpdate();

    }

    #region Parameters accessors

    public bool OnSeek() { return onSeek; }

    public bool OnArrive() { return onArrive; }

    public bool OnFollowPath() { return onFollowPath; }

    public bool OnRandom() { return onRandom; }

    public bool OnWallAvoidance() { return onWallAvoidance; }

    public Rigidbody2D GetRigidbody() { return m_ownRigidbody; }

    public float GetHarmDistance() { return harmDistance; }

    public float GetMaxSpeed() { return maxSpeed; }

    public GridNode GetCurrentGridPosition() { return m_currentGridPosition; }

    #endregion
    
    public void OnTriggerEnter2D(Collider2D collision)
    {

        // attaque le joueur
        if (collision.tag.Equals("Player"))
        {
            collision.GetComponent<PlayerStatus>().LoseEnergy(damages);
        }

    }
}
