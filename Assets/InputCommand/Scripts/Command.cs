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
		void IPlayerCommand.Execute(Player player)
		{
			player.Jump();
		}
	}

	/// <summary>
	/// 発射コマンド
	/// </summary>
	public class FireCommand : IPlayerCommand
	{
		void IPlayerCommand.Execute(Player player)
		{
			player.Fire();
		}
	}
}
