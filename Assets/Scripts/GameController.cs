using System.Collections;
using UnityEngine;
using TMPro;


public class GameController : MonoBehaviour
{
	public static GameController Instance { get; private set; }

	public delegate void GameDelegate();
	public event GameDelegate OnTimeIsUp;

	[SerializeField] private int platformCount = 10;
	[SerializeField] private float levelTime = 60;
	[SerializeField] private TextMeshProUGUI timerText = null;
	[SerializeField] private TextMeshProUGUI platformCountText = null;
	[SerializeField] private GameObject defeatContainer = null;
	[SerializeField] private GameObject victoryContainer = null;
	private float timer;
	private bool timeIsUp;
	private bool stopGame = false;
	public bool CanConsumePlatform { get => platformCount > 0; }

	public void ConsumePlatform()
	{
		if (platformCount <= 0) return;

		platformCount--;
		platformCountText.text = platformCount.ToString();
	}

	public void Victory()
	{
		if (stopGame) return;
		stopGame = true;

		victoryContainer.SetActive(true);
		StartCoroutine(ReloadDelay());
	}

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;

		defeatContainer.SetActive(false);
		victoryContainer.SetActive(false);
	}

	private void Start()
	{
		timer = levelTime;
		platformCountText.text = platformCount.ToString();
		OnTimeIsUp += Defeat;
	}

	private void Defeat()
	{
		if (stopGame) return;
		stopGame = true;

		defeatContainer.SetActive(true);
		StartCoroutine(ReloadDelay());
	}

	private void Update()
	{
		if (stopGame) return;

		timer -= Time.deltaTime;
		timerText.text = timer.ToString("0:00");

		if (timer <= 0 && !timeIsUp)
		{
			timer = 0;
			timeIsUp = true;
			OnTimeIsUp?.Invoke();
		}
	}

	private IEnumerator ReloadDelay()
	{
		yield return new WaitForSeconds(3);

		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}
}