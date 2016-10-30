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
using System.Windows.Shapes;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Views
{
    /// <summary>
    /// Логика взаимодействия для DetailsSaveWindow.xaml
    /// </summary>
    public partial class DetailsSaveWindow : Window
    {
        private Details details;
        public DetailsSaveWindow(Details details = null)
        {
            InitializeComponent();
            this.details = details;

            cbxOven.ItemsSource = OvenController.instance().Collection;

            if(details != null)
            {
                tbxName.Text = details.Name;
                cbxOven.SelectedValue = details.IdOven;

                Title = "Редактирование детали (" + details.Name + ")";
            }

            btnCancel.Click += delegate
            {
                Close();
            };

            btnSave.Click += delegate
            {
                save();
            };

        }

        private void save ()
        {
            if (!validate()) return;
            if (details == null)
            {
                details = new Details();
            }
            details.Name = tbxName.Text;
            details.IdOven = (int)cbxOven.SelectedValue;
            if(details.save())
            {
                Close();
            }
            else
            {
                MessageBox.Show("При сохранении детали произошли ошибки.\nПодробности см. в логе ошибок.");
            }
        }

        private bool validate()
        {
            if (tbxName.Text == "")
            {
                MessageBox.Show("Введите название детали!");
                return false;
            }
            if(cbxOven.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите печь!");
                return false;
            }
            return true;
        }

    }
}
