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

		// Queue = 先入れ先出しリスト
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
					return new JumpCommand();

				case Action.Fire:
					return new FireCommand();
			}
			return null;
		}

		public void Jump()
		{
			var rigidbody = GetComponent<Rigidbody>();
			rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}

		public void Fire()
		{
			var bullet = Instantiate(bulletPrefab);
			bullet.transform.position = transform.position;

			var rigidbody = bullet.GetComponent<Rigidbody>();
			rigidbody.AddForce(Vector3.right * bulletForce, ForceMode.Impulse);
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
