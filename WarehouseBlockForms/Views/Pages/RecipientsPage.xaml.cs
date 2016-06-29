using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для RecipientsPage.xaml
    /// </summary>
    public partial class RecipientsPage : Page
    {
        public RecipientsPage()
        {
            InitializeComponent();
            DataContext = new
            {
                Collection = RecipientsController.instance().Collection
            };

            btnAdd.Click += delegate
            {
                RecipientsSaveWindow recipientsSaveWindow = new RecipientsSaveWindow();
                recipientsSaveWindow.ShowDialog();
            };
            btnEdit.Click += delegate
            {
                Recipients selectedRecipient = (Recipients)dgRecipients.SelectedItem;
                if (selectedRecipient == null)
                {
                    MessageBox.Show("Выберите редактируемую запись!");
                    return;
                }
                RecipientsSaveWindow recipientsSaveWindow = new RecipientsSaveWindow(selectedRecipient);
                recipientsSaveWindow.ShowDialog();
            };
            btnDelete.Click += delegate
            {
                Recipients selectedRecipient = (Recipients)dgRecipients.SelectedItem;
                if (selectedRecipient == null)
                {
                    MessageBox.Show("Выберите удаляемую запись!");
                    return;
                }
                if (MessageBox.Show("Подтвердите удаление!", "Подтвердите удаление!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    selectedRecipient.remove();
                }
            };
        }
    }
}
