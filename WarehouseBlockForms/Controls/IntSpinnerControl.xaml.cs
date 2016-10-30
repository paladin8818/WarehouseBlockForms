using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WarehouseBlockForms.Classess;

namespace WarehouseBlockForms.Controls
{
    /// <summary>
    /// Логика взаимодействия для IntSpinnerControl.xaml
    /// </summary>
    public partial class IntSpinnerControl : UserControl
    {

        private double? OldValue;

        #region Minimum DP
        public double Minimum
        {
            get
            {
                return (double)GetValue(MinimumProperty);
            }
            set
            {
                SetMinimumDP(MinimumProperty, value);
            }
        }
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double),
            typeof(IntSpinnerControl), new FrameworkPropertyMetadata(Double.MinValue,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnMinimumChanged)));

        public event PropertyChangedEventHandler MinimumChanged;
        void SetMinimumDP(DependencyProperty property, double value)
        {
            SetValue(property, value);
            if (MinimumChanged != null)
            {
                MinimumChanged(this, new PropertyChangedEventArgs(value.ToString()));
            }
        }
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IntSpinnerControl spinner = d as IntSpinnerControl;
            if (spinner == null) return;
            spinner.Minimum = (double)e.NewValue;
        }

        #endregion

        #region Maximim DP
        public double Maximum
        {
            get
            {
                return (double)GetValue(MaximumProperty);
            }
            set
            {
                SetMaximumDP(MaximumProperty, value);
            }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double),
            typeof(IntSpinnerControl), new FrameworkPropertyMetadata(Double.MaxValue,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnMaximumChanged)));

        public event PropertyChangedEventHandler MaximumChanged;
        void SetMaximumDP(DependencyProperty property, double value)
        {
            SetValue(property, value);
            if (MaximumChanged != null)
            {
                MaximumChanged(this, new PropertyChangedEventArgs(value.ToString()));
            }
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IntSpinnerControl spinner = d as IntSpinnerControl;
            if (spinner == null) return;
            spinner.Maximum = (double)e.NewValue;
        }
        #endregion

        #region Value DP
        public double? Value
        {
            get
            {
                return (double?)GetValue(ValueProperty);
            }
            set
            {
                if (value != OldValue)
                {
                    double? actualValue = ValidateValue(value);
                    OldValue = actualValue;
                    SetValueDP(ValueProperty, actualValue);
                }
            }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double?),
            typeof(IntSpinnerControl),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnValueChanged)));

        public event PropertyChangedEventHandler ValueChanged;
        void SetValueDP(DependencyProperty property, double? value)
        {
            SetValue(property, value);
            if (ValueChanged != null)
            {
                ValueChanged(this, new PropertyChangedEventArgs(property.ToString()));
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IntSpinnerControl spinner = d as IntSpinnerControl;
            if (spinner == null) return;
            spinner.Value = spinner.ValidateValue((double?)e.NewValue);
        }

        #endregion
        public IntSpinnerControl()
        {
            InitializeComponent();

            (Content as FrameworkElement).DataContext = this;
            btnPlus.Click += delegate
            {
                Value = Value + 0.1;
            };
            btnMinus.Click += delegate
            {
                Value = Value - 0.1;
            };

            tbxField.TextChanged += delegate
            {
                string val = tbxField.Text.Replace('.', ',');
                try
                {
                    Value = Double.Parse(val);
                }
                catch
                {
                    tbxField.Text = OldValue.ToString();
                }
            };
        }

        private double? ValidateValue(double? inputValue)
        {
            double? actualValue = inputValue;

            if (actualValue < Minimum)
            {
                actualValue = Minimum;
            }
            if (actualValue > Maximum)
            {
                actualValue = Maximum;
            }
            setEnabledButtons(actualValue);
            return actualValue;
        }

        private void setEnabledButtons(double? value)
        {
            btnMinus.IsEnabled = true;
            btnPlus.IsEnabled = true;

            if (value == Minimum)
            {
                btnMinus.IsEnabled = false;
            }
            if (value == Maximum)
            {
                btnPlus.IsEnabled = false;
            }
        }

    }
}
