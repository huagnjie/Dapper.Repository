using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dappers.Repository;
using DapperTest.Model;

namespace DapperTest.Service
{
    public class StudentService: RepositoryFactory<StudentModel>
    {
        public IEnumerable<StudentModel> GetStudentList() {
            return this.BaseRepository().QueryList();
        }
    }
}
