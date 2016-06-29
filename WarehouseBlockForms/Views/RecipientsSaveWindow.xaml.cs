using System.Windows;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Views
{
    /// <summary>
    /// Логика взаимодействия для RecipientsSaveWindow.xaml
    /// </summary>
    public partial class RecipientsSaveWindow : Window
    {
        private Recipients recipient;
        public RecipientsSaveWindow(Recipients recipient = null)
        {
            InitializeComponent();
            this.recipient = recipient;
            if(recipient != null)
            {
                tbxFullName.Text = recipient.FullName;
                Title = "Редактирование получателя (" + recipient.FullName + ")";
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

        private void save()
        {
            if (!validate()) return;
            if (recipient == null)
            {
                recipient = new Recipients();
            }
            recipient.FullName = tbxFullName.Text;
            if (recipient.save())
            {
                Close();
            }
            else
            {
                MessageBox.Show("При сохранении получателя произошли ошибки.\nПодробности см. в логе ошибок.");
            }
        }

        private bool validate()
        {
            if (tbxFullName.Text == "")
            {
                MessageBox.Show("Введите ФИО получателя!");
                return false;
            }
            return true;
        }

    }
}
