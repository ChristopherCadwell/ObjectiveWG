using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPawn : Pawn
{
    #region Variables
    #region Enemy Combat Attributes
    [Header("Enemy Combat Attributes"), SerializeField, Tooltip("Current target of enemy")]
    protected Pawn target; //specifies the target of the enemy
    protected LayerMask playerLayer; //Specifies the layer this player occupies for enemy attacks.
    #endregion
    #endregion


    #region Functions

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Debug.Log("It Hit");
        }
    }

    

    #endregion
}
