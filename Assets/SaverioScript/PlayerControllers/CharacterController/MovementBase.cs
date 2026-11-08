using CharacterController.SaverioScript.PlayerControllers.CharacterController;
using CharacterController.Script.PlayerControllers;
using UnityEngine;

namespace CharacterController.Script
{
	public class MovementBase : MonoBehaviour,IAptitude
	{
		[SerializeField] public UnityEngine.CharacterController controller;
		private float verticalVelocity;
		private float currentwalkSpeed;
		[field:SerializeField] public Vector3 CurrentMoveDirection { get; private set; }
		[field:SerializeField] public bool Grounded { get; private set; }
		

		public void HandleRotation(float rotationSpeed,Camera cam)
		{
			var camForward = cam.transform.forward;
			camForward.y = 0f;
			if (camForward.sqrMagnitude < 0.001f) return;

			var targetRotation = Quaternion.LookRotation(camForward.normalized);
			controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRotation,
				rotationSpeed * Time.deltaTime);
		}

		public void HandleMovement( CharacterSetup characterSetup,Camera cam)
		{
			var dataCharacter = characterSetup.CharacterData;
			var direction = characterSetup.ReadInput.Direction;
	
			currentwalkSpeed = characterSetup.MoveState.MoveSpeed;

			var camForward = cam.transform.forward; camForward.y = 0f; camForward.Normalize();
			var camRight   = cam.transform.right;   camRight.y   = 0f; camRight.Normalize();

			var moveDir = (camForward * direction.y + camRight * direction.x).normalized;
			var characterDirection = moveDir * currentwalkSpeed;
			
			if (direction.magnitude > 0.1f)
				CurrentMoveDirection = Vector3.MoveTowards(CurrentMoveDirection,
					characterDirection,
					dataCharacter.playerAcceleration * Time.deltaTime);
			else
				CurrentMoveDirection = Vector3.MoveTowards(CurrentMoveDirection,
					Vector3.zero,
					dataCharacter.playerDeceleration * Time.deltaTime);

			var finalMove = CurrentMoveDirection + Vector3.up * verticalVelocity;
			controller.Move(finalMove * Time.deltaTime);
		}
		public void Jump(CharacterSetup characterSetup)
		{
			var isJumping = characterSetup.ReadInput.isJumping;

			if (isJumping)
			{
				Vaulting.TryVault(characterSetup, CurrentMoveDirection.magnitude);
				characterSetup.ReadInput.isJumping = false;

				if (characterSetup.MoveState is VaultState)
					return;
			}

			var jumpForce = characterSetup.CharacterData.jumpForce;
			var gravity = characterSetup.CharacterData.gravity;
			if (controller.isGrounded && verticalVelocity < 0f)
				verticalVelocity = -2f;

			if (isJumping && controller.isGrounded)
			{
				verticalVelocity = jumpForce;
				characterSetup.ReadInput.isJumping = false;
			}

			verticalVelocity += gravity * Time.deltaTime;
		}
		public void Initialize(CharacterSetup setup)
		{
			currentwalkSpeed = setup.CharacterData.playerVelocity;
		}
	}
}