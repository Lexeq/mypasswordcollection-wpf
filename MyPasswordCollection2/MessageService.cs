using System.Windows;

namespace MPC
{
    class MessageService : IMessageService
    {
        public void ShowMessage(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }
    }
}
