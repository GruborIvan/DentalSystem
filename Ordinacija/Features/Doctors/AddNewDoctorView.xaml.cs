﻿using Ordinacija.Features.Doctors.Models;
using Ordinacija.Features.Doctors.Service;
using System.Windows;

namespace Ordinacija.Features.Doctors
{
    /// <summary>
    /// Interaction logic for AddNewDoctorView.xaml
    /// </summary>
    public partial class AddNewDoctorView : Window
    {
        private bool _isEditMode;
        private readonly IDoctorService _doctorService;

        public readonly DoctorsView DoctorsView;
        public Doctor CurrentDoctor { get; }

        public AddNewDoctorView(
            IDoctorService doctorService,
            DoctorsView doctorsView,
            Doctor? doctor = null)
        {
            InitializeComponent();

            _doctorService = doctorService;
            _isEditMode = doctor != null;

            DoctorsView = doctorsView;
            CurrentDoctor = doctor ?? new Doctor();
            DataContext = CurrentDoctor;

            this.Title = _isEditMode ? "Edit Doctor" : "Add New Doctor";
        }

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            await (
                _isEditMode
                ? _doctorService.UpdateExistingDoctor(CurrentDoctor)
                : _doctorService.CreateNewDoctor(CurrentDoctor));

            await DoctorsView.DoctorViewModel.LoadDoctors();

            this.Close();
        }

        private void PonistiButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Da li ste sigurni da želite da zatvorite stranicu? Imate nesačuvane izmene.",
                "Confirm Exit",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
