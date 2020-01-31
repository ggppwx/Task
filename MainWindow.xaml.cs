using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace Task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static System.Timers.Timer timer;

        public MainWindow()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("hello");
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("starting monitor.....");
            
            SetTimer(() => {
                
                IntPtr handle = GetForegroundWindow();
                if (handle != null)
                {
                    int activeProcessId;
                    GetWindowThreadProcessId(handle, out activeProcessId);

                    Process activeProcess = Process.GetProcessById(activeProcessId);
                    // System.Diagnostics.Debug.WriteLine(activeProcess.ProcessName);
                    try
                    {
                        var moduleName = activeProcess.MainModule.ModuleName;
                        this.Dispatcher.Invoke(() =>
                        {
                            outputTxtBlock.Text = $"Monitoring {moduleName} ...";
                        });
                    }
                    catch (Win32Exception)
                    {
                        var processName = activeProcess.ProcessName;
                        this.Dispatcher.Invoke(() =>
                        {
                            outputTxtBlock.Text = $"Monitoring {processName} ...";
                        });
                    }

                    // save the the (process + time ) pair



                }

               
            });


        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Stopping monitor.....");
            timer.Stop();
        }


        private static void SetTimer(Action act)
        {
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += (source,  args) => act();
            timer.AutoReset = true;
            timer.Start();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
    }
}
