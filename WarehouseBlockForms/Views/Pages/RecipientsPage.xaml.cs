using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WarehouseBlockForms.Classess;
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
                Collection = RecipientsController.instance().ViewSource.View
            };

            RecipientsController.instance().ViewSource.SortDescriptions.Add(new SortDescription(OrderSettings.RecipientsSortColumn, OrderSettings.RecipientsSortDirection));

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

            btnUp.Click += delegate
            {
                Recipients recipient = dgRecipients.SelectedItem as Recipients;
                if (recipient == null) return;
                up(recipient);
            };

            btnDown.Click += delegate
            {
                Recipients recipient = dgRecipients.SelectedItem as Recipients;
                if (recipient == null) return;
                down(recipient);
            };

        }

        private void rowUp(object sender, RoutedEventArgs e)
        {
            Button upButton = sender as Button;
            if (upButton == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(upButton);
            if (dgrow == null) return;

            Recipients recipient = (Recipients)dgrow.Item;
            if (recipient == null) return;

            if (!recipient.up())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }

        }

        private void rowDown(object sender, RoutedEventArgs e)
        {
            Button downButton = sender as Button;
            if (downButton == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(downButton);
            if (dgrow == null) return;

            Recipients recipient = (Recipients)dgrow.Item;
            if (recipient == null) return;

            if (!recipient.down())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }
        }

        private void up(Recipients recipient)
        {
            if (recipient == null) return;
            if (!recipient.up())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }
        }

        private void down(Recipients recipient)
        {
            if (recipient == null) return;
            if (!recipient.down())
            {
                MessageBox.Show("При выполнении запроса произошли ошибки!\nПодробнее см. в логе ошибок.");
            }
        }

        private void dgRecipients_Sorting(object sender, DataGridSortingEventArgs e)
        {
            IniFile iniFile = new IniFile("settings.ini");
            iniFile.Write("recipients_sort_column", e.Column.SortMemberPath);
            OrderSettings.RecipientsSortColumn = e.Column.SortMemberPath;
            string sortDirection = e.Column.SortDirection.ToString();
            if (string.IsNullOrEmpty(sortDirection) || sortDirection == "Descending")
            {
                iniFile.Write("recipients_sort_direction", "Ascending");
                OrderSettings.RecipientsSortDirection = ListSortDirection.Ascending;
            }
            else
            {
                iniFile.Write("recipients_sort_direction", "Descending");
                OrderSettings.RecipientsSortDirection = ListSortDirection.Descending;
            }
        }
    }
}
