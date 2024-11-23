using ClassFindrDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary
{
    public class ScheduleData : IScheduleData
    {
        private readonly ISqlDataAccess _db;    // Instance of the database connection

        public ScheduleData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<ClassModel>> GetUserSchedule()
        {
            // THIS IS JUST EXAMPLE CODE.  REPLACE WITH ACTUAL API CALL LATER
            var output = new List<ClassModel>()
            {
                new ClassModel()
                {
                    CID = 1,
                    CourseCode = "MATH 3377 02",
                    Name = "Intro to Linear Algebra and Matrices",
                    Description = "Students study solving systems of linear equations, fundamental matrix theory (invertibility theorems, determinants), eigenvectors, and properties of linear transformations. Remaining topics are chosen from: Properties of general vector spaces, inner product spaces, and/or diagonalization of symmetric matrices.",
                    Room = "LDB 400",
                    Time = new DateTime(1, 1, 1, 11, 0, 0),
                    Days = "Tu|Th",
                    BRef = 5
                },
                new ClassModel()
                {
                    CID = 2,
                    CourseCode = "COSC 3337 01",
                    Name = "Information System Design and Management",
                    Description = "This is a course in the design and implementation of large-scale file and persistent object-based information systems. Client/Server systems are covered.",
                    Room = "AB1 204",
                    Time = new DateTime(1, 1, 1, 12, 30, 0),
                    Days = "Tu|Th",
                    BRef = 2
                },
                new ClassModel()
                {
                    CID = 3,
                    CourseCode = "COSC 4149 01",
                    Name = "Seminar in Computer Science",
                    Description = "Students learn fundamental ideas of emerging technologies and their real-life applications in ever-evolving software and hardware computing environments. The content of the course may vary from semester to semester, but will include current trends, issues, and professional skills.",
                    Room = "AB1 206",
                    Time = new DateTime(1, 1, 1, 13, 0, 0),
                    Days = "W",
                    BRef = 2
                },
                new ClassModel()
                {
                    CID = 4,
                    CourseCode = "COSC 4319 01",
                    Name = "Software Engineering",
                    Description = "This course is an introduction to formal methods of specifying, designing, implementing and testing software for large programming projects. Methods of estimating and predicting reliability are discussed.",
                    Room = "AB1 204",
                    Time = new DateTime(1, 1, 1, 9, 0, 0),
                    Days = "M|W|F",
                    BRef = 2
                },
                new ClassModel()
                {
                    CID = 5,
                    CourseCode = "COSC 4319 01",
                    Name = "Professionalism and Ethics",
                    Description = "Students examine the nature, need and value of well-formed ethical constructs within the digital forensics profession. Included in this course is a discussion, through case studies, of the nature of professionalism, personal and professional codes of ethics and conduct, and the professional handling of ethical and moral conflict. Students also explore the role of the professional in public policy and the awareness of consequences of ethical dissent and whistle blowing.  Prerequisite: Junior standing.",
                    Room = "FAR 202",
                    Time = new DateTime(1, 1, 1, 14, 0, 0),
                    Days = "Tu|Th",
                    BRef = 6
                }
            };

            return output;
        }

        public bool ScheduleMatches(int UID)
        {
            return true;
        }
    }
}
