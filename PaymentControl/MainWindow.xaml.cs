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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;

using Microsoft.Win32;

namespace PaymentControl
{
    /// <summary>MainWindow.xaml の相互作用ロジック</summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ItemsValue> lists;
        public ObservableCollection<ItemsValue> renderLists;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                lists = ListLoad();
            }
            catch
            {
                lists = new ObservableCollection<ItemsValue>();
            }

            renderLists = Function.CopyList(lists);
            listView.DataContext = renderLists;
            
            BoxItemsSetting();
        }

        /// <summary>リストをファイルから読み込む</summary>
        /// <returns>読み込んだリストデータ</returns>
        private ObservableCollection<ItemsValue> ListLoad()
        {
            ObservableCollection<ItemsValue> results = new ObservableCollection<ItemsValue>();
            if (File.Exists(@"data.bin"))
            {
                // data.binというファイルからアイテムのデータを読み込む
                using (FileStream fStream = new FileStream(@"data.bin", FileMode.Open))
                using (BinaryReader bReader = new BinaryReader(fStream))
                {
                    int NUM = bReader.ReadInt32();
                    for (int ID = 0; ID<NUM; ID++)
                    {
                        results.Add(new ItemsValue(bReader.ReadInt64())
                        {
                            status = bReader.ReadString(),
                            priority = bReader.ReadString(),
                            name = bReader.ReadString(),
                            kind = bReader.ReadString(),
                            price = bReader.ReadInt32(),
                            system = bReader.ReadString(),
                            paymentPage = bReader.ReadString(),
                            originalPaget = bReader.ReadString(),
                            supplemental = bReader.ReadString()
                        });
                    }
                }
            }
            else
            {
                // ファイルが存在しない場合は作成のみ行う
                File.Create(@"data.bin");
            }

            return results;
        }


        /// <summary>各コンボボックスの項目を設定</summary>
        private void BoxItemsSetting()
        {
            kindBox.Items.Add("道具");
            kindBox.Items.Add("VR機器");
            kindBox.Items.Add("テーブルゲーム");
            kindBox.Items.Add("PC周辺機器");
            kindBox.Items.Add("アダルト");

            priorityBox.Items.Add("最高");
            priorityBox.Items.Add("高");
            priorityBox.Items.Add("中");
            priorityBox.Items.Add("低");
            priorityBox.Items.Add("最低");

            statusBox.Items.Add("検討中");
            statusBox.Items.Add("購入予定");
            statusBox.Items.Add("購入済み");

            systemBox.Items.Add("買切り");
            systemBox.Items.Add("月額");

            kindSelectBox.Items.Add("");
            kindSelectBox.Items.Add("道具");
            kindSelectBox.Items.Add("VR機器");
            kindSelectBox.Items.Add("テーブルゲーム");
            kindSelectBox.Items.Add("PC周辺機器");
            kindSelectBox.Items.Add("アダルト");

            prioritySelectBox.Items.Add("");
            prioritySelectBox.Items.Add("最高");
            prioritySelectBox.Items.Add("高");
            prioritySelectBox.Items.Add("中");
            prioritySelectBox.Items.Add("低");
            prioritySelectBox.Items.Add("最低");

            statusSelectBox.Items.Add("");
            statusSelectBox.Items.Add("検討中");
            statusSelectBox.Items.Add("購入予定");
            statusSelectBox.Items.Add("購入済み");

            systemSelectBox.Items.Add("");
            systemSelectBox.Items.Add("買切り");
            systemSelectBox.Items.Add("月額");
        }

        /// <summary>入力したデータをリストに適用</summary>
        private void button_Apply_Click(object sender, RoutedEventArgs e)
        {
            long tempID;

            // IDが設定されていればその値を，されていなければ新たに現在Tickから設定
            if (pictureAddressBox.Text == "")
            {
                tempID = DateTime.Now.Ticks;
            }
            else
            {
                tempID = long.Parse(pictureAddressBox.Text);
            }

            // 現在扱っているデータとリストをIDにより照合しあれば書き換える
            foreach (var value in lists)
            {
                if(tempID == value.ID)
                {
                    value.name = nameBox.Text;
                    value.kind = kindBox.Text;
                    value.price = int.Parse(priceBox.Text);
                    value.status = statusBox.Text;
                    value.originalPaget = originPageBox.Text;
                    value.paymentPage = paymentPageBox.Text;
                    value.priority = priorityBox.Text;
                    value.supplemental = supplementalBox.Text;
                    value.system = systemBox.Text;

                    listView.Items.Refresh();
                    return;
                }
            }

            Console.WriteLine(listView.Height.ToString());

            // データ照合が無い場合に追加を行う
            lists.Add(new ItemsValue(tempID) { name = nameBox.Text, kind = kindBox.Text, price = int.Parse(priceBox.Text), originalPaget = originPageBox.Text, paymentPage = paymentPageBox.Text, priority = priorityBox.Text, status = statusBox.Text, supplemental = supplementalBox.Text, system = systemBox.Text });
        }

        /// <summary>ファイルとしてリストを出力</summary>
        private void button_Save_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fStream = new FileStream(@"data.bin", FileMode.Create))
            using (BinaryWriter bWriter = new BinaryWriter(fStream))
            {
                bWriter.Write(lists.Count);
                foreach (var value in lists)
                {
                    bWriter.Write(value.ID);
                    bWriter.Write(value.status);
                    bWriter.Write(value.priority);
                    bWriter.Write(value.name);
                    bWriter.Write(value.kind);
                    bWriter.Write(value.price);
                    bWriter.Write(value.system);
                    bWriter.Write(value.paymentPage);
                    bWriter.Write(value.originalPaget);
                    bWriter.Write(value.supplemental);
                }
            }
        }

        /// <summary>ファイル入力の際にサムネイルとする画像を選択</summary>
        private void button_PictSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "ファイルを開く";
            dialog.Filter = "画像ファイル|*.png;*.jpg;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                long ID = DateTime.Now.Ticks;
                if (!Directory.Exists(".\\cache\\picture")) { Directory.CreateDirectory(".\\cache\\picture"); }
                File.Copy(dialog.FileName, ".\\cache\\picture\\" + ID);
                pictureAddressBox.Text = ID.ToString();
            }
        }

        /// <summary>選択アイテム編集</summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            long tempID = (listView.SelectedItem as ItemsValue).ID;

            foreach(var value in lists)
            {
                if(value.ID == tempID)
                {
                    pictureAddressBox.Text = value.ID.ToString();
                    nameBox.Text = value.name;
                    kindBox.Text = value.kind;
                    priceBox.Text = value.price.ToString();
                    statusBox.Text = value.status;
                    originPageBox.Text = value.originalPaget;
                    paymentPageBox.Text = value.paymentPage;
                    priorityBox.Text = value.priority;
                    supplementalBox.Text = value.supplemental;
                    systemBox.Text = value.system;
                    return;
                }
            }
        }

        private void kindSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListExtractions();
        }

        private void prioritySelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListExtractions();
        }

        private void statusSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListExtractions();
        }

        private void systemSelectBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListExtractions();
        }

        private void ListExtractions()
        {
            List<ItemsValue> targetID = new List<ItemsValue>();
            renderLists = Function.CopyList(lists);
            
            if ((string)kindSelectBox.SelectedValue != "")
            {
                foreach (var value in renderLists)
                {
                    if (value.kind != (string)kindSelectBox.SelectedValue)
                    {
                        targetID.Add(value);
                    }
                }
            }

            for (int ID = 0; ID < targetID.Count; ID++)
            {
                renderLists.Remove(targetID[ID]);
            }


            targetID = new List<ItemsValue>();
            

            if ((string)prioritySelectBox.SelectedValue != "")
            {
                foreach (var value in renderLists)
                {
                    if (value.priority != (string)prioritySelectBox.SelectedValue)
                    {
                        targetID.Add(value);
                    }
                }
            }

            for (int ID = 0; ID < targetID.Count; ID++)
            {
                renderLists.Remove(targetID[ID]);
            }

            targetID = new List<ItemsValue>();

            if ((string)statusSelectBox.SelectedValue != "")
            {
                foreach (var value in renderLists)
                {
                    if (value.status != (string)statusSelectBox.SelectedValue)
                    {
                        targetID.Add(value);
                    }
                }
            }

            for (int ID = 0; ID < targetID.Count; ID++)
            {
                renderLists.Remove(targetID[ID]);
            }


            targetID = new List<ItemsValue>();

            if ((string)systemSelectBox.SelectedValue != "")
            {
                foreach (var value in renderLists)
                {
                    if (value.system != (string)systemSelectBox.SelectedValue)
                    {
                        targetID.Add(value);
                    }
                }
            }

            for (int ID = 0; ID < targetID.Count; ID++)
            {
                renderLists.Remove(targetID[ID]);
            }

            listView.ItemsSource = renderLists;
            listView.Items.Refresh();
        }
        
    }
}