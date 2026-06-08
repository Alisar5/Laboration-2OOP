
using Laboration_2OOP.ViewModels;
using System;

using System.Windows;


namespace Laboration_2OOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly MainViewModel _vm;



        public MainWindow()
        {
            InitializeComponent();

            _vm = new MainViewModel();
            DataContext = _vm;


        }
        // UC1: Anmäl / Avanmäl


    }


}