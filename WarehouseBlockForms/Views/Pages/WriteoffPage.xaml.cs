using System;
using System.Windows;
using System.Windows.Controls;
using WarehouseBlockForms.Classess;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Helpers;
using WarehouseBlockForms.Models;
using Xceed.Wpf.Toolkit;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для WriteoffPage.xaml
    /// </summary>
    public partial class WriteoffPage : Page
    {
        WriteoffDetailsHelperCollection wodh_collection = WriteoffDetailsHelperCollection.instance();

        public WriteoffPage()
        {
            InitializeComponent();

            DataContext = new
            {
                Collection = wodh_collection.Collection
            };
            cbxRecipients.ItemsSource = RecipientsController.instance().Collection;
            btnAddRow.Click += delegate
            {
                WriteoffDetailsHelper writeoffDetails = new WriteoffDetailsHelper();
                wodh_collection.add(writeoffDetails);
            };

            btnSaveWriteoff.Click += delegate
            {
                saveWriteoff();
            };

        }

        private void ovenSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;
            if (comboBox.SelectedIndex == -1) return;
            if (comboBox.SelectedValue == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(comboBox);
            if (dgrow == null) return;

            WriteoffDetailsHelper writeoffDetailHelper = (WriteoffDetailsHelper)dgrow.Item;
            if (writeoffDetailHelper == null) return;

            writeoffDetailHelper.IdOven = (int)comboBox.SelectedValue;
            writeoffDetailHelper.IdDetails = 0;
        }

        private void detailsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;
            if (comboBox.SelectedIndex == -1) return;
            if (comboBox.SelectedValue == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(comboBox);
            if (dgrow == null) return;

            WriteoffDetailsHelper writeoffDetailHelper = (WriteoffDetailsHelper)dgrow.Item;
            if (writeoffDetailHelper == null) return;

            writeoffDetailHelper.IdDetails = (int)comboBox.SelectedValue;
        }

        private void removeRecord(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(button);
            if (dgrow == null) return;

            WriteoffDetailsHelper writeoffDetailHelper = (WriteoffDetailsHelper)dgrow.Item;
            if (writeoffDetailHelper == null) return;

            wodh_collection.remove(writeoffDetailHelper);
        }

        private void saveWriteoff()
        {
            if(cbxRecipients.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Выберите получателя!");
                return;
            }
            if(tbxAppNumber.Text == "")
            {
                System.Windows.MessageBox.Show("Введите номер заявки!");
                return;
            }
            if (wodh_collection.Collection.Count == 0)
            {
                System.Windows.MessageBox.Show("Невозможно сохранить пустое списание!");
                return;
            }
            if (System.Windows.MessageBox.Show("Подтвердите списание", "Подтвердите списание", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            if (!wodh_collection.isAllCorrect())
            {
                System.Windows.MessageBox.Show("Проверьте введенные данные!\nОдна или более строк заполнены некорректно!");
                return;
            }

            Writeoff writeoff = new Writeoff();
            writeoff.WriteoffDate = DateTime.Now;
            writeoff.AppNumber = tbxAppNumber.Text;
            writeoff.IdRecipient = (int)cbxRecipients.SelectedValue;

            if (writeoff.save())
            {
                for (int i = 0; i < wodh_collection.Collection.Count; i++)
                {
                    WriteoffDetailsHelper wodh = wodh_collection.Collection[i];
                    WriteoffDetails writeoffDetails = new WriteoffDetails();
                    writeoffDetails.IdDetails = wodh.IdDetails;
                    writeoffDetails.IdWriteoff = writeoff.Id;
                    writeoffDetails.DetailsCount = wodh.DetailsCount;
                    if (!writeoffDetails.save())
                    {
                        System.Windows.MessageBox.Show("При сохранении элементов списания произошли ошибки.");
                        writeoff.remove();
                        return;
                    }
                }
                wodh_collection.clear();
            }
            else
            {
                System.Windows.MessageBox.Show("При сохранении списания произошли ошибки.");
                return;
            }

        }

        private void countValueChange(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DecimalUpDown updown = sender as DecimalUpDown;
            if (updown == null) return;

            DataGridRow dgrow = FindItem.FindParentItem<DataGridRow>(updown);
            if (dgrow == null) return;

            WriteoffDetailsHelper writeoffDetailHelper = (WriteoffDetailsHelper)dgrow.Item;
            if (writeoffDetailHelper == null) return;

            writeoffDetailHelper.DetailsCount = (int)updown.Value;
        }

    }
}
