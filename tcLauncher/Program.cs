

namespace DnKR
{
    namespace tcLauncher
    {
        internal static class Program
        {
            /// <summary>
            /// ������� ����� ����� ��� ����������.
            /// </summary>
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
