using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

public class Parallax : MonoBehaviour
{
    #region Vars
    #region Objects
    private GameObject locationCheck;
    private CinemachineVirtualCamera cam;
    private Transform player;
    private SpriteRenderer loopRenderer;
    private Scene scene;

    #endregion
    #region Numbers
    private float startZ;
    private Vector2 startPos;
    private Vector2 temp;
    private Vector2 distance;
    #endregion

    #region User Options
    [Header("Parallax the X axis?")] public bool xParallax;
    [Header("Parallax the Y axis?")] public bool yParallax;
    [Header("Should object follow the camera?")] public bool stationary;
    [Header("Should the parallaxed object be repeted infinitely?")] public bool infiniteLoop;
    [Header("Parallax Factor(-100 to 100)")] public float parallax;

    #endregion
    #region Properties
    private Vector2 travel => (Vector2)locationCheck.transform.position - startPos;
    private float parallaxFactor => parallax / 100;
    private float doubleAspect => cam.m_Lens.Aspect * 2;
    private float tileX => (doubleAspect > 3 ? doubleAspect : 3);
    private float viewX => loopRenderer.sprite.rect.width / loopRenderer.sprite.pixelsPerUnit;
    #endregion
    #endregion


    private void OnEnable()
    {
        //check what scene and run scenloaded functions.  This should load every time a scene change occurs
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene loaded, LoadSceneMode mode)
    {
        scene = loaded;//update what scene we are in

        if (GameSettings.Instance != null)
        {
            if (scene.name != "MainMenu")
            {
                loopRenderer = GetComponent<SpriteRenderer>();
                startPos = transform.position;
                startZ = transform.position.z;
                cam = GameObject.FindGameObjectWithTag("ParallaxCam").GetComponent<CinemachineVirtualCamera>();
                locationCheck = GameObject.FindGameObjectWithTag("CMPipe");
                player = cam.LookAt;

                if (loopRenderer != null && infiniteLoop)
                {
                    float spriteSizeX = loopRenderer.sprite.rect.width / loopRenderer.sprite.pixelsPerUnit;
                    float spriteSizeY = loopRenderer.sprite.rect.height / loopRenderer.sprite.pixelsPerUnit;

                    loopRenderer.drawMode = SpriteDrawMode.Tiled;
                    loopRenderer.size = new Vector2(spriteSizeX * tileX, spriteSizeY);
                    transform.localScale = Vector3.one;
                }
            }
        }
        else
            return;
    }


    private void FixedUpdate()
    {
        Vector2 newPos = startPos + travel * parallaxFactor;
        transform.position = new Vector3(xParallax? newPos.x : startPos.x, yParallax? newPos.y : startPos.y, startZ);

        if (infiniteLoop)
        {
            Vector2 totalTravel = locationCheck.transform.position - transform.position;
            float boundsOffset = (viewX / 2) * (totalTravel.x > 0 ? 1 : -1);
            float screens = (int)((totalTravel.x + boundsOffset) / viewX);
            transform.position += new Vector3(screens * viewX, 0);
        }

    }
    
    
    public void OnDisable()
    {
        //just because it is proper to unsub delegates
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

