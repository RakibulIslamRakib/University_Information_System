using University_Information_System.Data;
using University_Information_System.Models;
using University_Information_System.Services.ServiceInterfaces;

namespace University_Information_System.Services.ServiceClasses
{
    public class DepartmentService: CommonUtilityClass,IDepartmentService
    {

         
        public DepartmentService(ApplicationDbContext db):base(db) {  }


   

        

        public void AddDepertment(Depertment depertment)
        {
            db.Depertment.Add(depertment);
            db.SaveChanges();
        }


        public void UpdateDepertment(Depertment depertment)
        {
            db.Depertment.Update(depertment);
            db.SaveChanges();
        }

        public void DeleteDepertment(Depertment depertment)
        {
            RemoveAllStudentOfTheDepertment(depertment);
            RemoveAllSubDeptOfTheDept(depertment.id);

            db.Depertment.Remove(depertment);
            db.SaveChanges();
        }

        public Depertment GetDepertmentDetailsById(int id)
        {
            var depertment = db.Depertment.Find(id);
            //depertment.Teachers = GetTeacherByDepertmentId(id);
            depertment.Teachers = new List<Teacher>();
            depertment.Students = GetStudentByDepertmentId(id);
            depertment.Subjects = GetSubjectByDepertmentId(id);

            return depertment;
        }



        public List<Student> GetStudentByDepertmentId(int id)
        {

            List<Student> allStudentList = db.Student.ToList();

            var allStudentOfTheDepertment = new List<Student>();

            foreach (var student in allStudentList)
            {
                if (student.DepertmentId == id) allStudentOfTheDepertment.Add(student);
            }

            return (List<Student>)allStudentOfTheDepertment;
        }

        public void RemoveAllStudentOfTheDepertment(Depertment depertment)
        {
            var students = db.Student.ToList();
            var studentOdTheDept = from student in db.Student
                                   where student.DepertmentId == depertment.id
                                   select student;

            var enrolMentsOfTheStudentsOfTheDept = new List<SubjectStudentMapped>();
            var enrolments = db.SubjectStudentMapped;

            foreach (var student in studentOdTheDept)
            {
                foreach (var enrol in enrolments)
                {
                    if (enrol.studentId == student.id)
                    {
                        enrolMentsOfTheStudentsOfTheDept.Add(enrol);
                    }
                }
            }

            db.SaveChanges();

            enrolments.RemoveRange(enrolMentsOfTheStudentsOfTheDept);
            db.SaveChanges();

            db.Student.RemoveRange(studentOdTheDept);
            db.SaveChanges();
        }

        public void AddSubjectDapertmentMapped(SubjectDepartmentMapped subjectDapertmentMapped)
        {
            db.SubjectDepartmentMapped.Add(subjectDapertmentMapped);
            db.SaveChanges();
        }

        public void DeleteSubjectFromSubjectDepartmentMapped(int subjectId, int deptId)
        {
            var subjectDeptList = db.SubjectDepartmentMapped.ToList();
            var subDeptVar = from sd in (from subDept in subjectDeptList
                                         where subDept.subjectId == subjectId
                                         select subDept)
                             where sd.departmentId == deptId
                             select sd;
            if (subDeptVar.Count() > 0)
            {
                db.SubjectDepartmentMapped.Remove(subDeptVar.ElementAt(0));
                db.SaveChanges();
            }

        }

        public void RemoveAllSubDeptOfTheDept(int deptId)
        {
            var subDept = db.SubjectDepartmentMapped;
            var subDeptOfTheDept = from sd in subDept where sd.departmentId == deptId select sd;
            db.SubjectDepartmentMapped.RemoveRange(subDeptOfTheDept);
            db.SaveChanges();
        }

    }
}
