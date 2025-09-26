using UnityEngine;
using TMPro;

public class victoryStat : MonoBehaviour
{
    public TextMeshProUGUI statText;

    private void Awake() {
        statText.text = "During your adventure you collected " + GameManager.Instance.totalFriends + " friends!";
    }
}
