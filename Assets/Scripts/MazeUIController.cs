using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeUIController : MonoBehaviour
{
    public Image infirIMG, serptIMG, eclypIMG, drakeIMG, crossIMG;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GAME.infir) infirIMG.enabled = true;
        if (!GameManager.GAME.infir) infirIMG.enabled = false;
        if (GameManager.GAME.serpt) serptIMG.enabled = true;
        if (!GameManager.GAME.serpt) serptIMG.enabled = false;
        if (GameManager.GAME.eclyp) eclypIMG.enabled = true;
        if (!GameManager.GAME.eclyp) eclypIMG.enabled = false;
        if (GameManager.GAME.drake) drakeIMG.enabled = true;
        if (!GameManager.GAME.drake) drakeIMG.enabled = false;
        if (GameManager.GAME.cross) crossIMG.enabled = true;
        if (!GameManager.GAME.cross) crossIMG.enabled = false;
    }
}
