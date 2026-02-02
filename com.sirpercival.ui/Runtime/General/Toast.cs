using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ToastType
{
    Success,
    Info,
    Warning,
    Error
}

public class Toast : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private GameObject successTitle, infoTitle, warningTitle, errorTitle;
    [SerializeField] private Button btnAction;
    [SerializeField] private TMP_Text btnText;

    public void Setup(ToastType type, string msg, bool fadeOut, Action onConfirm, string confirmBtnText)
    {
        // do fadeout with DoTween
        messageText.text = msg;
        EnableTitleGO(type);
        btnAction.gameObject.SetActive(onConfirm != null);
        if (onConfirm != null)
        {
            btnText.text = confirmBtnText;
            btnAction.onClick.AddListener(() => onConfirm?.Invoke());
        }
        if (fadeOut) Destroy(gameObject, 5f);
    }

    private void EnableTitleGO(ToastType type)
    {
        successTitle.SetActive(type == ToastType.Success);
        infoTitle.SetActive(type == ToastType.Info);
        warningTitle.SetActive(type == ToastType.Warning);
        errorTitle.SetActive(type == ToastType.Error);
    }
    public void Kill()
    {
        Destroy(gameObject);
    }
}