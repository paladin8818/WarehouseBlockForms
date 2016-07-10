using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WarehouseBlockForms.Controllers;
using WarehouseBlockForms.Models;
using WarehouseBlockForms.Reports;

namespace WarehouseBlockForms.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AvailabilityPage.xaml
    /// </summary>
    public partial class AvailabilityPage : Page
    {
        public AvailabilityPage()
        {
            InitializeComponent();

            DataContext = new
            {
                Collection = DetailsController.instance().ViewSource.View
            };
            cbxOvenFilter.ItemsSource = OvenController.instance().Collection;

            cbxOvenFilter.SelectionChanged += delegate
            {
                int selectedValue = (cbxOvenFilter.SelectedIndex != -1) ? (int)cbxOvenFilter.SelectedValue : 0;
                DetailsController.instance().showFromOvenId(selectedValue);
            };
            btnClearOvenFilter.Click += delegate
            {
                cbxOvenFilter.SelectedIndex = -1;
            };
            tbxFastFilter.TextChanged += delegate
            {
                DetailsController.instance().showFromText(tbxFastFilter.Text);
            };
            btnClearFastFilter.Click += delegate
            {
                tbxFastFilter.Text = "";
            };

            btnCreateReport.Click += delegate
            {
               /* try
                {*/
                    AvailabilityReport report = new AvailabilityReport();
                    report.Postfix = "сформирован вручную";

                    report.H1 = report.ReportName;
                    report.HeaderRow = new string[] { "№", "Печь", "Артикул", "Наименование", "Количество"};
                    string[][] data = new string[DetailsController.instance().Collection.Count][];
                    for (int i = 0; i < DetailsController.instance().Collection.Count; i++)
                    {
                        Details detail = DetailsController.instance().Collection[i];
                        data[i] = new string[5];
                        data[i][0] = detail.RowIndex.ToString();
                        data[i][1] = detail.OvenName;
                        data[i][2] = detail.VendorCode;
                        data[i][3] = detail.Name;
                        data[i][4] = detail.CurrentCount.ToString();
                    }
                report.Data = data;

                    if (report.Save())
                    {
                    MessageBox.Show("Отчет сохранен!");
                    }
                    else
                    {
                        MessageBox.Show("При формировании отчета произошли ошибки!");
                    }
               /* }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }*/

            };

        }
    }
}
