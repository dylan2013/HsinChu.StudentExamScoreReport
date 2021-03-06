﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.Data;
using System.Data;

namespace HsinChu.StudentExamScoreReport
{
    public class Utility
    {
        /// <summary>
        /// 透過學生ID,試別名稱,取得課程編號與科目
        /// </summary>
        /// <param name="StudentIDList"></param>
        /// <param name="ExamName"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Get_SCAttend_CourseID_Subject(List<string> StudentIDList, string ExamName, string SchoolYear, string Semster)
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            if (StudentIDList.Count > 0 && ExamName != "" && SchoolYear != "" && Semster != "")
            {
                QueryHelper qh = new QueryHelper();
                string strSQL = "select distinct course.id,subject from course inner join sc_attend on course.id=sc_attend.ref_course_id inner join te_include on course.ref_exam_template_id=te_include.ref_exam_template_id inner join exam on exam.id=te_include.ref_exam_id where sc_attend.ref_student_id in(" + string.Join(",", StudentIDList.ToArray()) + ") and course.school_year=" + SchoolYear + " and course.semester=" + Semster + " and exam.exam_name='" + ExamName + "' order by subject";
                DataTable dt = qh.Select(strSQL);
                foreach (DataRow dr in dt.Rows)
                    retVal.Add(dr["id"].ToString(), dr["subject"].ToString());
            }
            return retVal;
        }

        /// <summary>
        /// 透過班級ID 取得一般與輟學學生ID
        /// </summary>
        /// <param name="ClassIDList"></param>
        /// <returns></returns>
        public static List<string> GetClassStudentIDList18ByClassIDList(List<string> ClassIDList)
        {            
            List<string> retVal = new List<string>();
            if (ClassIDList.Count > 0)
            {
                QueryHelper qh = new QueryHelper();
                string strSQL = "select student.id from student inner join class on class.id=student.ref_class_id where student.status in(1,8) and class.id in(" + string.Join(",", ClassIDList.ToArray()) + ");";
                DataTable dt = qh.Select(strSQL);
                foreach (DataRow dr in dt.Rows)
                    retVal.Add(dr["id"].ToString());
            }
            return retVal;
        }

        /// <summary>
        /// 取得評量比例設定
        /// </summary>
        public static Dictionary<string, decimal> GetScorePercentageHS()
        {
            Dictionary<string, decimal> returnData = new Dictionary<string, decimal>();
            FISCA.Data.QueryHelper qh1 = new FISCA.Data.QueryHelper();
            string query1 = @"select id,CAST(regexp_replace( xpath_string(exam_template.extension,'/Extension/ScorePercentage'), '^$', '0') as integer) as ScorePercentage  from exam_template";
            System.Data.DataTable dt1 = qh1.Select(query1);

            foreach (System.Data.DataRow dr in dt1.Rows)
            {
                string id = dr["id"].ToString();
                decimal sp = 50;
                
                if(decimal.TryParse(dr["ScorePercentage"].ToString(), out sp))
                    returnData.Add(id, sp);
                else
                    returnData.Add(id, 50);
            }
            return returnData;
        }

        /// <summary>
        /// 評量比例設定暫存用
        /// </summary>
        public static Dictionary<string, decimal> ScorePercentageHSDict = new Dictionary<string, decimal>();
    }
}
