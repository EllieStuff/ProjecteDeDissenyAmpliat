using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private GameObject Weaponary;
    [SerializeField] private GameObject CanvasUI;
    private int _tutoState;
    public Image img1;
    private bool _tutoActive;

    void Start()
    {
        _tutoActive = true;
        _tutoState = 0;
        Weaponary.SetActive(false);
        CanvasUI.SetActive(false);
    }

    void Update()
    {
        switch (_tutoState)                         // HACER ESCENA TUTORIAL ONLI y CAMBIAR ESCENAS CUANDO ACABE TUTO
        {
            case 0:                                 // Texto explicando como hacer las cosas: coger y arrastrar arma, pasar stage y tal
                _tutoActive = true;
                img1.gameObject.SetActive(true);
                break;
            case 1:                                 // Texto explicando que hay diferentes pesos y posiciones entre las que puedes elegir y tal
                img1.gameObject.SetActive(false);
                break;
            case 2:                                 // Texto explicando como van los sliders y coleccionables y tal

                break;
            case 3:                                 // Texto explicando el rewind hispano de alecmonlon y que pueden ver la fórmula y tal

                break;
            default:                                // Acaba tutorial 
                _tutoActive = false;
                Weaponary.SetActive(true);
                CanvasUI.SetActive(true);
                break;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && _tutoActive) _tutoState++;
    }
}
