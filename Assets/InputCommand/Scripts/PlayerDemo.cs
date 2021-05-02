using System.Collections.Generic;
using UnityEngine;

namespace InputCommand
{
    /// <summary>
    /// プレイヤーに対して自動でコマンドを流し込む
    /// </summary>
    public class PlayerDemo : MonoBehaviour
    {
        [SerializeField] private DemoData demoData;
        private List<DemoData.CommandInfo> commandInfos;
        private int commandIndex = 0;

        private Player target;
        private Dictionary<int, IPlayerCommand> commandDic = new Dictionary<int, IPlayerCommand>();
        private float waitEndAt;

        private void Start()
        {
            target = GetComponent<Player>();
            commandInfos = demoData.CommandInfos;
        }

		private void Update()
        {
            // 待機中
            if (waitEndAt > Time.time)
            {
                return;
            }

            // コマンドを取り出し
            var info = commandInfos[commandIndex];
            if (info.IsWait)
			{
                waitEndAt = Time.time + info.WaitDuration;
            }
			else
            {
                if (!commandDic.TryGetValue((int)info.Action, out IPlayerCommand command))
                {
                    command = target.GenerateCommand(info.Action);
                    commandDic.Add((int)info.Action, command);
                }
                target.EnqueueCommand(command);
            }
            commandIndex++;
            commandIndex = commandIndex % commandInfos.Count;
        }
	}
}
