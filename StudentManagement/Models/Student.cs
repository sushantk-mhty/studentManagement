using System;
namespace StudentManagement.Models
{
	public class Student
	{
		public Student()
		{
		}
		public int StudentId  { get; set; }
		public string? FullName  { get; set; }
		public string? EmailAddress { get; set; }
		public string? City { get; set; }
		public DateTime CreatedOn { get; set; }
		public int CreatedBy { get; set; }

		public string? Response { get; set; }

	}
}

