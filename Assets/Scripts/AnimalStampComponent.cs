using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimalStampComponent :  MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform spriteRectTransform;
    private bool isDragging;
    private Vector2 pointerOffset;
    public float scale = 1f;

    public PaperGameManager paperGameManager;
    public bool inPaper =false;
    public bool collides =false;
    public bool inAnimalZone = true;
    

    void Start()
    {
    inPaper =false;
    collides =false;
   inAnimalZone = true;
        // Get the RectTransform component of the sprite
        spriteRectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Calculate the offset between the pointer position and the sprite position
        pointerOffset = eventData.position - spriteRectTransform.anchoredPosition;
        isDragging = true;
        if (!collides && !inPaper)
        {
            this.GetComponent<Image>().color = new Color(255,255,255,255);
        }
        else
        {
            //TODO throw it pack to animal zone
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            // Update the sprite position based on the pointer position and offset
            spriteRectTransform.anchoredPosition = eventData.position - pointerOffset;
            if (isInsideZone(paperGameManager.animalZoneCorners,paperGameManager.animalZoneCollider))
            {
                this.GetComponent<Image>().color = new Color(255,255,255,255);       
                inAnimalZone = true;
                collides = false;
            }
            else if(isInsideZone(paperGameManager.paperCorners,paperGameManager.paperCollider))
            {
                if (!doesCollideWithOtherStamps())
                {
                    this.GetComponent<Image>().color = Color.green;
                    inPaper = true;
                    collides = false;
                    inAnimalZone = false; 
                }
                else
                {
                    this.GetComponent<Image>().color = Color.red;
                    inPaper = true;
                    collides = true;
                    inAnimalZone = false; 
                }
             
            }else
            {
                this.GetComponent<Image>().color = Color.red;
                inPaper = false;
                inAnimalZone = false;
                collides = false;
            }
        }
    }
    
    bool doesCollideWithOtherStamps()
    {
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        
        foreach (var stamp in paperGameManager.stamps)
        {
            if (stamp != this)
            {
                if (polygonCollider.IsTouching(stamp.GetComponent<PolygonCollider2D>()))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    bool isInsideZone(Vector3[] corners,BoxCollider2D collider2D)
    {
        RectTransform rt = this.GetComponent<RectTransform>();
        PolygonCollider2D polygonCollider = this.GetComponent<PolygonCollider2D>();
        Debug.Log("Sprite pos:" + rt.position.ToString());
        Debug.Log("Border pos:" + corners[0].ToString() + corners[1].ToString());
        
        bool isIside = true;
        foreach (var corner in polygonCollider.GetPath(0))
        {
            if (!collider2D.bounds.Contains(corner + rt.anchoredPosition))
            {
                isIside = false;
            }
        }
        return isIside;
    }
}

