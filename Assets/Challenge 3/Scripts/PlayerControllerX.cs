using System.Collections;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
	public bool gameOver;
	private float floatForce = 40.0f;
	private float gravityModifier = 1.5f;
	private Rigidbody playerRb;
	private float yRangeUp = 15;
	private float yRangeDown = 0;

	public ParticleSystem explosionParticle;
	public ParticleSystem fireworksParticle;

	private AudioSource playerAudio;
	public AudioClip moneySound;
	public AudioClip explodeSound;
	public AudioClip bounceSound;



	// Start is called before the first frame update
	void Start()
	{
		Physics.gravity *= gravityModifier;
		playerAudio = GetComponent<AudioSource>();

		// Initialize the playerRb by getting the Rigidbody component from the GameObject
		playerRb = GetComponent<Rigidbody>();

		// Apply a small upward force at the start of the game
		playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

	}

	// Update is called once per frame
	void Update()
	{
		//keep player in bound
		if (transform.position.y > yRangeUp)
		{
			transform.position = new Vector3(transform.position.x, yRangeUp - 0.5f, transform.position.z);
		}
		if (transform.position.y < yRangeDown)
		{
			transform.position = new Vector3(transform.position.x, yRangeDown + 2, transform.position.z);
			playerAudio.PlayOneShot(bounceSound, 1.0f);
		}
		// While space is pressed and player is low enough, float up
		if (Input.GetKey(KeyCode.Space) && !gameOver)
		{
			playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		// if player collides with bomb, explode and set gameOver to true
		if (other.gameObject.CompareTag("Bomb"))
		{
			StartCoroutine(ExplodeAndDestroy(other.gameObject));
		}
		// if player collides with money, fireworks
		else if (other.gameObject.CompareTag("Money"))
		{
			fireworksParticle.Play();
			playerAudio.PlayOneShot(moneySound, 1.0f);
			Destroy(other.gameObject);
		}
	}

	private IEnumerator ExplodeAndDestroy(GameObject bombObject)
	{
		explosionParticle.Play();
		playerAudio.PlayOneShot(explodeSound, 1.0f);
		gameOver = true;
		Debug.Log("Game Over!");

		// Wait for a few seconds to allow the explosion and audio to finish
		yield return new WaitForSeconds(0.5f); // Adjust the time as needed

		// Now destroy the bomb object
		Destroy(bombObject);

		// Destroy the object with this script
		Destroy(gameObject);

	}

}
