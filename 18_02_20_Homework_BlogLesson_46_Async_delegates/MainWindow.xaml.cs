using System;
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

namespace _18_02_20_Homework_BlogLesson_46_Async_delegates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate T delegateThatReturnsSomething<T>();
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            Func<double> forPi = () => { return Math.PI; };
            btnPiButton.Click += (object sender, RoutedEventArgs e) => 
            {
                 IAsyncResult iAsyncRez =  forPi.BeginInvoke(AsiuncCallBack_AfterDelegateCompleted, forPi);
            };



            delegateThatReturnsSomething<string> forString = () => { return "AAA"; };
            forString += () => { return "BBB"; };
            forString += () => { return "CCC"; };

            btnStringButton.Click += (object sender, RoutedEventArgs e) => 
            {
                string endRez = string.Empty;
                foreach(var s in forString.GetInvocationList())
                {
                    IAsyncResult rez = (s as delegateThatReturnsSomething<string>).BeginInvoke(null, s as delegateThatReturnsSomething<string>);
                    endRez += (s as delegateThatReturnsSomething<string>).EndInvoke(rez) + Environment.NewLine;
                }
                this.Dispatcher.Invoke(() =>
                {
                    (sender as Button).Content = endRez;
                });
            };
        
        }



        private void AsiuncCallBack_AfterDelegateCompleted(IAsyncResult iAsresult)
        {
            Func<double> localFuncDel = iAsresult.AsyncState as Func<double>;
            var finalResult = localFuncDel.EndInvoke(iAsresult);
            this.Dispatcher.Invoke(() => 
            {
                btnPiButton.Content = finalResult.ToString();
            });


        }
    }
}
