using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTest.Model
{
    [Table("Student")]
    public class StudentModel
    {
        [ExplicitKey]
        public string StudentId { get; set; }

        public string StudentCode { get; set; }

        public string StudentName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifyDate { get; set; }

        #region 自定义

        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create()
        {
            this.Create(null);
        }

        /// <summary>
        /// 新增调用
        /// </summary>
        public void Create(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.StudentId = keyValue;
            }
            else
            {
                this.StudentId = Guid.NewGuid().ToString();
            }
            this.CreateDate = DateTime.Now;
        }


        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public void Modify(string keyValue)
        {
            this.StudentId = keyValue;
            Modify();
        }

        /// <summary>
        /// 编辑调用
        /// </summary>
        public void Modify()
        {
            this.ModifyDate = DateTime.Now;
        }
        #endregion
    }
}
