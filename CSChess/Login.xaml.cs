using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ChessLibrary;

namespace CSChess
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Internet Internet = new ChessLibrary.Internet();
        public Login()
        {
            InitializeComponent();
        }

        void Connect()
        {
            try
            {
                Internet.Connect(CSChess.Properties.Resources.IP);
            }
            catch(Exception err)
            {
                MessageBox.Show("Ошибка подключения.\nПодробнее: " + err.Message, "Ошибка");
            }
        }

        private void DisableBtns()
        {
            LoginBtn.IsEnabled = false;
            RegisterBtn.IsEnabled = false;
            OfflineBtn.IsEnabled = false;
        }

        private void EnableBtns()
        {
            LoginBtn.IsEnabled = true;
            RegisterBtn.IsEnabled = true;
            OfflineBtn.IsEnabled = true;
        }

        private void OpenGameWindow()
        {
            Close();
            //Новое окно
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableBtns();
            bool res = await LoginMain(LoginText.Text, PasswordText.Text);
            EnableBtns();
            if (res) OpenGameWindow();
        }

        Task<bool> LoginMain(string login, string pass)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (!Internet.IsConnected) Internet.Connect(CSChess.Properties.Resources.IP);
                    Internet.Login(login, pass);
                    return true;
                }
                catch (ChessLibrary.Internet.Exceptions.UserLoginError err)
                {
                    Messages.Errors.LoginError(err.Message);
                    return false;
                }
                catch (System.Net.Sockets.SocketException err)
                {
                    Messages.Errors.ConnectError(err.Message);
                    return false;
                }
            });
        }

        private async void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableBtns();
            bool res = await RegisterMain(LoginText.Text, PasswordText.Text);
            EnableBtns();
            if (res) OpenGameWindow();
        }
        Task<bool> RegisterMain(string login, string pass)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (!Internet.IsConnected) Internet.Connect(CSChess.Properties.Resources.IP);
                    Internet.Register(login, pass);
                    return true;
                }
                catch (ChessLibrary.Internet.Exceptions.UserAlreadyRegistred err)
                {
                    Messages.Errors.RegisterError(err.Message);
                    return false;
                }
                catch (System.Net.Sockets.SocketException err)
                {
                    Messages.Errors.ConnectError(err.Message);
                    return false;
                }
            });
        }
    }
}
