﻿using Prism.Commands;
using SMS.MVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SMS.MVVM.ViewModels
{
	public class EditStudentWindowVM : INotifyPropertyChanged, ICloseWindows
	{

		// #begin INotifyPropertyChanged Interface 
		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		// #end

		// for ICloseWindows
		public Action CloseAction { get; internal set; }
		public Action Close { get; set; }


		private string _firstName;

		public string FirstName { get { return _firstName; } set { _firstName = value; OnPropertyChanged(nameof(FirstName)); } }

		private string _lastName;

		public string LastName { get { return _lastName; } set { _lastName = value; OnPropertyChanged(nameof(LastName)); } }

		private string _imagei;

		public string Imagei { get { return _imagei; } set { _imagei = value; OnPropertyChanged(nameof(Imagei)); } }

		private string _gender;

		public string Gender { get { return _gender; } set { _gender = value; OnPropertyChanged(nameof(Gender)); } }

		public List<string> _genders;
		public List<string> Genders { get { return _genders; } set { _genders = value; OnPropertyChanged(nameof(Genders)); } }

		private DateTime _dateOfBirth;

		public DateTime DateOfBirth { get { return _dateOfBirth; } set { _dateOfBirth = value; OnPropertyChanged(nameof(DateOfBirth)); } }

		private string _email;

		public string Email { get { return _email; } set { _email = value; OnPropertyChanged(nameof(Email)); } }


		private string _gPA;

		public string GPA { get { return _gPA; } set { _gPA = value; OnPropertyChanged(nameof(GPA)); } }


		private string _browseImageText;

		public string BrowseImageText { get { return _browseImageText; } set { _browseImageText = value; OnPropertyChanged(nameof(BrowseImageText)); } }

		// Close Button Command
		private DelegateCommand _closeCommand;
		public DelegateCommand CloseCommand =>
			_closeCommand ?? (_closeCommand = new DelegateCommand(ExecuteCloseCommand));

		void ExecuteCloseCommand()
		{
			using (DataContext context = new DataContext())
			{
				Student tmp = context.Students.Single(x => x.IsSelected == true);
				tmp.IsSelected = false;
				context.SaveChanges();
			}

			Close?.Invoke();
		}

		// Create Button Command
		private DelegateCommand _createCommand;
		public DelegateCommand CreateCommand =>
			_createCommand ?? (_createCommand = new DelegateCommand(ExecuteCreateCommand));


		void ExecuteCreateCommand()
		{
			using (DataContext context = new DataContext())
			{
				Student tmp = context.Students.Single(x => x.IsSelected == true);
				if (tmp != null)
				{
					tmp.FirstName = _firstName;
					tmp.LastName = _lastName;
					tmp.Gender = _gender[0];
					tmp.Email = _email;
					tmp.Image = _imagei;
					tmp.DateOfBirth = _dateOfBirth.ToShortDateString();
					tmp.GPA = Convert.ToDouble(_gPA);
					tmp.Email = _email;
					tmp.IsSelected = false;
					context.SaveChanges();
				}
				else
				{
					MessageBox.Show("Please select a student again");
				}
			}
			MessageBox.Show("Refresh the student records to see the changes 😊");
			Close?.Invoke();
		}

		// Image Browse Button Command
		private DelegateCommand _imageBrowseCommand;
		public DelegateCommand ImageBrowseCommand =>
			_imageBrowseCommand ?? (_imageBrowseCommand = new DelegateCommand(ExecuteImageBrowseCommand));


		void ExecuteImageBrowseCommand()
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			// Set filter for file extension and default file extension 
			dlg.DefaultExt = ".png";
			dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


			// Display OpenFileDialog by calling ShowDialog method 
			Nullable<bool> result = dlg.ShowDialog();


			// Get the selected file name and display in a TextBox 
			if (result == true)
			{
				// Open document 
				string filename = dlg.FileName;
				//textBox1.Text = filename;
				_imagei = filename;
				_browseImageText = Path.GetFileName(_imagei);
			}
			MessageBox.Show(Path.GetFileName(_imagei) + " has been selected 😊");

		}

		public EditStudentWindowVM()
		{
			Genders = new List<string> { "Male", "Female" };
			_browseImageText = "Browse Image";
			string dateString = "2000-01-01";
			DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
			_dateOfBirth = date;

			using (DataContext context = new DataContext())
			{
				Student tmp = context.Students.Single(x => x.IsSelected == true);
				_firstName = tmp.FirstName;
				_lastName = tmp.LastName;
				_gender = tmp.Gender.ToString()=="M"?"Male":"Female";
				_imagei = tmp.Image;
				_dateOfBirth = DateTime.Parse(tmp.DateOfBirth);
				_gPA = tmp.GPA.ToString();
				_email = tmp.Email;
			}
		}
	}

}
