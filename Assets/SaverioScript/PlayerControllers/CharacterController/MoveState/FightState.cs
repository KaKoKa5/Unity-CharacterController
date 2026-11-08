using System.Collections;
using System.Collections.Generic;
using Script.PlayerControllers;
using UnityEngine;

namespace CharacterController.Script
{
	public class FightState : IMoveState
	{
		public float MoveSpeed { get; set; }

		public void EnterState(CharacterSetup characterSetup)
		{
			characterSetup.Animator.SetBool("fight", true);
		}


		public void UpdateState(CharacterSetup characterSetup)
		{
			if (!characterSetup.ReadInput.IsFighting) characterSetup.GoTo(new NormalWalk());
		}

		public void ExitState(CharacterSetup characterSetup)
		{
			characterSetup.Animator.SetBool("fight", false);
		}
	}
}