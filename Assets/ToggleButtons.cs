using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtons : MonoBehaviour
{
    [SerializeField] private bool usesParenting=false;
    [SerializeField] private List<Button> toggleButtons = new List<Button>();
    private Button currentButton;
    [SerializeField] private Color colorWhenSelected;
    [SerializeField] private Color colorWhenUnselected;

    private void Awake()
    {
        if (!usesParenting)
        {
            if(toggleButtons.Count<=0) return;
            foreach (var button in toggleButtons)
            {
                button.image.color = colorWhenUnselected;
            }
            toggleButtons[0].onClick.Invoke();
        }
        else
        {
            var children = transform.GetComponentsInChildren<Button>(true).ToList();
            if(children.Count<=0) return;
            foreach (var child in children)
            {
                child.image.color = colorWhenUnselected;
            }
            children[0].onClick.Invoke();
        }
        
    }

    public void MeTriggered(Button btn)
    {
        if (currentButton!=null)
        {
            currentButton.image.color = colorWhenUnselected;
        }
        
        btn.image.color = colorWhenSelected;
        currentButton = btn;
    }
}
