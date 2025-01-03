﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Prism.Commands;
using SMS.MVVM.Models;
using SMS.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SMS.MVVM.ViewModels
{
	public partial class MainWindowVM : ObservableObject
	{
		[ObservableProperty]
		public ObservableCollection<Student> students;

		[ObservableProperty]
		public Student selectedStudent = null;

		[ObservableProperty]
		public string counterText;

		[RelayCommand]
		public void RemovePerson()
		{
			if (selectedStudent != null)
			{
				string name = selectedStudent.FirstName;
				students.Remove(selectedStudent);
				MessageBox.Show($"{name} is Deleted successfuly.", "DELETED \a ");

			}
			else
			{
				MessageBox.Show("Plese Select Student before Delete.", "Error");


			}
		}
		[RelayCommand]
		public void AddPerson()
		{
			//Students.Add(new Student("Albert", "Newton", "2.png", "2002/01/12", 3.69));
			var window = new AddStudWinow();
			window.Show();

		}

		[RelayCommand]
		public void RefreshStudentRecords()
		{
			Read();
			CounterText = students.Count.ToString() + " College Students";
		}

		// To aceess parent ancestor's data context, have to had to use DelegateCommand from "Prism" 
		private DelegateCommand<Student> _deleteCommand;
		public DelegateCommand<Student> DeleteCommand =>
			_deleteCommand ?? (_deleteCommand = new DelegateCommand<Student>(ExecuteDeleteCommand));

		void ExecuteDeleteCommand(Student parameter)
		{
			using (DataContext context = new DataContext())
			{
				Student selectedStudent = parameter;

				if (selectedStudent != null)
				{
					Student std = context.Students.Single(x => x.Id == selectedStudent.Id);

					context.Remove(std);
					context.SaveChanges();
				}
			}
			Read();
			CounterText = students.Count.ToString() + " College Students";
		}

		// To aceess parent ancestor's data context, have to had to use DelegateCommand from "Prism" 
		private DelegateCommand<Student> _editCommand;
		public DelegateCommand<Student> EditCommand =>
			_editCommand ?? (_editCommand = new DelegateCommand<Student>(ExecuteEditCommand));

		void ExecuteEditCommand(Student parameter)
		{
			// change clicked student's IsSelected property, false -> true
			using (DataContext context = new DataContext())
			{
				context.Students.Single(x => x.Id == parameter.Id).IsSelected = true;
				context.SaveChanges();
			}

			var window = new EditStudentWindow();
			window.Show();
		}


		public MainWindowVM()
		{
			students = new ObservableCollection<Student>();


			//for(int i=0; i < 10; i++)
			//{
			//	Students.Add(new Student("Kamal", "Perera", 'M', "1.png", "24/10/2000", 3.89, "kamal@gmail.com"));
			//	Students.Add(new Student("Mala", "Cooray", 'F', "3.png", "12/01/2001", 3.49, "mala@gmail.com"));

			//}


			Random random = new Random();

			using (DataContext context = new DataContext())
			{
				for (int i = 0; i < 2; i++)
				{
					string randomString1 = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", 4)
					  .Select(s => s[random.Next(s.Length)]).ToArray());
					string randomString2 = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", 4)
					  .Select(s => s[random.Next(s.Length)]).ToArray());

					char gender = (random.Next(2) == 0) ? 'M' : 'F';

					char randomChar = (char)(random.Next(49, 58));

					double gpa = random.NextDouble() * (4.0 - 3.0) + 3.0;
					gpa = Math.Round(gpa, 2);

					context.Students.Add(new Student(randomString1, randomString2, gender, "/Images/" + randomChar + ".png", randomChar + "/2" + randomChar + "/200" + randomChar, gpa, randomString1 + "@gmail.com"));
				}
				context.SaveChanges();
			}
			Read();




			CounterText = students.Count.ToString() + " College Students";


		}


		public void Read()
		{
			using (DataContext context = new DataContext())
			{
				//students = context.Students.ToList();
				students.Clear();
				foreach (var std in context.Students.ToList())
				{
					students.Add(std);
				}

				//students = context.Students.ToList();
				//students.Clear();
				//foreach (var std in context.Students)
				//{
				//	context.Students.Remove(std);
				//}
			}

		}
	}

}
