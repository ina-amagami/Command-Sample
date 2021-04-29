using System.Collections.Generic;
using UnityEngine;

namespace InputCommand
{
	public class Player : MonoBehaviour
	{
		public enum Action
		{
			// ジャンプ
			Jump,
			// 弾を発射
			Fire
		}

		[Header("ジャンプ力")]
		[SerializeField] private float jumpForce;
		[Header("弾プレハブ")]
		[SerializeField] private GameObject bulletPrefab;
		[Header("弾の初速")]
		[SerializeField] private float bulletForce;
		[Header("Zキーのアクション")]
		[SerializeField] private Action actionKeyZ = Action.Fire;
		[Header("Xキーのアクション")]
		[SerializeField] private Action actionKeyX = Action.Jump;

		private IPlayerCommand commandKeyZ;
		private IPlayerCommand commandKeyX;
		
		private Queue<IPlayerCommand> commandQueue = new Queue<IPlayerCommand>();

		public void EnqueueCommand(IPlayerCommand command)
		{
			commandQueue.Enqueue(command);
		}

		private void Start()
		{
			commandKeyZ = GenerateCommand(actionKeyZ);
			commandKeyX = GenerateCommand(actionKeyX);
		}

		public IPlayerCommand GenerateCommand(Action action)
		{
			switch (action)
			{
				case Action.Jump:
					return new JumpCommand(jumpForce);

				case Action.Fire:
					return new FireCommand(bulletPrefab, bulletForce);
			}
			return null;
		}

		private void HundleInput()
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{
				EnqueueCommand(commandKeyZ);
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				EnqueueCommand(commandKeyX);
			}
		}

		private void Update()
		{
			HundleInput();
			if (commandQueue.Count == 0)
			{
				return;
			}
			IPlayerCommand command = commandQueue.Dequeue();
			command.Execute(this);
		}
	}
}
