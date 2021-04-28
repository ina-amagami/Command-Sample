using UnityEngine;

namespace InputCommand
{
	/// <summary>
	/// プレイヤー用コマンドの抽象クラス
	/// </summary>
	public abstract class PlayerCommand
	{
		public abstract void Execute(Player player);
	}

	/// <summary>
	/// ジャンプ
	/// </summary>
	public class JumpCommand : PlayerCommand
	{
		private float force;

		public JumpCommand(float force)
		{
			this.force = force;
		}

		public override void Execute(Player player)
		{
			var rigidbody = player.GetComponent<Rigidbody>();
			rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
		}
	}

	/// <summary>
	/// 発射コマンド
	/// </summary>
	public class FireCommand : PlayerCommand
	{
		private GameObject bulletPrefab;
		private float force;

		public FireCommand(GameObject bulletPrefab, float force)
		{
			this.bulletPrefab = bulletPrefab;
			this.force = force;
		}

		public override void Execute(Player player)
		{
			var bullet = Object.Instantiate(bulletPrefab);
			bullet.transform.position = player.transform.position;

			var rigidbody = bullet.GetComponent<Rigidbody>();
			rigidbody.AddForce(Vector3.right * force, ForceMode.Impulse);
		}
	}
}
