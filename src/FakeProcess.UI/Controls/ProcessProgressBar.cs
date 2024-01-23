using CommunityToolkit.Mvvm.Input;
using FakeProcess.Domain.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FakeProcess.UI.Controls
{
    public class ProcessProgressBar : Control
    {
        public string ProcessName 
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty ProcessNameProperty =
            DependencyProperty.Register(nameof(ProcessName), typeof(string), typeof(ProcessProgressBar));

        public ProcessType Type
        {
            get { return (ProcessType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(nameof(Type), typeof(ProcessType), typeof(ProcessProgressBar));

        public ProcessState State
        {
            get { return (ProcessState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(ProcessState), typeof(ProcessProgressBar));

        public double ProgressValue
        {
            get { return (double)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.Register(nameof(ProgressValue), typeof(double), typeof(ProcessProgressBar), new PropertyMetadata(0.0));
       
        public IRelayCommand<int> OnDelete
        {
            get { return (IRelayCommand<int>)GetValue(OnDeleteProperty); }
            set { SetValue(OnDeleteProperty, value); }
        }
        public static readonly DependencyProperty OnDeleteProperty =
            DependencyProperty.Register(nameof(OnDelete), typeof(IRelayCommand<int>), typeof(ProcessProgressBar));


        public object OnDeleteParameter
        {
            get { return (object)GetValue(OnDeleteParameterProperty); }
            set { SetValue(OnDeleteParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnDeleteParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnDeleteParameterProperty =
            DependencyProperty.Register(nameof(OnDeleteParameter), typeof(object), typeof(ProcessProgressBar));



        public string Lable
        {
            get { return (string)GetValue(LableProperty); }
            set { SetValue(LableProperty, value); }
        }

        public static readonly DependencyProperty LableProperty =
            DependencyProperty.Register("Lable", typeof(string), typeof(ProcessProgressBar), new PropertyMetadata("Не активно"));



        static ProcessProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProcessProgressBar), new FrameworkPropertyMetadata(typeof(ProcessProgressBar)));
        }
    }
}
