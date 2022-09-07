using NuGet.DependencyResolver;
using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class TeacherService: ITeacherService
    {

        public readonly ApplicationDbContext db;

        public TeacherService(ApplicationDbContext db)
        {
            this.db = db;
        }


        public void AddTeacher(Teacher teacher)
        {
            db.Teacher.Add(teacher);
            db.SaveChanges();
        }

        public void DeleteSubTeacherMapped(int subjectId, int teacherId)
        {
            var subTeacher = db.SubjectTeacherMapped.FirstOrDefault(st => st.TeacherId == teacherId && st.SubjectId==subjectId);
            db.SubjectTeacherMapped.Remove(subTeacher);
            db.SaveChanges();
        }

        public void DeleteTeacher(Teacher teacher)
        {
            var subTeacherOfTheTeacher = db.SubjectTeacherMapped.Where(st => st.TeacherId == teacher.Id);
            db.SubjectTeacherMapped.RemoveRange(subTeacherOfTheTeacher);
            db.SaveChanges();

            db.Teacher.Remove(teacher);
            db.SaveChanges();
        }



        public List<Teacher> getAllTeacher()
        {
            var teachers = db.Teacher.ToList();

            return teachers;
        }



        public Teacher GetTeacherById(int id)
        {
            return db.Teacher.Find(id);
        }



        public void UpdateTeacher(Teacher teacher)
        {
            db.Teacher.Update(teacher);
            db.SaveChanges();
        }



        /*
        public List<Teacher> GetTeacherByDepertmentId(int id)
        {
            var teacherDepertment = db.DepartmentTeacherMapped.ToList();
            var teacherIdOfTheDepertment = from deptTeacher in teacherDepertment
                                           where deptTeacher.departmentId == id
                                           orderby deptTeacher.teacherId
                                           select deptTeacher.teacherId;
            List<Teacher> allTeacherList = db.Teacher.ToList();

            var allteacherOfTheDepertment = new List<Teacher>();
            int index = 0;
            foreach (var teacher in allTeacherList)
            {
                if (teacherIdOfTheDepertment.Count() <= index) break;

                if (teacher.Id == teacherIdOfTheDepertment.ElementAt(index)) 
                {
                    allteacherOfTheDepertment.Add(teacher);
                    index++;
                }
            }


            return allteacherOfTheDepertment;
        }

        */


        public List<Subject> GetSubjectByTeacherId(int id)
        {
            var subteacherOfTheTeacher = db.SubjectTeacherMapped.Where(st => st.SubjectId == id);
            var allSubjectOfTheTeacher = new List<Subject>();

            foreach (var subTeacher in subteacherOfTheTeacher)
            {
                allSubjectOfTheTeacher.Add(db.Subject.FirstOrDefault(subject => subject.id == subTeacher.SubjectId));
            }

            return allSubjectOfTheTeacher;
        }

        /*

        private List<Depertment> GetDepertmentByTeacherId(int id)
        {
            var depertmentTeacher = db.DepartmentTeacherMapped.ToList();
            var depertmentIdOfTheTeacher = from deptTeacher in depertmentTeacher
                                           where deptTeacher.teacherId == id
                                        orderby deptTeacher.departmentId
                                        select deptTeacher.departmentId;
            List<Depertment> allDepertmentList = db.Depertment.ToList();

            var allDepertmentOfTheTeacher = new List<Depertment>();
            int index = 0;
            foreach (var depertment in allDepertmentList)
            {
                if (depertmentIdOfTheTeacher.Count() <= index) break;
                if (depertment.id == depertmentIdOfTheTeacher.ElementAt(index)) 
                {
                    allDepertmentOfTheTeacher.Add(depertment);
                    index++;
                }
            }


            return allDepertmentOfTheTeacher;
        }
        */

        public void AddSubjectTeacherMapped(SubjectTeacherMapped subjectTeacherMapped)
        {
            db.SubjectTeacherMapped.Add(subjectTeacherMapped);
            db.SaveChanges();
        }

        public Teacher GetTeacherDetailsById(int id)
        {
            var teacher = GetTeacherById(id);
            teacher.Subjects = GetSubjectByTeacherId(id);
            teacher.Depertments = new List<Depertment>();


            return teacher;
        }

    }
}
