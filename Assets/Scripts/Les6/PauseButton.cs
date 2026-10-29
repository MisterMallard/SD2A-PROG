using UnityEngine;

public class PauseButton : MonoBehaviour
{
	private bool pause;

	public void Pause(){
		pause = !pause;

		if(pause){
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}
}
