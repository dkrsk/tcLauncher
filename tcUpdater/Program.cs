using System.Diagnostics;

namespace DnKR.tcLauncher.SelfUpdate
{
    public class tcUpdater
    {
        //class FileNotFound : Exception
        //{
        //    public FileNotFound() : base()
        //    {

        //    }
        //}

        static void Main(string[] arguments)
        {
            string new_filename = arguments[0];
            string old_filename = arguments[1];
            int processID = Int32.Parse(arguments[2]);

            try
            {

                Debug.WriteLine(File.Exists(new_filename));
                if (!File.Exists(new_filename) && !File.Exists(old_filename))
                {
                    throw new FileNotFoundException();
                }

                try
                {
                    Process old_process = Process.GetProcessById(processID);
                    old_process.Kill();
                    old_process.WaitForExit();
                }
                catch (ArgumentException)
                {
                    
                }

                File.Delete(old_filename);
                File.Move(new_filename, old_filename);

                Process new_process = Process.Start(old_filename);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}