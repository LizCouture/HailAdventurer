using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerTag : MonoBehaviour
{
    public string playerName = "playerName";
    public Sprite playerAvatar;
    public TextMeshProUGUI playerLabel;
    public Image avatarFrame;

    // Start is called before the first frame update
    void Start()
    {
      if (playerName != null && playerLabel != null)
        {
            playerLabel.text = playerName;
        } 
      if (playerAvatar != null && avatarFrame != null)
        {
            avatarFrame.sprite = playerAvatar;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
