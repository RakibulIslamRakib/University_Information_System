namespace University_Information_System.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string TeachersName { get; set; }

        public virtual Depertment Depertment { get; set; }
    }
}