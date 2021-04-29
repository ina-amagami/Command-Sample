using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Undo
{
    public class GameManager : MonoBehaviour
	{
		[SerializeField] private Button undoButton;
		[SerializeField] private Button redoButton;
		[SerializeField] private RectTransform directionUIRoot;

		// 実行済のコマンド一覧（Undoしか使わないならStackとの相性が良い）
		private List<ICommand> commands = new List<ICommand>();
		private int commandIndex = -1;

#if UNITY_EDITOR
		public List<ICommand> Commands => commands;
		public int CommandIndex => commandIndex;
#endif

		private Unit targetUnit;

		private void Start()
		{
			RefleshUndoRedoButton();
			directionUIRoot.gameObject.SetActive(false);
		}

		private void Update()
		{
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (!Physics.Raycast(ray, out RaycastHit hit))
			{
				return;
			}
			if (!hit.transform.TryGetComponent(out Unit unit))
			{
				return;
			}
			targetUnit = unit;
			directionUIRoot.gameObject.SetActive(true);
			directionUIRoot.position = RectTransformUtility.WorldToScreenPoint(Camera.main, unit.transform.position);
		}

		#region ボタン操作

		private bool IsEnableUndo => commandIndex >= 0;
		private bool IsEnableRedo => commandIndex < commands.Count - 1;

		// Undo
		public void OnClickUndo()
		{
			if (!IsEnableUndo)
			{
				return;
			}
			var command = commands[commandIndex];
			command.Undo();
			commandIndex--;

			RefleshUndoRedoButton();
			directionUIRoot.gameObject.SetActive(false);
		}

		// Redo
		public void OnClickRedo()
		{
			if (!IsEnableRedo)
			{
				return;
			}
			commandIndex++;
			var command = commands[commandIndex];
			command.Execute();

			RefleshUndoRedoButton();
			directionUIRoot.gameObject.SetActive(false);
		}

		private void RefleshUndoRedoButton()
		{
			undoButton.gameObject.SetActive(IsEnableUndo);
			redoButton.gameObject.SetActive(IsEnableRedo);
		}

		// 右
		public void OnClickRight()
		{
			if (!targetUnit) { return; }
			int x = targetUnit.X + 1;
			int y = targetUnit.Y;
			ExecuteMoveCommand(x, y);
		}

		// 下
		public void OnClickDown()
		{
			if (!targetUnit) { return; }
			int x = targetUnit.X;
			int y = targetUnit.Y - 1;
			ExecuteMoveCommand(x, y);
		}

		// 左
		public void OnClickLeft()
		{
			if (!targetUnit) { return; }
			int x = targetUnit.X - 1;
			int y = targetUnit.Y;
			ExecuteMoveCommand(x, y);
		}

		// 上
		public void OnClickUp()
		{
			if (!targetUnit) { return; }
			int x = targetUnit.X;
			int y = targetUnit.Y + 1;
			ExecuteMoveCommand(x, y);
		}

		private void ExecuteMoveCommand(int x, int y)
		{
			var moveCommand = new MoveUnitCommand(targetUnit, x, y);
			moveCommand.Execute();

			// Undoで戻したコマンドがあればリストから取り除いておく
			while (commandIndex < commands.Count - 1)
			{
				commands.RemoveAt(commands.Count - 1);
			}

			commands.Add(moveCommand);
			commandIndex++;

			// 再度ターゲット選択から
			targetUnit = null;
			directionUIRoot.gameObject.SetActive(false);

			RefleshUndoRedoButton();
		}

		#endregion
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(GameManager))]
	public class GameManagerEditor : Editor
	{
		public void OnEnable()
		{
			EditorApplication.update += Repaint;
		}

		public void OnDisable()
		{
			EditorApplication.update -= Repaint;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (!Application.isPlaying)
			{
				return;
			}

			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("--- 実行コマンドリスト ---");

			var gameManager = target as GameManager;
			var commands = gameManager.Commands;
			var commandIndex = gameManager.CommandIndex;

			for (int i = 0; i < commands.Count; ++i)
			{
				Color colorCache = GUI.color;
				if (i == commandIndex)
				{
					GUI.color = Color.white;
				}
				EditorGUILayout.LabelField($"{i.ToString()}. {commands[i].DebugText}");
				GUI.color = colorCache;
			}
		}
	}
#endif
}
