using System.Windows;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Views
{
    /// <summary>
    /// Логика взаимодействия для OvenSaveWindow.xaml
    /// </summary>
    public partial class OvenSaveWindow : Window
    {
        private Oven oven = null;
        public OvenSaveWindow(Oven oven = null)
        {
            InitializeComponent();
            this.oven = oven;
            if(oven != null)
            {
                tbxName.Text = oven.Name;
                Title = "Редактирование наименования (" + oven.Name + ")";
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
            if(oven == null)
            {
                oven = new Oven();
            }
            oven.Name = tbxName.Text;
            if(oven.save())
            {
                Close();
            }
            else
            {
                MessageBox.Show("При сохранении наименования произошли ошибки.\nПодробности см. в логе ошибок.");
            }
        }

        private bool validate ()
        {
            if(tbxName.Text == "")
            {
                MessageBox.Show("Введите наименование!");
                return false;
            }
            return true;
        }
    }
}
