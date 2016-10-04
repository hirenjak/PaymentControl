using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace PaymentControl
{
    public static class Function
    {
        public static ObservableCollection<ItemsValue> CopyList(ObservableCollection<ItemsValue> source)
        {
            ObservableCollection<ItemsValue> results = new ObservableCollection<ItemsValue>();

            foreach(var value in source)
            {
                results.Add(new ItemsValue(value.ID)
                {
                    status = value.status,
                    priority = value.priority,
                    name = value.name,
                    kind = value.kind,
                    price = value.price,
                    system = value.system,
                    paymentPage = value.paymentPage,
                    originalPaget = value.originalPaget,
                    supplemental = value.supplemental
                });
            }

            return results;
        }


        public static void CBoxAdd(ComboBox targetBox, string path)
        {
            using (StreamReader sReader = new StreamReader(path))
            {
                string readValue = "";
                while ((readValue = sReader.ReadLine()) != null)
                {
                    targetBox.Items.Add(readValue);
                }
            }
        }
    }


}
