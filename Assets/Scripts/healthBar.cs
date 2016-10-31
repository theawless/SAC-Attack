using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class healthBar : MonoBehaviour
{

    float maxHealth = 100;
    float maxPower = 100;

    float P1health = 100;
    float P1power = 100;

    float P2health = 100;
    float P2power = 100;

    public Image P1currenthealthBar;
    public Image P1powerBar;

    public Image P2currenthealthBar;
    public Image P2powerBar;
    [SerializeField]
    private CharacterStats player2 = null;
    [SerializeField]
    private CharacterStats player1 = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player1 == null)
            player1 = GameObject.FindGameObjectWithTag(TagsTypeString.Player1.ToString()).GetComponent<CharacterStats>();
        if (player2 == null)
            player2 = GameObject.FindGameObjectWithTag(TagsTypeString.Player2.ToString()).GetComponent<CharacterStats>();

        Debug.Log(GameObject.FindGameObjectWithTag(TagsTypeString.Player2.ToString()));
        if (player1 == null || player2 == null) { Debug.Log("WHAT"); return; }

        P1health = player1.Health;
        P1power = player1.Power;
        P2health = player2.Health;
        P2power = player2.Power;

        if (P1health <= 0 || P2health <= 0)
        {
            var wins = "";
            if (P1health <= 0) { wins = "Player 2"; }
            if (P2health <= 0) { wins = "Player 1"; }
            var canvas = GameObject.FindGameObjectWithTag("Canvas");
            GameObject.FindGameObjectWithTag("Text").GetComponent<Text>().text = wins + " Wins!";
            StartCoroutine("LoadScene");
        }

        UpdateHealthBar(P1currenthealthBar, P1health, P1powerBar, P1power);
        UpdateHealthBar(P2currenthealthBar, P2health, P2powerBar, P2power);
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3f);
        Destroy(GameObject.FindGameObjectWithTag("GameInfo"));
        SceneManager.LoadScene(0);
    }
    void UpdateHealthBar(Image currenthealthBar, float health, Image powerBar, float power)
    {
        float healthRatio = health / maxHealth;
        float powerRatio = power / maxPower;
        if (healthRatio < 0)
            healthRatio = 0;
        if (powerRatio < 0)
            powerRatio = 0;

        currenthealthBar.rectTransform.localScale = new Vector3(healthRatio, 1, 1);
        powerBar.rectTransform.localScale = new Vector3(powerRatio, 1, 1);
    }
}
