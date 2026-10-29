using UnityEngine;

public class SlideMenuAnim : MonoBehaviour
{
	[SerializeField] private GameObject PanelMenu;
	
	public void ShowHideMenu()
	{
		if(PanelMenu != null)
		{
			Animator animator = PanelMenu.GetComponent<Animator>();
			if(animator != null )
			{
				bool isOpen = animator.GetBool("show");
				animator.SetBool("show", !isOpen);
			}
		}
	}
}
