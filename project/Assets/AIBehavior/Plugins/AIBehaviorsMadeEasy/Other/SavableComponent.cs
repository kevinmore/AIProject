using UnityEngine;
using System.Collections;

namespace AIBehavior
{
	public class SavableComponent : MonoBehaviour
	{
		// === Save Variables === //
		
		public int saveId = -1;


		// === Save Methods === //
		
		public int GetSaveID()
		{
			saveId = SaveIdDistributor.GetId(saveId);
			return saveId;
		}
	
	
		public void SetSaveID(int id)
		{
			saveId = id;
			SaveIdDistributor.SetId(id);
		}
	}
}