using System;

using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace PaymentControl
{
    public class Data : INotifyPropertyChanged
    {
        public int totalPrice { get; set; }


        /// <summary>INotifyPropertyChangedで必要</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>INotifyPropertyChangedで必要</summary>
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
