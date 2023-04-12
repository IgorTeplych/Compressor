﻿using LZW.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LZW.View
{
    /// <summary>
    /// Логика взаимодействия для Miniature.xaml
    /// </summary>
    public partial class Miniature : UserControl
    {
        public MainViewModel mainViewModel;
        public Miniature()
        {
            mainViewModel = new MainViewModel();
            this.DataContext = mainViewModel;
            InitializeComponent();
        }
    }
}
