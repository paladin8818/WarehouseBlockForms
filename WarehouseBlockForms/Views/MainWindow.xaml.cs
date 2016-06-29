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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WarehouseBlockForms.Classes;
using WarehouseBlockForms.Models;

namespace WarehouseBlockForms.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool administratorMode = false;
        public MainWindow(bool administratorMode = false)
        {
            InitializeComponent();
            this.administratorMode = administratorMode;

            if(DataBase.Connect() == null)
            {
                MessageBox.Show("Ошибка открытия базы данных.\nРабота приложения будет остановлена.");
                Close();
                return;
            }

            setWorkPages();

            if (this.administratorMode)
            {
                Title += " (режим администрирования)";

                tiSettings.Visibility = Visibility.Visible;
                setSettingsPages();
            }
        }

        private void setWorkPages()
        {
            Uri supplyPageUri = new Uri("/Views/Pages/SupplyPage.xaml", UriKind.Relative);
            frameSupply.Source = supplyPageUri;
        }

        private void setSettingsPages ()
        {
            Uri ovenPageUri = new Uri("/Views/Pages/OvenPage.xaml", UriKind.Relative);
            frameSettingsOven.Source = ovenPageUri;

            Uri detailsPageUri = new Uri("/Views/Pages/DetailsPage.xaml", UriKind.Relative);
            frameSettingsDetails.Source = detailsPageUri;

            Uri recipientsPageUri = new Uri("/Views/Pages/RecipientsPage.xaml", UriKind.Relative);
            frameSettingsRecipients.Source = recipientsPageUri;

        }
    }
}
