using System.Collections;
using System.Collections.Generic;
using CoinPackage.Debugging;
using GridSystem;
using UnityEditor.PackageManager;
using UnityEngine;

public class DummyMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null)
            {
                hit.collider.GetComponent<Cell>()?.OnLeftClicked();
            }
        }
        
        if (Input.GetMouseButton(1)){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null)
            {
                hit.collider.GetComponent<Cell>()?.OnRightClicked();
            }
        }
    }
}
