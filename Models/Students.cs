using System.ComponentModel.DataAnnotations;

namespace StudentPerformanceApp.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Группа обязательна")]
        [Display(Name = "Группа")]
        public string Group { get; set; }

        [Required(ErrorMessage = "Баллы обязательны")]
        [Range(0, 100, ErrorMessage = "Баллы должны быть от 0 до 100")]
        [Display(Name = "Баллы")]
        public int Score { get; set; }
    }
}