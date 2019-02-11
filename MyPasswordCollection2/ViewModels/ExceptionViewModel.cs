using System;

namespace MPC.ViewModels
{
    class ExceptionViewModel : BaseViewModel
    {
        public string Message { get; set; }

        public string Type { get; set; }

        public string Details { get; set; }

        public ExceptionViewModel(Exception exception)
        {
            Message = exception.Message;
            Type = exception.GetType().Name;
            Details = exception.ToString();
        }
    }
}
