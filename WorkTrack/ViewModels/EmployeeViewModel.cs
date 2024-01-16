using System.ComponentModel.DataAnnotations;
using WorkTrack.Models;

namespace WorkTrack.ViewModels
{
	public class EmployeeViewModel
	{
		public int EmployeeId { get; set; }

		[Display(Name = "Ім'я")]
		[Required(ErrorMessage = "Це поле не повинно бути порожнім")]
		[MaxLength(20, ErrorMessage = "Довжина цього поля не повинна перевищувати 20 симолів")]
		public string FirstName { get; set; } = null!;

		[Display(Name = "Прізвище")]
		[Required(ErrorMessage = "Це поле не повинно бути порожнім")]
		[MaxLength(20, ErrorMessage = "Довжина цього поля не повинна перевищувати 20 симолів")]
		public string LastName { get; set; } = null!;

		[Display(Name = "По батькові")]
		[Required(ErrorMessage = "Це поле не повинно бути порожнім")]
		[MaxLength(20, ErrorMessage = "Довжина цього поля не повинна перевищувати 20 симолів")]
		public string MiddleName { get; set; } = null!;

		[Display(Name = "Номер телефону")]
		[Required(ErrorMessage = "Це поле не повинно бути порожнім")]
		[MaxLength(12, ErrorMessage = "Довжина цього поля не повинна перевищувати 12 симолів")]
		[RegularExpression(@"^\(\d{3}\)\d{7}$", ErrorMessage = "Невірний формат для номера телефону")]
		public string PhoneNumber { get; set; } = null!;

		[Display(Name = "Дата народження")]
		[Required(ErrorMessage = "Це поле не повинно бути порожнім")]
		[DataType(DataType.Date, ErrorMessage = "Введіть коректний формат дати (наприклад, ДД-ММ-РРРР)")]
		public DateTime BirthDate { get; set; }

		[Display(Name = "Дата прийняття на роботу")]
		[Required(ErrorMessage = "Це поле не повинно бути порожнім")]
		[DataType(DataType.Date, ErrorMessage = "Введіть коректний формат дати (наприклад, ДД-ММ-РРРР)")]
		public DateTime HireDate { get; set; }

		[Display(Name = "Оклад")]
		[Required(ErrorMessage = "Це поле не повинно бути порожнім")]
		[Range(1, 1000000000, ErrorMessage = "Це поле повинно бути більшим за 0")]
		[RegularExpression(@"^\d+(\,\d{2})?$", ErrorMessage = "Невірний формат")]
		public decimal Salary { get; set; }

		public AddressViewModel Address { get; set; } = null!;

		public Position? Position { get; set; }

		public Department? Department { get; set; }

		[Display(Name = "Відділ")]
		[Required(ErrorMessage = "Це поле не повинно бути порожнім")]
		public int SelectedDepartmentId { get; set; }

		[Display(Name = "Посада")]
		[Required(ErrorMessage = "Це поле не повинно бути порожнім")]
		public int SelectedPositionId { get; set; }
	}
}
