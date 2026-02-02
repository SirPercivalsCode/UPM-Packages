using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfirmationPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Transform btnParent;
    [SerializeField] private Button btnPrefab;

    public bool IsActive { get; private set; }

    private Action onConfirm;



    public void Show(string title, string message, ConfirmationPopupButtonData[] callbacks)
    {
        IsActive = true;
        titleText.text = title;
        messageText.text = message;

        EventSystem.current.SetSelectedGameObject(null);

        gameObject.SetActive(true);
        btnParent.DeleteChildren();
        Button btn = null;
        for (int i = 0; i < callbacks.Length; i++)
        {
            if(i >= 4) // max 3 buttons
            {
                Debug.LogWarning("ConfirmationPopup: Too many buttons, only showing first 3.");
                break;
            }
            btn = Instantiate(btnPrefab, btnParent);
            int temp = i; // capture the current index for the listener
            btn.onClick.AddListener(() =>
            {
                callbacks[temp].ConfirmCallback?.Invoke();
                Cancel();
            });
            btn.GetComponentInChildren<TextMeshProUGUI>().text = callbacks[i].ButtonText;
        }
    }
    public void Show(string title, string message, Action confirmCallback)
    {
        onConfirm = confirmCallback;
        ConfirmationPopupButtonData[] callbacks = new ConfirmationPopupButtonData[]
        {
            new ConfirmationPopupButtonData("Cancel", Cancel),
            new ConfirmationPopupButtonData("Confirm", Confirm)
        };
        Show(title, message, callbacks);
    }

    public void Confirm()
    {
        onConfirm?.Invoke();
        Cancel();
    }

    public void Cancel()
    {
        onConfirm = null;
        IsActive = false;
        gameObject.SetActive(false);
    }
}

public struct ConfirmationPopupButtonData
{
    public string ButtonText;
    public Action ConfirmCallback;

    public ConfirmationPopupButtonData(string buttonText, Action confirmCallback)
    {
        ButtonText = buttonText;
        ConfirmCallback = confirmCallback;
    }
}