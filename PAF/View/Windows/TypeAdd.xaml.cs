﻿using PAF.ViewModel;
using System.Windows;

namespace PAF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для TypeAdd.xaml
    /// </summary>
    public partial class TypeAdd : Window
    {
        public TypeAdd()
        {
            InitializeComponent();
            this.DataContext = new TypeVM();
        }
    }
}
