using UnityEngine;
using UnityEngine.UI;
public class SpeedUpManager : MonoBehaviour
{
	private bool speedUp;
	[SerializeField] private Image btnimage;
	[SerializeField] private Color normalColor;
	[SerializeField] private Color speedUpColor;

	public void ToggleSpeedUp()
	{
		speedUp = !speedUp;

		if(speedUp )
		{
			Time.timeScale = 2;
			btnimage.color = speedUpColor;
		}
		else 
		{
			Time.timeScale = 1;
			btnimage.color = normalColor;
		}
	}
}
