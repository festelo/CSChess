using System.Windows;

namespace CSChess
{
    static public class Messages
    {
        static public class Errors {
            static string header = "Ошибка";
            static public void ConnectError(string message) { MessageBox.Show("Ошибка подключения.\nПодробнее: " + message, header); }
            static public void RegisterError(string message) { MessageBox.Show("Ошибка регистрации.\nПодробнее: " + message, header); }
            static public void LoginError(string message) { MessageBox.Show("Ошибка входа.\nПодробнее: " + message, header); }
        }
    }
}
