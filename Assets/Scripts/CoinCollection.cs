using UnityEngine;

public class CoinCollection : MonoBehaviour
{

    ScoreManager score;
    ObjectAudioManager playerAudio;

    [SerializeField] int value = 100;

    private void Start()
    {
        playerAudio = GameObject.FindGameObjectWithTag("PlayerAudio").GetComponent<ObjectAudioManager>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // If the player touches this
        {
            score.IncreaseScore(value);
            playerAudio.Play("CoinGet");
            Destroy(this.transform.parent.gameObject); // Destroy this
        }
    }
}
