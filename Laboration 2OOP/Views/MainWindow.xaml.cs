using Laboration_2OOP.DemoData;
using Laboration_2OOP.Domän;
using Laboration_2OOP.Requests;
using Laboration_2OOP.Services;
using Laboration_2OOP.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
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

    public class UiMember
    {
        public int Id { get; }
        public string DisplayText { get; }
        public UiMember(int id, string text) { Id = id; DisplayText = text; }
    }

    public class UiEvent
    {
        public int Id { get; }
        public string DisplayText { get; }
        public UiEvent(int id, string text) { Id = id; DisplayText = text; }
    }

    public class UiGame
    {
        public int Id { get; }
        public string DisplayText { get; }
        public UiGame(int id, string text) { Id = id; DisplayText = text; }
        public override string ToString() => DisplayText;
    }
}
        
    
