using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfoView : MonoBehaviour
{
    public NetworkPlayer np;
    public TextMeshProUGUI NicknameField;
    public TextMeshProUGUI CoinsField;
    public Image Avatar;

    public GameObject coinPrefab;
    public Transform coinGivePosition;

    public string Nickname = "";
    public int Coins = 0;
    public CharacterAsset ca;

    public void loadFromPlayer(NetworkPlayer np)
    {
        this.np = np;
        Nickname = np.nickname;
        Coins = np.coins;
        ca = np.avatar;
        loadVisualsFromObject();
    }

    public void loadVisualsFromObject()
    {
        NicknameField.text = Nickname;
        CoinsField.text = Coins.ToString();
        Avatar.sprite = ca.AvatarImage;
    }

    public IEnumerator giveCoin()
    {
        GameObject newCoin = Instantiate(coinPrefab, coinGivePosition.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.Append(newCoin.transform.DOMove(gameObject.transform.position, 1.0f));
        Animator animator = newCoin.GetComponent<Animator>();
        animator.Play("FlipCoin");
        Destroy(newCoin, animator.GetCurrentAnimatorStateInfo(0).length);
        Coins++;
        CoinsField.text = Coins.ToString();
        yield return null;
    }
}
