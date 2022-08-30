using University_Information_System.Data;
using University_Information_System.Models;

namespace University_Information_System.Services
{
    public class MainServices : IMainService
    {
        private readonly ApplicationDbContext db;

        public MainServices(ApplicationDbContext db)
        {
            this.db = db;

        }


        //Add:
        public void AddDepertment(Depertment depertment)
        {
            db.Depertment.Add(depertment);
            db.SaveChanges();
        }


        public void AddStudent(Student student)
        {
           db.Student.Add(student); 
            db.SaveChanges();
        }

        public void AddSubject(Subject subject)
        {
            db.Subject.Add(subject);
            db.SaveChanges();
        }

        public void AddTeacher(Teacher teacher)
        {
           db.Teacher.Add(teacher);
            db.SaveChanges();  
        }


        //Delete:

        public void RemoveAllSubjectOfTheDepertment(Depertment depertment)
        {
            var subjects = db.Subject.ToList();
            var subjectDept = from subject in db.Subject
                              where subject.Depertment == depertment
                              select subject;

            db.Subject.RemoveRange(subjectDept);
            db.SaveChanges();
        }

        public void RemoveAllStudentOfTheDepertment(Depertment depertment)
        {
            var students = db.Student.ToList();
            var studentDept = from student in db.Student
                              where student.Depertment == depertment
                              select student;

            db.Student.RemoveRange(studentDept);
            db.SaveChanges();
        }

        public void RemoveAllTeacherOfTheDepertment(Depertment depertment)
        {
            var teachers = db.Teacher.ToList();
            var teacherDept = from teacher in db.Teacher
                              where teacher.Depertment == depertment
                              select teacher;

            db.Teacher.RemoveRange(teacherDept);
            db.SaveChanges();
        }


        public void DeleteDepertment(Depertment depertment)
        {
            RemoveAllStudentOfTheDepertment(depertment);
            RemoveAllSubjectOfTheDepertment(depertment);
            RemoveAllTeacherOfTheDepertment(depertment);
            db.Depertment.Remove(depertment);
            db.SaveChanges ();
        }


        public void DeleteStudent(Student student)
        {
            db.Student.Remove(student);
            db.SaveChanges();
        }

        public void DeleteSubject(Subject subject)
        {
            db.Subject.Remove(subject);
            db.SaveChanges();
        }

        public void DeleteTeacher(Teacher teacher)
        {
            db.Teacher.Remove(teacher);
            db.SaveChanges();
        }



        //Return List:
        public List<Depertment> getAllDepertment()
        {
            var depertments = db.Depertment.ToList();
            
            return depertments;
        }

        public List<Student> getAllStudent()
        {
            var students = db.Student.ToList();
            
            return students;
        }

        public List<Subject> getAllSubject()
        {
            var subjects = db.Subject.ToList();
           
            return subjects;
        }

        public List<Teacher> getAllTeacher()
        {
            var teachers = db.Teacher.ToList();
           
            return teachers;
        }




      
        public Depertment GetDepertmentById(int id)
        {

            return db.Depertment.Find(id);
        }

        public Student GetStudentById(int id)
        {

            return db.Student.Find(id);  
        }

        public Subject GetSubjectById(int id)
        {
            return db.Subject.Find(id);
        }

        public Teacher GetTeacherById(int id)
        {
            return db.Teacher.Find(id);
        }


        public void UpdateDepertment(Depertment depertment)
        {
            db.Depertment.Update(depertment);
            db.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            db.Student.Update(student);
            db.SaveChanges();
        }

        public void UpdateSubject(Subject subject)
        {
            db.Subject.Update(subject);
            db.SaveChanges();
        }

        public void UpdateTeacher(Teacher teacher)
        {
            db.Teacher.Update(teacher);
            db.SaveChanges();
        }
    }
}
