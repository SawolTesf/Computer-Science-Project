﻿using Microsoft.Maui.Controls;
using ProfessorApp.Pages;
using ProfessorApp.Services;
using ProfessorApp.Views;

namespace ProfessorApp
{
    public partial class App : Application
    {
        public App(ClientService clientService)
        {
            InitializeComponent();

            // Set StudentManagement page wrapped in NavigationPage as the initial page
            MainPage = new NavigationPage(new StudentManagement());
        }
    }
}
