using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class TeacherService: CommonUtilityClass,ITeacherService
    {

        public TeacherService(ApplicationDbContext db):base(db) {}


        public void AddTeacher(Teacher teacher)
        {
            db.Teacher.Add(teacher);
            db.SaveChanges();
        }


        public void DeleteTeacher(Teacher teacher)
        {
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
            var subjectTeacher = db.SubjectTeacherMapped.ToList();
            var subjectIdOfTheTeacher = from subTeacher in subjectTeacher
                                        where subTeacher.TeacherId == id
                                        orderby subTeacher.SubjectId
                                        select subTeacher.SubjectId;
            List<Subject> allSubjectList = db.Subject.ToList();

            var allSubjectOfTheTeacher = new List<Subject>();
            int index = 0;
            foreach (var subject in allSubjectList)
            {
                if (subjectIdOfTheTeacher.Count() <= index) break;
                if (subject.id == subjectIdOfTheTeacher.ElementAt(index))
                {
                    allSubjectOfTheTeacher.Add(subject);
                    index++;
                }
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


        public Teacher GetTeacherDetailsById(int id)
        {
            var teacher = GetTeacherById(id);
            teacher.Subjects = GetSubjectByTeacherId(id);
            // teacher.Depertments = GetDepertmentByTeacherId(id);
            teacher.Depertments = new List<Depertment>();


            return teacher;
        }



        public void DeleteSubjectFromSubjectTeacherMapped(int subjectId, int teacherId)
        {
            throw new NotImplementedException();
        }
    }
}
