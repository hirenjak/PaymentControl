using System;

using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace PaymentControl
{
    /// <summary> それぞれのアイテムを保存しておくためのクラス </summary>
    public class ItemsValue : INotifyPropertyChanged
    {
        public BitmapImage picture { set; get; }
        public long ID { get; set; }
        public string status { get; set; }
        public string priority { get; set; }
        public string name { get; set; }
        public string kind { get; set; }
        public int price { get; set; }
        public string system { get; set; }
        public string paymentPage { get; set; }
        public string originalPaget { get; set; }
        public string supplemental { get; set; }

        /// <summary>画像読み込みに必要なのでIDのみ引数での入力</summary>
        /// <param name="ID"></param>
        public ItemsValue(long ID)
        {
            try
            {
                this.ID = ID;
                picture = new BitmapImage();
                picture.BeginInit();
                picture.UriSource = new Uri(System.IO.Path.GetFullPath(".\\cache\\picture\\" + ID));
                picture.DecodePixelWidth = 200;
                picture.EndInit();
            }
            catch
            {

            }
        }
        

        /// <summary>INotifyPropertyChangedで必要</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>INotifyPropertyChangedで必要</summary>
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
