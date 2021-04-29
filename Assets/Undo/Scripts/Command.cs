namespace Undo
{
	/// <summary>
	/// コマンドのインタフェース
	/// </summary>
	public interface ICommand
	{
		void Execute();
		void Undo();

#if UNITY_EDITOR
		string DebugText { get; }
#endif
	}

	/// <summary>
	/// 移動コマンド
	/// </summary>
	public class MoveUnitCommand : ICommand
	{
		private Unit unit;
		private int x, y;
		private int beforeX, beforeY;

#if UNITY_EDITOR
		public string DebugText => $"{unit.name}: ({beforeX},{beforeY}) → ({x},{y})";
#endif

		public MoveUnitCommand(Unit unit, int x, int y)
		{
			this.unit = unit;
			this.x = x;
			this.y = y;
		}

		public void Execute()
		{
			beforeX = unit.X;
			beforeY = unit.Y;
			unit.MoveTo(x, y);
		}

		public void Undo()
		{
			unit.MoveTo(beforeX, beforeY);
		}
	}
}
