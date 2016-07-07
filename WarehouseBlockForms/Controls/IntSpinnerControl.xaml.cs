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
using WarehouseBlockForms.Classes;

namespace WarehouseBlockForms.Controls
{
    /// <summary>
    /// Логика взаимодействия для IntSpinnerControl.xaml
    /// </summary>
    public partial class IntSpinnerControl : UserControl
    {

        private int? OldValue;

        #region Minimum DP
        public int Minimum
        {
            get
            {
                return (int)GetValue(MinimumProperty);
            }
            set
            {
                SetMinimumDP(MinimumProperty, value);
            }
        }
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(int),
            typeof(IntSpinnerControl), new FrameworkPropertyMetadata(Int32.MinValue,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnMinimumChanged)));

        public event PropertyChangedEventHandler MinimumChanged;
        void SetMinimumDP(DependencyProperty property, int value)
        {
            SetValue(property, value);
            MinimumChanged?.Invoke(this, new PropertyChangedEventArgs(value.ToString()));
        }
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IntSpinnerControl spinner = d as IntSpinnerControl;
            if (spinner == null) return;
            spinner.Minimum = (int)e.NewValue;
        }

        #endregion

        #region Maximim DP
        public int Maximum
        {
            get
            {
                return (int)GetValue(MaximumProperty);
            }
            set
            {
                SetMaximumDP(MaximumProperty, value);
            }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(int),
            typeof(IntSpinnerControl), new FrameworkPropertyMetadata(Int32.MaxValue,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnMaximumChanged)));

        public event PropertyChangedEventHandler MaximumChanged;
        void SetMaximumDP(DependencyProperty property, int value)
        {
            SetValue(property, value);
            MaximumChanged?.Invoke(this, new PropertyChangedEventArgs(value.ToString()));
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IntSpinnerControl spinner = d as IntSpinnerControl;
            if (spinner == null) return;
            spinner.Maximum = (int)e.NewValue;
        }
        #endregion

        #region Value DP
        public int? Value
        {
            get
            {
                return (int?)GetValue(ValueProperty);
            }
            set
            {
                if(value != OldValue)
                {
                    int? actualValue = ValidateValue(value);
                    OldValue = actualValue;
                    SetValueDP(ValueProperty, actualValue);
                }
            }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int?),
            typeof(IntSpinnerControl), 
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                new PropertyChangedCallback(OnValueChanged)));

        public event PropertyChangedEventHandler ValueChanged;
        void SetValueDP(DependencyProperty property, int? value)
        {
            SetValue(property, value);
            ValueChanged?.Invoke(this, new PropertyChangedEventArgs(property.ToString()));
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IntSpinnerControl spinner = d as IntSpinnerControl;
            if (spinner == null) return;
            spinner.Value = spinner.ValidateValue((int?)e.NewValue);
        }

        #endregion
        public IntSpinnerControl()
        {
            InitializeComponent();

            (Content as FrameworkElement).DataContext = this;
            btnPlus.Click += delegate
            {
                Value++;
            };
            btnMinus.Click += delegate
            {
                Value--;
            };

            tbxField.TextChanged += delegate
            {
                try
                {
                    Value = int.Parse(tbxField.Text);
                }
                catch
                {
                    tbxField.Text = OldValue.ToString();
                }
            };
        }

        private int? ValidateValue (int? inputValue)
        {
            int? actualValue = inputValue;

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

        private void setEnabledButtons(int? value)
        {
            btnMinus.IsEnabled = true;
            btnPlus.IsEnabled = true;

            if (value == Minimum)
            {
                btnMinus.IsEnabled = false;
            }
            if(value == Maximum)
            {
                btnPlus.IsEnabled = false;
            }
        }

    }
}
