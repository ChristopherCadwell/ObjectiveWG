using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Flasher : MonoBehaviour
{

    [SerializeField]
    private Color unEntered,
        entered,
        flashA,
        flashB;
    private bool hasEntered = false;
    
    [SerializeField, Header("The speed of the room flashing")] private float flashDelay = 0.2f;
    [SerializeField, Header("The speed of the frame animation")] private float frameDelay = 0.5f;
    private SpriteRenderer sprite;
    private SpriteRenderer boarder;
    private Camera cam;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();//get the sprite renderer so we can change colors
        boarder = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (!hasEntered)
        {
            sprite.enabled = false;
            boarder.enabled = false;
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<PlayerPawn>())
        {
            hasEntered = true;
            cam = GameObject.FindGameObjectWithTag("MapCam").GetComponent<Camera>();
            cam.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y, -10.0f);
            StartCoroutine(CoFlash());
            StartCoroutine(BoarderAnim());
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.GetComponent<PlayerPawn>())//if the thing colliding with us has a playerpawn object
        {
            StopAllCoroutines(); //stop flashing
            sprite.color = entered;//set color
            boarder.enabled = false;//hide boarder
        }
    }

    IEnumerator BoarderAnim()
    {
        boarder.enabled = true;
        Vector3 normal = new Vector3(1.15f, 2.0f, 1.0f);
        Vector3 large = new Vector3(2.0f, 2.85f, 1.0f);
        WaitForSeconds waitTime = new WaitForSeconds(frameDelay);

        while (true) 
        {
            if (boarder.transform.localScale == normal)
                boarder.transform.localScale = large;
            else
                boarder.transform.localScale = normal;

            yield return waitTime;            
        }


    }
    IEnumerator CoFlash() //the coroutine 
    {
        /*
         * Loops until stop is called from an ontriggerexit event.
         * this just changes the color of the minimap square the player
         * is in to show the active portion of that map
         */
        //show the room on the map
        sprite.enabled = true;
        WaitForSeconds waitTime = new WaitForSeconds(flashDelay);

        while (true)
        {
            
            if (sprite.color == flashA) //determine what color the box is
                sprite.color = flashB; //change color
            else
                sprite.color = flashA; //change color

            yield return waitTime;//pauses just the coroutine, then starts over after waitTime
        }
    }
}
