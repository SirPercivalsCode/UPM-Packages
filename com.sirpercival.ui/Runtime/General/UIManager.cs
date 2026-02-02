using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Transform uiCanvas;
    [SerializeField] private GameObject errorLog;
    [SerializeField] private TMP_Text errorLogText;

    [Header("Popups")]
    [SerializeField] private ConfirmationPopup confirmationPopupPrefab;
    [SerializeField] private bool confirmWhenPopupMissing = true;

    private ConfirmationPopup currentConfirmationPopup;
    public bool PopupActive => currentConfirmationPopup != null && currentConfirmationPopup.IsActive;

    [Header("Toasts")]
    [SerializeField] private Toast toastPrefab;
    [SerializeField] private Transform toastParent;

    [Header("Loading")]
    [SerializeField] private GameObject loadingSpinner;
    private int activeAwaitsCount;

    private void Start()
    {
        if(!uiCanvas) uiCanvas = GetComponentInChildren<Canvas>().transform;
    }

    #region Loading Spinner
    public void ShowSpinner()
    {
        if(_quitting) return;
        activeAwaitsCount++;
        loadingSpinner.SetActive(true);
    }
    public void HideSpinner()
    {
        if (_quitting) return;
        activeAwaitsCount = Mathf.Max(0, activeAwaitsCount - 1);
        if (activeAwaitsCount == 0)
            loadingSpinner.SetActive(false);
    }
    public async Task RunWithSpinner(Func<Task> asyncAction)
    {
        ShowSpinner();
        try
        {
            await asyncAction();
        }
        finally
        {
            HideSpinner();
        }
    }
    #endregion

    #region Popup
    private bool CheckConfirmationPopup(Action onConfirm)
    {
        if (currentConfirmationPopup != null) return true;
        if (confirmationPopupPrefab != null)
        {
            currentConfirmationPopup = Instantiate(confirmationPopupPrefab, uiCanvas);
            return true;
        }
        if (confirmWhenPopupMissing) onConfirm?.Invoke();    // this for testing
        return false;
    }
    public void ConfirmationPopup(string title, string message, ConfirmationPopupButtonData[] callbacks)
    {
        CheckConfirmationPopup(callbacks[0].ConfirmCallback);
        currentConfirmationPopup.Show(title, message, callbacks);
    }
    public void ConfirmationPopup(string title, string message, Action onConfirm)
    {
        CheckConfirmationPopup(onConfirm);
        currentConfirmationPopup.Show(title, message, onConfirm);
    }
    public void PopupConfirm(bool confirm)
    {
        if (!PopupActive) throw new ArgumentException("Popup isn't active but trying to confirm.");
        if (confirm) currentConfirmationPopup.Confirm();
        else currentConfirmationPopup.Cancel();
    }
    #endregion

    public void Toast(ToastType type, string message, bool fadeOut = true, Action onConfirm = null, string confirmBtnText = null)
    {
        if (_quitting)
        {
            Debug.LogWarning("UIManager is quitting, cannot show toast.");
            return;
        }
        Toast toast =  Instantiate(toastPrefab, toastParent);
        toast.Setup(type, message, fadeOut, onConfirm, confirmBtnText);
    }

    public void ShowTooltip(string text, Vector2 position) => ShowError("Tooltips not implemented yet!", true);
    public void ShowError(string message, bool fadeOut = false) => Toast(ToastType.Error, message, fadeOut, ShowErrorLog, "Show");
    public void ShowErrorLog() => errorLog.SetActive(true);
    public void AddToErrorLog(string errorMessage) => errorLogText.text += errorMessage + "\n\n";
}


public static class LoadingExtensions
{
    public static async Task WithLoadingSpinner(this Task task)
    {
        UIManager.Instance.ShowSpinner();
        try
        {
            await task;
        }
        finally
        {
            UIManager.Instance.HideSpinner();
        }
    }

    public static async Task<T> WithLoadingSpinner<T>(this Task<T> task)
    {
        UIManager.Instance.ShowSpinner();
        try
        {
            return await task;
        }
        finally
        {
            UIManager.Instance.HideSpinner();
        }
    }
}