using System.Collections.Generic;
using System.Linq;
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


        public void RemoveAllStudentOfTheDepertment(Depertment depertment)
        {
            var students = db.Student.ToList();
            var studentOdTheDept = from student in db.Student
                              where student.DepertmentId == depertment.id
                              select student;

            var enrolMentsOfTheStudentsOfTheDept = new List<SubjectStudentMapped>();
            var enrolments = db.SubjectStudentMapped;

            foreach(var student in studentOdTheDept)
            {
                foreach(var enrol in enrolments)
                {
                    if(enrol.studentId == student.id)
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


        public void RemoveAllSubDeptOfTheDept(int deptId)
        {
            var subDept = db.SubjectDepartmentMapped;
            var subDeptOfTheDept = from sd in subDept where sd.departmentId==deptId select sd;
            db.SubjectDepartmentMapped.RemoveRange(subDeptOfTheDept);
            db.SaveChanges();
        }


        public void DeleteDepertment(Depertment depertment)
        {
            RemoveAllStudentOfTheDepertment(depertment);
            RemoveAllSubDeptOfTheDept(depertment.id);

            db.Depertment.Remove(depertment);
            db.SaveChanges ();
        }


        public void DeleteStudent(Student student)
        {
            db.Student.Remove(student);
            db.SaveChanges();
        }
        public void RemoveAllEnrolmentOfTheStudent(int studentId)
        {
            var subStudent = db.SubjectStudentMapped;
            var enrolmentOfTheStudent = from ss in subStudent where ss.studentId == studentId select ss;
            db.SubjectStudentMapped.RemoveRange(enrolmentOfTheStudent);
            db.SaveChanges();
        }

        public void RemoveAllEnrolmentOfTheSubject(int subjectId)
        {
            var subStudent = db.SubjectStudentMapped;
            var enrolmentOfTheSubject = from ss in subStudent where ss.subjectId == subjectId select ss;
            db.SubjectStudentMapped.RemoveRange(enrolmentOfTheSubject);
            db.SaveChanges();
        }


        public void RemoveAllSubTeacherOfTheSubject(int subjectId)
        {
            var subTeacher = db.SubjectTeacherMapped;
            var subTeacherOfTheSubject = from st in subTeacher where st.SubjectId == subjectId select st;
            db.SubjectTeacherMapped.RemoveRange(subTeacher);
            db.SaveChanges();
        }

        public void DeleteSubject(Subject subject)
        {
            RemoveAllEnrolmentOfTheSubject(subject.id);
            RemoveAllSubTeacherOfTheSubject(subject.id);
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


        public List<Subject> GetSubjectByDepertmentId(int id)
        {
            var subjectDepertment = db.SubjectDepartmentMapped.ToList();
            var subjectIdOfTheDepertment = from subDept in subjectDepertment 
                                           where subDept.departmentId == id
                                           orderby subDept.subjectId
                                           select subDept.subjectId;
            List<Subject> allSubjectList = db.Subject.ToList();

            var allSubjectOfTheDepertment = new List<Subject>();
            int index = 0;
            foreach(var subject in allSubjectList)
            {
                if (subjectIdOfTheDepertment.Count() <= index) break;

                if (subject.id == subjectIdOfTheDepertment.ElementAt(index)) 
                {
                    allSubjectOfTheDepertment.Add(subject);
                    index++;
                }
            }


            return allSubjectOfTheDepertment;
        }

        public List<Student> GetStudentBySubjectId(int id)
        {
            var subjectStudent = db.SubjectStudentMapped.ToList();
            var studentIdOfTheSubject = from subStudent in subjectStudent
                                        where subStudent.subjectId == id
                                        orderby subStudent.studentId
                                        select subStudent.studentId;
            List<Student> allStudentList = db.Student.ToList();

            var allStudentOfTheSubject = new List<Student>();
            int index = 0;
            foreach (var student in allStudentList)
            {
                if (studentIdOfTheSubject.Count() <= index) break;
                if (student.id == studentIdOfTheSubject.ElementAt(index)) 
                {
                    allStudentOfTheSubject.Add(student);
                    index++;
                }
            }


            return allStudentOfTheSubject;
        }
        public List<Teacher> GetTeacherBySubjectId(int id)
        {
            var subjectTeacher = db.SubjectTeacherMapped.ToList();
            var teacherIdOfTheSubject = from subTeacher in subjectTeacher
                                           where subTeacher.SubjectId == id
                                           orderby subTeacher.TeacherId
                                           select subTeacher.TeacherId;
            List<Teacher> allTeacherList = db.Teacher.ToList();

            var allTeacherOfTheSubject = new List<Teacher>();
            int index = 0;
            foreach (var teacher in allTeacherList)
            {
                if (teacherIdOfTheSubject.Count() <= index) break;
                if (teacher.Id == teacherIdOfTheSubject.ElementAt(index)) 
                {
                    allTeacherOfTheSubject.Add(teacher);
                    index++;
                }
            }


            return allTeacherOfTheSubject;
        }


        public List<Depertment> GetDepertmentBySubjectId(int id)
        {
            var subjectDepertment = db.SubjectDepartmentMapped.ToList();
            var depertmentIdOfTheSubject = from subDept in subjectDepertment
                                           where subDept.subjectId == id
                                           orderby subDept.departmentId
                                           select subDept.departmentId;
            List<Depertment> allDepartmentList = db.Depertment.ToList();

            var allDepertmentOfTheSubject = new List<Depertment>();
            int index = 0;
            foreach (var depertment in allDepartmentList)
            {
                if(depertmentIdOfTheSubject.Count() <= index) break;
                if (depertment.id == depertmentIdOfTheSubject.ElementAt(index)) 
                {
                    allDepertmentOfTheSubject.Add(depertment);
                    index++;
                }
            }


            return allDepertmentOfTheSubject;
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

        public List<Student> GetStudentByDepertmentId(int id)
        {

            List<Student> allStudentList = db.Student.ToList();

            var allStudentOfTheDepertment = new List<Student>();

            foreach(var student in allStudentList)
            {
                if (student.DepertmentId == id) allStudentOfTheDepertment.Add(student);
            }

                return (List<Student>)allStudentOfTheDepertment;
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

        public Subject GetSubjectDetailsById(int id)
        {
            var subject = db.Subject.Find(id);
            subject.Depertments = GetDepertmentBySubjectId(id);
            subject.Teachers = GetTeacherBySubjectId(id);
            subject.Students = GetStudentBySubjectId(id);

            return subject;
        }

        public List<Subject> GetSubjectByTeacherId(int id)
        {
            var subjectTeacher = db.SubjectTeacherMapped.ToList();
            var subjectIdOfTheTeacher = from subTeacher in subjectTeacher
                                        where subTeacher.TeacherId== id
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

        public List<Subject> GetSubjectByStudentId(int id)
        {
            var subjectStudent= db.SubjectStudentMapped.ToList();
            var subjectIdOfTheStudent= from subStudent in subjectStudent
                                        where subStudent.studentId == id
                                        orderby subStudent.subjectId
                                        select subStudent.subjectId;
            var deptId = db.Student.Find(id).DepertmentId;

            List<Subject> allSubjectListOfTheDept = GetSubjectByDepertmentId(deptId);

            var allSubjectOfTheStudent = new List<Subject>();
            int index = 0;
            foreach (var subject in allSubjectListOfTheDept)
            {
                if (subjectIdOfTheStudent.Count() <= index) break;
                if (subject.id == subjectIdOfTheStudent.ElementAt(index)) 
                {
                    allSubjectOfTheStudent.Add(subject);
                    index++;
                }
            }


            return allSubjectOfTheStudent;
        }

        public Student GetStudentDetailsById(int id)
        {
            var student = db.Student.Find(id);
            student.DeptName= GetDepertmentById(student.DepertmentId).DeptName;
            student.Subjects = GetSubjectByStudentId(id);

            return student;
        }

        public void AddSubjectDapertmentMapped(SubjectDepartmentMapped subjectDapertmentMapped)
        {
            db.SubjectDepartmentMapped.Add(subjectDapertmentMapped);
            db.SaveChanges();
        }

        public void DeleteSubjectFromSubjectDepartmentMapped(int subjectId, int deptId)
        {
            var subjectDeptList = db.SubjectDepartmentMapped.ToList();
            var subDeptVar =   from sd in ( from subDept in subjectDeptList
                          where subDept.subjectId == subjectId select subDept) where sd.departmentId==deptId select sd;
            if (subDeptVar.Count()>0)
            {
                db.SubjectDepartmentMapped.Remove(subDeptVar.ElementAt(0));
                db.SaveChanges();
            }
            
        }

        public void AddSubjectTeacherMapped(SubjectTeacherMapped subjectTeacherMapped)
        {
            db.SubjectTeacherMapped.Add(subjectTeacherMapped);
            db.SaveChanges();
        }

        public void AddSubjectStudentMapped(SubjectStudentMapped subjectStudentMapped)
        {
            db.SubjectStudentMapped.Add(subjectStudentMapped);
            db.SaveChanges();
        }

        public void DeleteSubjectFromSubjectTeacherMapped(int subjectId, int teacherId)
        {
            var subjectTeacherList = db.SubjectTeacherMapped.ToList();
            var subTeacherVar = from sT in (from subTeacher in subjectTeacherList
                                            where subTeacher.SubjectId == subjectId
                                         select subTeacher)
                             where sT.TeacherId ==teacherId
                             select sT;
            if (subTeacherVar.Count() > 0)
            {
                db.SubjectTeacherMapped.Remove(subTeacherVar.ElementAt(0));
                db.SaveChanges();
            }
        }

        public void DeleteEnrolmentFromSubjectStudentMapped(int subjectId, int studentId)
        {
            var enrolmentList = db.SubjectStudentMapped.ToList();
            var subStudentVar = from ss in (from subStudent in enrolmentList
                                            where subStudent.subjectId == subjectId
                                            select subStudent)
                                where ss.studentId == studentId
                                select ss;
            if (subStudentVar.Count() > 0)
            {
                db.SubjectStudentMapped.Remove(subStudentVar.ElementAt(0));
                db.SaveChanges();
            }
        }

      
    }
}
