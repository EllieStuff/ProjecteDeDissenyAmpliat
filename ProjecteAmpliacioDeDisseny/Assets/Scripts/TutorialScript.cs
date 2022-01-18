using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    public int _tutoState;
    public Image img0, img1, img2, img3, imgWin, odinChikito;
    public bool _tutoActive;
    public int lastTutoState;

    void Start()
    {
        _tutoActive = true;
        _tutoState = 0;
        lastTutoState = 0;
        img0.gameObject.SetActive(false);
        img1.gameObject.SetActive(false);
        img2.gameObject.SetActive(false);
        img3.gameObject.SetActive(false);
        imgWin.gameObject.SetActive(false);
        odinChikito.gameObject.SetActive(false);
    }

    void Update()
    {
        switch (_tutoState)                         // HACER ESCENA TUTORIAL ONLI y CAMBIAR ESCENAS CUANDO ACABE TUTO
        {
            case 0:                                 // Texto explicando como hacer las cosas: coger y arrastrar arma, pasar stage y tal
                _tutoActive = true;
                img0.gameObject.SetActive(true);
                odinChikito.gameObject.SetActive(true);
                break;
            case 1:                                 // Texto explicando que hay diferentes pesos y posiciones entre las que puedes elegir y tal
                img0.gameObject.SetActive(false);      
                img1.gameObject.SetActive(true);
                odinChikito.gameObject.SetActive(true);
                break;
            case 2:                                 // Texto explicando como van los sliders y coleccionables y tal
                img1.gameObject.SetActive(false);
                img2.gameObject.SetActive(true);
                odinChikito.gameObject.SetActive(true);
                break;
            case 3:                                 // Texto explicando el rewind hispano de alecmonlon y que pueden ver la fórmula y tal
                img2.gameObject.SetActive(false);
                img3.gameObject.SetActive(true);
                odinChikito.gameObject.SetActive(true);
                break;
            case 4:
                img3.gameObject.SetActive(false);
                imgWin.gameObject.SetActive(true);
                odinChikito.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    SceneManager.LoadScene("Tutorial");
                break;
            case 5:
                img0.gameObject.SetActive(false);
                img1.gameObject.SetActive(false);
                img2.gameObject.SetActive(false);
                img3.gameObject.SetActive(false);
                imgWin.gameObject.SetActive(false);
                odinChikito.gameObject.SetActive(false);
                break;
            default:                                // Acaba tutorial 
                img0.gameObject.SetActive(false);
                img1.gameObject.SetActive(false);
                img2.gameObject.SetActive(false);
                img3.gameObject.SetActive(false);
                imgWin.gameObject.SetActive(false);
                odinChikito.gameObject.SetActive(false);
                _tutoActive = false;
                
                break;
        }
        //if (Input.GetKeyDown(KeyCode.Mouse0) && _tutoActive) _tutoState++;
    }
}
