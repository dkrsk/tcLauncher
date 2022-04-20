using System.Collections.Concurrent;

namespace DnKR.tcLauncher
{
    public partial class GameLog : Form
    {
        public GameLog()
        {
            InitializeComponent();
        }

        static ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();

        public static void AddLog(string msg)
        {
            logQueue.Enqueue(msg);
        }

        private void timerLog_Tick(object sender, EventArgs e)
        {
            string msg;
            while (logQueue.TryDequeue(out msg))
            {
                rtbLog.AppendText(msg + "\n");
                rtbLog.ScrollToCaret();
            }
        }
    }
}
