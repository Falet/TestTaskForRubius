using System.Threading.Tasks;
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
        
    }

    public async Task HideContent()
    {
        
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
