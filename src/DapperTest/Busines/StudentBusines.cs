using DapperTest.Model;
using DapperTest.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DapperTest.Busines
{
    public class StudentBusines
    {
        private static StudentService studentService = new StudentService();
        public IEnumerable<StudentModel> GetStudentList()
        {
            return studentService.GetStudentList();
        }
    }
}
