using UnityEngine;


namespace AIBehaviorExamples
{
	public class Alarm : MonoBehaviour
	{
		public AudioClip alarmSound;


		public void OnGetHelp()
		{
			audio.PlayOneShot(alarmSound);
		}
	}
}