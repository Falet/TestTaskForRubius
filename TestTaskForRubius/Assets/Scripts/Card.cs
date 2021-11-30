using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Texture Texture2D
    {
        get => content.texture;
        set => content.texture = value;
    }
    
    [SerializeField] private RawImage content;
    [SerializeField] private Image back;
    [SerializeField] private float flipDurationShow;
    [SerializeField] private float flipDurationHide;
    [SerializeField] private Canvas canvas;
    
    private bool _isHide;
    
    private void Awake()
    {
        ChangeVisibilityFront(false);
        _isHide = true;
    }

    public async Task ShowContent()
    {
        if(_isHide)
        {
            await transform.DORotate(new Vector3(0, 90, 0), flipDurationShow).AsyncWaitForCompletion();
            ChangeVisibilityBack(false);
            ChangeVisibilityFront(true);
            await transform.DORotate(new Vector3(0, 0, 0), flipDurationShow).AsyncWaitForCompletion();
            _isHide = false;
        }
    }

    public async Task HideContent()
    {
        if(_isHide == false)
        {
            await transform.DORotate(new Vector3(0, 90, 0), flipDurationHide).AsyncWaitForCompletion();
            ChangeVisibilityFront(false);
            ChangeVisibilityBack(true);
            await transform.DORotate(new Vector3(0, 0, 0), flipDurationHide).AsyncWaitForCompletion();
            _isHide = true;
        }
    }

    private void ChangeVisibilityFront(bool isActivate)
    {
        canvas.enabled = isActivate;
    }

    private void ChangeVisibilityBack(bool isActivate)
    {
        back.enabled = isActivate;
    }
}
