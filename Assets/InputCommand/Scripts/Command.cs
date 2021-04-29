using UnityEngine;

namespace InputCommand
{
	/// <summary>
	/// プレイヤー用コマンドのインタフェース
	/// </summary>
	public interface IPlayerCommand
	{
		void Execute(Player player);
	}

	/// <summary>
	/// ジャンプ
	/// </summary>
	public class JumpCommand : IPlayerCommand
	{
		private float force;

		public JumpCommand(float force)
		{
			this.force = force;
		}

		void IPlayerCommand.Execute(Player player)
		{
			var rigidbody = player.GetComponent<Rigidbody>();
			rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
		}
	}

	/// <summary>
	/// 発射コマンド
	/// </summary>
	public class FireCommand : IPlayerCommand
	{
		private GameObject bulletPrefab;
		private float force;

		public FireCommand(GameObject bulletPrefab, float force)
		{
			this.bulletPrefab = bulletPrefab;
			this.force = force;
		}

		void IPlayerCommand.Execute(Player player)
		{
			var bullet = Object.Instantiate(bulletPrefab);
			bullet.transform.position = player.transform.position;

			var rigidbody = bullet.GetComponent<Rigidbody>();
			rigidbody.AddForce(Vector3.right * force, ForceMode.Impulse);
		}
	}
}
