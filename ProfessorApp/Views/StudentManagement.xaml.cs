﻿using AttendanceSystem.Models; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; 
using Newtonsoft.Json;

namespace ProfessorApp.Pages
{
    public partial class UploadPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "http://localhost:5225/api/student";

        public UploadPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        //Event handler for selecting file
        private async void OnSelectFileClicked(object sender, EventArgs e)
        {
            //Open file picker to select file
            var file = await FilePicker.PickAsync();
            if (file != null)
            {
                // Once file is selected, upload the student data
                await UploadStudentData(file.FullPath);
            }
            else
            {
                Console.WriteLine("No file selected.");
            }
        }

        //Method called to read and upload the txt file
        public async Task UploadStudentData(string filePath)
        {
            try
            {
                var students = ParseStudentFile(filePath);
                foreach (var student in students)
                {
                    var response = await AddStudentToDatabase(student);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Student {student.Username} added successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to add student {student.Username}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        //Parse the txt file into student data
        private List<Student> ParseStudentFile(string filePath)
        {
            var students = new List<Student>();

            var lines = File.ReadAllLines(filePath);
            //Skip first line as it is header
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                //Skip empty lines
                if (string.IsNullOrEmpty(line)) continue;
                var parts = line.Split('\t');

                if (parts.Length == 4)
                {
                    var student = new Student
                    {
                        LastName = parts[0],
                        FirstName = parts[1],
                        Username = parts[2],
                        UTDID = parts[3]
                    };
                    students.Add(student);
                }
            }

            return students;
        }

        //Method to add student to database through API
        private async Task<HttpResponseMessage> AddStudentToDatabase(Student student)
        {
            var studentJson = JsonConvert.SerializeObject(student);
            var content = new StringContent(studentJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiBaseUrl, content);
            return response;
        }

        private void OnAddStudentClicked(object sender, EventArgs e)
        {
            //Toggle the Add Student form
            AddStudentFrame.IsVisible = !AddStudentFrame.IsVisible;

            if (AddStudentFrame.IsVisible)
            {
                FirstNameEntry.Text = string.Empty;
                LastNameEntry.Text = string.Empty;
                UsernameEntry.Text = string.Empty;
                AddStudentUTDIDEntry.Text = string.Empty;
            }
        }
        

        private async void OnSubmitStudentClicked(object sender, EventArgs e)
        {
            var firstName = FirstNameEntry.Text?.Trim();
            var lastName = LastNameEntry.Text?.Trim();
            var username = UsernameEntry.Text?.Trim();
            var utdid = AddStudentUTDIDEntry.Text?.Trim();

            //Check if Username and UTDID are the correct amount of characters
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(username) || string.IsNullOrEmpty(utdid))
            {
                await DisplayAlert("Input Error", "Please fill in all fields.", "OK");
                return;
            }

            if (username.Length != 9)
            {
                await DisplayAlert("Validation Error", "Invaild NETID", "OK");
                return;
            }
            else if (utdid.Length != 10)
            {
                await DisplayAlert("Validation Error", "Invaild UTDID", "OK");
                return;
            }

                //Create new student object
                var student = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    UTDID = utdid
                };

            try
            {
                var studentJson = JsonConvert.SerializeObject(student);
                var content = new StringContent(studentJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ApiBaseUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Student added successfully.", "OK");

                    //Clear the fields for adding another student
                    FirstNameEntry.Text = string.Empty;
                    LastNameEntry.Text = string.Empty;
                    UsernameEntry.Text = string.Empty;
                    AddStudentUTDIDEntry.Text = string.Empty;

                    //Hide form after submission
                    AddStudentFrame.IsVisible = false;  // Hide form after submission
                }
                else
                {
                    await DisplayAlert("Error", "Failed to add student. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            //Clear all the fields
            FirstNameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            UsernameEntry.Text = string.Empty;
            AddStudentUTDIDEntry.Text = string.Empty;

            //Hide the form
            AddStudentFrame.IsVisible = false;
        }

        //Method to delete a student by UTDID
        private async void OnDeleteStudentClicked(object sender, EventArgs e)
        {
            var utdid = DeleteUTDIDEntry.Text?.Trim();

            if (string.IsNullOrEmpty(utdid))
            {
                await DisplayAlert("Input Error", "Please enter a UTDID.", "OK");
                return;
            }

            try
            {
                var checkResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/id/{utdid}"); 
                if (checkResponse.IsSuccessStatusCode)
                {
                    var deleteResponse = await _httpClient.DeleteAsync($"{ApiBaseUrl}/{utdid}"); 
                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Success", $"Student with UTDID {utdid} deleted.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", $"Failed to delete student {utdid}.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Not Found", $"No student found with UTDID {utdid}.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

    }
}

    

