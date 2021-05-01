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
        private IPlayerCommand jumpCommand;
        private IPlayerCommand fireCommand;
        private float waitEndAt;

        private void Start()
        {
            target = GetComponent<Player>();

            jumpCommand = new JumpCommand();
            fireCommand = new FireCommand();

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
                switch (info.Action)
                {
                    case Player.Action.Jump:
                        target.EnqueueCommand(jumpCommand);
                        break;

                    case Player.Action.Fire:
                        target.EnqueueCommand(fireCommand);
                        break;
                }
            }
            commandIndex++;
            commandIndex = commandIndex % commandInfos.Count;
        }
	}
}
