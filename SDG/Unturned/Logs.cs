using System;
using System.IO;
using SDG.Framework.Debug;
using UnityEngine;

namespace SDG.Unturned
{
	public class Logs : MonoBehaviour
	{
		private void onMessageAdded(TerminalLogMessage message, TerminalLogCategory category)
		{
			if (this.stream == null || this.writer == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(message.internalText))
			{
				return;
			}
			DateTime dateTime = new DateTime(message.timestamp);
			string text = dateTime.ToString("HH:mm:ss - dd/MM/yyyy");
			this.writer.WriteLine(string.Concat(new string[]
			{
				"[",
				text,
				"] ",
				category.internalName,
				": ",
				message.internalText
			}));
		}

		private void onLogMessageReceived(string text, string stack, LogType type)
		{
			if (this.stream == null || this.writer == null)
			{
				return;
			}
			switch (type)
			{
			case 0:
			case 1:
			case 4:
				if (!Dedicator.isDedicated)
				{
					Terminal.print(null, text, "Exceptions", "<color=" + Terminal.failColor + ">Exceptions</color>", false);
				}
				this.writer.WriteLine();
				this.writer.WriteLine(text);
				this.writer.WriteLine(stack);
				return;
			case 3:
				Terminal.print(text, null, "General", "General", true);
				return;
			}
		}

		public void awake()
		{
			if (string.IsNullOrEmpty(this.path))
			{
				this.path = ReadWrite.PATH;
				if (Dedicator.isDedicated)
				{
					this.path = this.path + "/Logs/Server_" + Dedicator.serverID + ".log";
				}
				else
				{
					this.path += "/Logs/Client.log";
				}
				if (!Directory.Exists(Path.GetDirectoryName(this.path)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(this.path));
				}
				if (File.Exists(this.path))
				{
					string text = ReadWrite.PATH;
					if (Dedicator.isDedicated)
					{
						text = text + "/Logs/Server_" + Dedicator.serverID + "_Prev.log";
					}
					else
					{
						text += "/Logs/Client_Prev.log";
					}
					File.Copy(this.path, text, true);
				}
			}
			if (this.stream == null)
			{
				this.stream = new FileStream(this.path, FileMode.Create, FileAccess.Write, FileShare.Write);
			}
			if (this.writer == null)
			{
				this.writer = new StreamWriter(this.stream);
				this.writer.AutoFlush = true;
			}
			Terminal.onMessageAdded += this.onMessageAdded;
			Application.logMessageReceived += new Application.LogCallback(this.onLogMessageReceived);
		}

		private void OnDestroy()
		{
			if (this.writer != null)
			{
				this.writer.Flush();
				this.writer.Close();
				this.writer.Dispose();
				this.writer = null;
			}
			if (this.stream != null)
			{
				this.stream.Close();
				this.stream.Dispose();
				this.stream = null;
			}
		}

		private string path;

		private FileStream stream;

		private StreamWriter writer;
	}
}
