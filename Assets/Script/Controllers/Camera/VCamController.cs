using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;

public class VCamController : MonoBehaviour
{
    private GameManager gm;
    private CinemachineVirtualCamera vcam;
    public float pixelsPerUnit;
    public CinemachineConfiner confiner;

    // Start is called before the first frame update
    void Start()
    {
   
        gm = GameManager.Instance; //assign gm var
        vcam = GetComponent<CinemachineVirtualCamera>(); //find virtual cam component
        gm.cam = vcam;
        vcam.LookAt = gm.player.transform; // set lookat target
        vcam.Follow = gm.player.transform; // set follow target
        confiner = gm.confiner;
    }
}
