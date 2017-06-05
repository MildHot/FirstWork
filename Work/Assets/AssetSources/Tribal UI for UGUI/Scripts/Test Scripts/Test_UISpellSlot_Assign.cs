using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Test_UISpellSlot_Assign : MonoBehaviour {

	public UISpellSlot slot;
	public UISpellDatabase spellDatabase;
	public int assignSpell;
	
	void Awake()
	{
		if (this.slot == null)
			this.slot = this.GetComponent<UISpellSlot>();
	}
	
	void Start()
	{
		if (this.slot == null || this.spellDatabase == null)
		{
			this.Destruct();
			return;
		}
		
		this.slot.Assign(this.spellDatabase.GetByID(this.assignSpell));
		this.Destruct();
	}
	
	[ContextMenu("Assign")]
	public void Assign()
	{
		if (this.slot == null || this.spellDatabase == null)
		{
			return;
		}
		
		this.slot.Assign(this.spellDatabase.GetByID(this.assignSpell));
	}
	
	private void Destruct()
	{
		DestroyImmediate(this);
	}
}
