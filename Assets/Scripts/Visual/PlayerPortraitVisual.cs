using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlayerPortraitVisual : MonoBehaviour {

    public CharacterAsset charAsset;
    [Header("Text Component References")]
    //public Text NameText;
    public TextMeshProUGUI NameLabel;
    [Header("Image References")]
    public Image PortraitImage;

    void Awake()
	{
		if(charAsset != null)
			ApplyLookFromAsset();
	}
	
	public void ApplyLookFromAsset()
    {
        NameLabel.text = charAsset.name.ToString();
        NameLabel.color = charAsset.PlayerTextTint;
        PortraitImage.sprite = charAsset.AvatarImage;
    }


 /*   public void Explode()
    {
        Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.PrependInterval(2f);
        s.OnComplete(() => GlobalSettings.Instance.GameOverPanel.SetActive(true));
    }*/



}
