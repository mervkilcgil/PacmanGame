using System;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Button primaryButton;

    [SerializeField] protected bool isOpen;

    public void Start()
    {
        OnStartPanel();
        if(isOpen)
            Open();
        else
            Close();
    }

    protected virtual void OnStartPanel()
    {
        
    }

    public virtual void Open()
    {
        canvas.gameObject.SetActive(true);
        isOpen = true;
    }

    public virtual void Close()
    {
        canvas.gameObject.SetActive(false);
        isOpen = false;
    }
}