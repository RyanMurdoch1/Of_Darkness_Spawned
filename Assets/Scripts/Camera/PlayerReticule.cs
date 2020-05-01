using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReticule : MonoBehaviour
{
    [SerializeField] private Image reticule;
    [SerializeField] private Sprite[] reticulesStates;
    [SerializeField] private RectTransform canvasRect;
    private Vector2 _mousePosition;

    private void Awake()
    {
        Cursor.visible = false;
        BowState.DrawBow += BowDrawn;
        BowState.BowForce += BowCharge;
    }

    private void OnDisable()
    {
        BowState.DrawBow -= BowDrawn;
        BowState.BowForce -= BowCharge;
    }

    private void Update()
    {
        gameObject.transform.localPosition = new Vector2(Input.mousePosition.x - canvasRect.sizeDelta.x/2f, Input.mousePosition.y - canvasRect.sizeDelta.y/2f);
    }

    private void BowDrawn(bool value)
    {
        reticule.gameObject.SetActive(value);
    }

    private void BowCharge(int charge)
    {
        reticule.sprite = reticulesStates[charge];
    }
}
