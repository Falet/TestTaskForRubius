using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image mask;
    [SerializeField] private RawImage content;
    [SerializeField] private Image front;
    [SerializeField] private Image back;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float flipDurationShow;
    [SerializeField] private float flipDurationHide;
    
    private bool IsHide;
    
    private void Awake()
    {
        ChangeVisibilityFront(false);
        IsHide = true;
    }

    public void SetTexture(Texture2D texture)
    {
        content.texture = texture;
    }

    public async Task ShowContent()
    {
        if(IsHide)
        {
            await transform.DORotate(new Vector3(0, 90, 0), flipDurationShow).AsyncWaitForCompletion();
            ChangeVisibilityBack(false);
            ChangeVisibilityFront(true);
            await transform.DORotate(new Vector3(0, 0, 0), flipDurationShow).AsyncWaitForCompletion();
            IsHide = false;
        }
    }

    public async Task HideContent()
    {
        if(IsHide == false)
        {
            await transform.DORotate(new Vector3(0, 90, 0), flipDurationHide).AsyncWaitForCompletion();
            ChangeVisibilityFront(false);
            ChangeVisibilityBack(true);
            await transform.DORotate(new Vector3(0, 0, 0), flipDurationHide).AsyncWaitForCompletion();
            IsHide = true;
        }
    }

    private void ChangeVisibilityFront(bool isActivate)
    {
        front.enabled = isActivate;
        content.enabled = isActivate;
        mask.enabled = isActivate;
        text.enabled = isActivate;
    }

    private void ChangeVisibilityBack(bool isActivate)
    {
        back.enabled = isActivate;
    }
}
