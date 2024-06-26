﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.DAL.SchoolContext
{

    public class SchoolDbContext : IdentityDbContext
    {

        #region DbSets
        public DbSet<Attendance> dbsAttendance { get; set; }

        //public DbSet<StudentAttendance> dbsStudentAttendance { get; set; }
        //public DbSet<StaffAttendance> dbsStaffAttendance { get; set; }

        public DbSet<Standard> dbsStandard { get; set; }
        public DbSet<Department> dbsDepartment { get; set; }
        public DbSet<ExamSchedule> dbsExamSchedule { get; set; }
        public DbSet<ExamSubject> dbsExamSubject { get; set; }
        public DbSet<ExamType> dbsExamType { get; set; }
        public DbSet<Mark> dbsMark { get; set; }       
        public DbSet<Staff> dbsStaff { get; set; }
        public DbSet<StaffExperience> dbsStaffExperience { get; set; }
        public DbSet<StaffSalary> dbsStaffSalary { get; set; }
        public DbSet<Student> dbsStudent { get; set; }
        public DbSet<Subject> dbsSubject { get; set; }
        public DbSet<FeeType> dbsFeeType { get; set; }       
        public DbSet<DueBalance> dbsDueBalance { get; set; }        
        public DbSet<AcademicMonth> dbsAcademicMonths { get; set; }
        public DbSet<AcademicYear> dbsAcademicYears { get; set; }
        public DbSet<Fee> fees { get; set; }
        public DbSet<MonthlyPayment> monthlyPayments { get; set; }
        public DbSet<OthersPayment> othersPayments { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<OtherPaymentDetail> otherPaymentDetails { get; set; }
        public DbSet<PaymentMonth> paymentMonths { get; set; }

        #endregion

        #region Constructor
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        } 
        #endregion

        //This SaveChanges() method is implemented for inserting Computed column [NetSalary column from StaffSalary Table] into Database.
        public override int SaveChanges()
        {
            // Calculate NetSalary before saving changes
            foreach (var entry in ChangeTracker.Entries<StaffSalary>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var staffSalary = entry.Entity;
                    staffSalary.CalculateNetSalary();
                }
            }

            #region Testing_Purpose
            // Validation logic before saving changes

            //var validationErrors = GetValidationErrors();
            //if (validationErrors.Any())
            //{
            //    // Rollback changes
            //    // You can use transaction rollback or any other mechanism here
            //    throw new InvalidOperationException("Validation failed. Changes rolled back.");
            //}

            // Save changes if validation passes 
            #endregion

            return base.SaveChanges();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(u => new { u.UserId, u.LoginProvider, u.ProviderKey });

            modelBuilder.Entity<IdentityUserRole<string>>()
        .HasKey(r => new { r.UserId, r.RoleId });


            // Configure the foreign key constraint for dbsMark referencing dbsSubject

            modelBuilder.Entity<Mark>()
                .HasOne(m => m.Subject)
                .WithMany()
                .HasForeignKey(m => m.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);
            // Specify ON DELETE NO ACTION


            //    modelBuilder.Entity<StaffExperience>()
            //.Property(p => p.ServiceDuration)
            //.HasComputedColumnSql("DATEDIFF(year, JoiningDate, ISNULL(LeavingDate, GETDATE()))"); // Calculate duration in years


            //    modelBuilder.Entity<StaffAttendance>()
            //.Property(e => e.WorkingDate)
            //.HasDefaultValueSql("GETUTCDATE()");

            //modelBuilder.Entity<StudentAttendance>()
            //.Property(e => e.WorkingDate)
            //.HasDefaultValueSql("GETUTCDATE()");


            #region Index
            modelBuilder.Entity<Subject>()
            .HasIndex(s => s.SubjectCode)
            .IsUnique();

                modelBuilder.Entity<Student>()
            .HasIndex(s => s.AdmissionNo)
            .IsUnique();

                modelBuilder.Entity<Student>()
            .HasIndex(s => s.EnrollmentNo)
            .IsUnique();
            
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.UniqueStudentAttendanceNumber)
                .IsUnique();
          
            modelBuilder.Entity<Staff>()
                .HasIndex(s => s.UniqueStaffAttendanceNumber)
                .IsUnique();

            #endregion

            modelBuilder.Entity<AcademicMonth>().HasData(
           new AcademicMonth { MonthId = 1, MonthName = "January" },
           new AcademicMonth { MonthId = 2, MonthName = "February" },
           new AcademicMonth { MonthId = 3, MonthName = "March" },
           new AcademicMonth { MonthId = 4, MonthName = "April" },
           new AcademicMonth { MonthId = 5, MonthName = "May" },
           new AcademicMonth { MonthId = 6, MonthName = "June" },
           new AcademicMonth { MonthId = 7, MonthName = "July" },
           new AcademicMonth { MonthId = 8, MonthName = "August" },
           new AcademicMonth { MonthId = 9, MonthName = "September" },
           new AcademicMonth { MonthId = 10, MonthName = "October" },
           new AcademicMonth { MonthId = 11, MonthName = "November" },
           new AcademicMonth { MonthId = 12, MonthName = "December" }
       );
            for (int year = 2000; year <= 2050; year++)
            {
                modelBuilder.Entity<AcademicYear>().HasData(
                    new AcademicYear { AcademicYearId = year - 2000 + 1, Name = year.ToString() }
                );
            }
            // ----------------------------------------------- //
            #region Attendance
            // Seed Attendance data
            modelBuilder.Entity<Attendance>().HasData(
                new Attendance
                {
                    AttendanceId = 1,                  
                    IsPresent = true
                    //,
                    //Staffs = new Staff[]
                    //{
                    //new Staff { StaffId = 1},
                    //new Staff { StaffId = 2}
                    //},
                    //Students = new Student[]
                    //{
                    //new Student { StudentId = 1 },
                    //new Student { StudentId = 2 }
                    //}
                },
                new Attendance
                {
                    AttendanceId = 2,                    
                    IsPresent = true
                    //,
                    //Staffs = new Staff[]
                    //{
                    //new Staff { StaffId = 3},
                    //new Staff { StaffId = 3}
                    //},
                    //Students = new Student[]
                    //{
                    //new Student { StudentId = 3},
                    //new Student { StudentId = 2 }
                    //}
                },
                new Attendance
                {
                    AttendanceId = 3,
                    IsPresent = true
                    //,
                    //Staffs = new List<Staff>
                    //{
                    //new Staff { StaffId = 1},
                    //new Staff { StaffId = 2}
                    //},
                    //Students = new List<Student>
                    //{
                    //new Student { StudentId = 1},
                    //new Student { StudentId = 2 }
                    //}
                },
                new Attendance
                {
                    AttendanceId = 4,                   
                    IsPresent = true
                    //,
                    //Staffs = new Staff[]
                    //{
                    //new Staff { StaffId = 1},
                    //new Staff { StaffId = 2}
                    //},
                    //Students = new Student[]
                    //{
                    //new Student { StudentId = 1 },
                    //new Student { StudentId = 2 }
                    //}
                }
            );
            #endregion
     // ----------------------------------------------- //
            #region Department
            // Seed Department data
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "IT" },
                new Department { DepartmentId = 2, DepartmentName = "HR" },
                new Department { DepartmentId = 3, DepartmentName = "Finance" }
            );
            #endregion
     // ----------------------------------------------- //
            
     // ----------------------------------------------- //
            #region ExamSubjects
            // Seed ExamSchedule data along with associated ExamSubjects
            modelBuilder.Entity<ExamSchedule>().HasData(
                new ExamSchedule
                {
                    ExamScheduleId = 1,
                    ExamScheduleName = "Midterm Exam",
                    ExamTypeId = 1
                    //,
                    //ExamSubjects = new List<ExamSubject>
                    //{
                    //new ExamSubject { ExamSubjectId = 1},
                    //new ExamSubject { ExamSubjectId = 2}
                    //}
                },
                new ExamSchedule
                {
                    ExamScheduleId = 2,
                    ExamScheduleName = "Final Exam",
                    ExamTypeId = 2
                    //,
                    //ExamSubjects = new List<ExamSubject>
                    //{
                    //new ExamSubject { ExamSubjectId = 1},
                    //new ExamSubject { ExamSubjectId = 2 }
                    //}
                },
                new ExamSchedule
                {
                    ExamScheduleId = 3,
                    ExamScheduleName = "Practical Exam",
                    ExamTypeId = 3
                    //,
                    //ExamSubjects = new List<ExamSubject>
                    //{
                    //new ExamSubject { ExamSubjectId = 2},
                    //new ExamSubject { ExamSubjectId = 3 }
                    //}
                }
            );
            #endregion
     // ----------------------------------------------- //
            #region ExamSubject
            // Seed ExamSubject data
            modelBuilder.Entity<ExamSubject>().HasData(
                new ExamSubject { ExamSubjectId = 1, SubjectId = 1, ExamScheduleId = 1 },
                new ExamSubject { ExamSubjectId = 2, SubjectId = 2, ExamScheduleId = 2 },
                new ExamSubject { ExamSubjectId = 3, SubjectId = 3, ExamScheduleId = 3 },
                new ExamSubject { ExamSubjectId = 4, SubjectId = 1, ExamScheduleId = 1 },
                new ExamSubject { ExamSubjectId = 5, SubjectId = 2, ExamScheduleId = 2 },
                new ExamSubject { ExamSubjectId = 6, SubjectId = 3, ExamScheduleId = 3 }
            );
            #endregion
     // ----------------------------------------------- //
            #region ExamType
            // Seed ExamType data
            modelBuilder.Entity<ExamType>().HasData(
                new ExamType { ExamTypeId = 1, ExamTypeName = "Midterm" },
                new ExamType { ExamTypeId = 2, ExamTypeName = "Final" },
                new ExamType { ExamTypeId = 3, ExamTypeName = "Practical" }
            );
            #endregion
     // ----------------------------------------------- //
            
     // ----------------------------------------------- //
            
     // ----------------------------------------------- //
            
     // ----------------------------------------------- //
            
     // ----------------------------------------------- //
            #region Mark
            // Seed Mark data
            modelBuilder.Entity<Mark>().HasData(
                new Mark
                {
                    MarkId = 1,
                    TotalMarks = 80,
                    PassMarks = 40,
                    ObtainedScore = 65,
                    Grade = Grade.B,
                    PassStatus = Pass.Passed,
                    MarkEntryDate = DateTime.Now,
                    Feedback = "Good job!",
                    StaffId = 1,
                    StudentId = 1,
                    SubjectId = 1
                },
                new Mark
                {
                    MarkId = 2,
                    TotalMarks = 90,
                    PassMarks = 40,
                    ObtainedScore = 75,
                    Grade = Grade.A,
                    PassStatus = Pass.Passed,
                    MarkEntryDate = DateTime.Now,
                    Feedback = "Excellent work!",
                    StaffId = 2,
                    StudentId = 2,
                    SubjectId = 2
                },
                new Mark
                {
                    MarkId = 3,
                    TotalMarks = 90,
                    PassMarks = 40,
                    ObtainedScore = 75,
                    Grade = Grade.A,
                    PassStatus = Pass.Passed,
                    MarkEntryDate = DateTime.Now,
                    Feedback = "Excellent work!",
                    StaffId = 3,
                    StudentId = 3,
                    SubjectId = 3
                }
                // Add more seed data as needed
            );
            #endregion
     // ----------------------------------------------- //
            #region MarkEntry_Excluded
            // Seed MarkEntry data along with associated Marks
            //modelBuilder.Entity<MarkEntry>().HasData(
            //    new MarkEntry
            //    {
            //        MarkEntryId = 1,
            //        MarkEntryDate = DateTime.Now,
            //        StaffId = 1,
            //        SubjectId = 1
            //        //    , 
            //        //    Marks = new List<Mark>
            //        //    {
            //        //new Mark
            //        //{
            //        //    MarkId = 1
            //        //},
            //        //new Mark
            //        //{
            //        //    MarkId = 2
            //        //}
            //        //    }
            //    }
            //    ,
            //    new MarkEntry
            //    {
            //        MarkEntryId = 2,
            //        MarkEntryDate = DateTime.Now,
            //        StaffId = 2,
            //        SubjectId = 2
            //        //    , 
            //        //    Marks = new List<Mark>
            //        //    {
            //        //new Mark
            //        //{
            //        //    MarkId = 2
            //        //}

            //        //    }
            //    }
            //    ,
            //    new MarkEntry
            //    {
            //        MarkEntryId = 3,
            //        MarkEntryDate = DateTime.Now,
            //        StaffId = 3,
            //        SubjectId = 3
            //        //    ,
            //        //    Marks = new List<Mark>
            //        //    {
            //        //new Mark
            //        //{
            //        //    MarkId = 2
            //        //}

            //        //    }
            //    }

            //);
            #endregion
     // ----------------------------------------------- //
            #region Staff
            // Seed Staff data if required
            modelBuilder.Entity<Staff>().HasData(
                new Staff
                {
                    StaffId = 1,
                    StaffName = "John Doe",
                    UniqueStaffAttendanceNumber = 2000,
                    Gender = Gender.Male,
                    DepartmentId = 1,
                    Status = "Active",
                    StaffSalaryId = 1
                    //,
                    //StaffExperiences = new List<StaffExperience>
                    //{
                    //new StaffExperience { StaffExperienceId = 1},
                    //new StaffExperience { StaffExperienceId = 2}
                    //},

                },
                new Staff
                {
                    StaffId = 2,
                    StaffName = "Jane Smith",
                    UniqueStaffAttendanceNumber = 2001,
                    Gender = Gender.Female,
                    DepartmentId = 2,
                    Status = "Active",
                    StaffSalaryId = 2

                    //,
                    //StaffExperiences = new List<StaffExperience>
                    //{
                    //new StaffExperience { StaffExperienceId = 1},
                    //new StaffExperience { StaffExperienceId = 2}
                    //}

                },
                new Staff
                {
                    StaffId = 3,
                    StaffName = "Jane Smith",
                    UniqueStaffAttendanceNumber = 2002,
                    Gender = Gender.Female,
                    DepartmentId = 3,
                    Status = "Active",
                    StaffSalaryId = 3
                    //,
                    //StaffExperiences = new List<StaffExperience>
                    //{
                    //new StaffExperience { StaffExperienceId = 1},
                    //new StaffExperience { StaffExperienceId = 2}
                    //}

                }
            );
            #endregion
     // ----------------------------------------------- //
            #region StaffExperience
            // Seed StaffExperience data if required
            modelBuilder.Entity<StaffExperience>().HasData(
                new StaffExperience
                {
                    StaffExperienceId = 1,
                    CompanyName = "ABC School",
                    Designation = "Teacher",
                    Responsibilities = "Teaching Mathematics and Physics",
                    Achievements = "Improved student performance by 20%",
                },
                new StaffExperience
                {
                    StaffExperienceId = 2,
                    CompanyName = "ABC School",
                    Designation = "Teacher",
                    Responsibilities = "Teaching Mathematics and Physics",
                    Achievements = "Improved student performance by 20%",
                },
                new StaffExperience
                {
                    StaffExperienceId = 3,
                    CompanyName = "ABC School",
                    Designation = "Teacher",
                    Responsibilities = "Teaching Mathematics and Physics",
                    Achievements = "Improved student performance by 20%",
                }

            );
            #endregion
     // ----------------------------------------------- //
            #region StaffSalary
            // Seed StaffSalary data if required
            modelBuilder.Entity<StaffSalary>().HasData(
                new StaffSalary
                {
                    StaffSalaryId = 1,
                    BasicSalary = 5000,
                    FestivalBonus = 1000,
                    Allowance = 500,
                    MedicalAllowance = 300,
                    HousingAllowance = 800,
                    TransportationAllowance = 200,
                    SavingFund = 200,
                    Taxes = 500,
                },
               new StaffSalary
               {
                   StaffSalaryId = 2,
                   BasicSalary = 5000,
                   FestivalBonus = 1000,
                   Allowance = 500,
                   MedicalAllowance = 300,
                   HousingAllowance = 800,
                   TransportationAllowance = 200,
                   SavingFund = 200,
                   Taxes = 500,
               },
               new StaffSalary
               {
                   StaffSalaryId = 3,
                   BasicSalary = 5000,
                   FestivalBonus = 1000,
                   Allowance = 500,
                   MedicalAllowance = 300,
                   HousingAllowance = 800,
                   TransportationAllowance = 200,
                   SavingFund = 200,
                   Taxes = 500,
               }
            );
            #endregion
     // ----------------------------------------------- //
            #region Standard
            // Seed Standard data if required
            modelBuilder.Entity<Standard>().HasData(
                new Standard
                {
                    StandardId = 1,
                    StandardName = "Class One",
                    StandardCapacity = "30 students",
                },
                new Standard
                {
                    StandardId = 2,
                    StandardName = "Class Two",
                    StandardCapacity = "35 students",
                },
                 new Standard
                 {
                     StandardId = 3,
                     StandardName = "Class Three",
                     StandardCapacity = "35 students",
                 }

            );
            #endregion
     // ----------------------------------------------- //
            #region Student
            // Seed Student data if required
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentId = 1,
                    AdmissionNo = 1000,
                    EnrollmentNo = 2000,
                    UniqueStudentAttendanceNumber = 1000,
                    StudentName = "John Doe",
                    StudentGender = GenderList.Male,
                    StudentBloodGroup = "A+",
                    StudentNationality = "American",
                    StudentNIDNumber = "17948678987654320",
                    StudentContactNumber1 = "1234567890",
                    StudentEmail = "john.doe@example.com",
                    PermanentAddress = "123 Main Street, City, Country",
                    TemporaryAddress = "456 Elm Street, City, Country",
                    FatherName = "Michael Doe",
                    FatherNID = "17948678987624322",
                    FatherContactNumber = "9876543210",
                    MotherName = "Alice Doe",
                    MotherNID = "17948678987754322",
                    MotherContactNumber = "9876543220",
                    LocalGuardianName = "Jane Smith",
                    LocalGuardianContactNumber = "9876543230",
                    StandardId = 1,
                },

                new Student
                {
                    StudentId = 2,
                    AdmissionNo = 1001,
                    EnrollmentNo = 2001,
                    UniqueStudentAttendanceNumber = 1001,
                    StudentName = "John Doe",
                    StudentGender = GenderList.Male,
                    StudentBloodGroup = "A+",
                    StudentNationality = "American",
                    StudentNIDNumber = "17948678987654322",
                    StudentContactNumber1 = "1234567890",
                    StudentEmail = "john.doe@example.com",
                    PermanentAddress = "123 Main Street, City, Country",
                    TemporaryAddress = "456 Elm Street, City, Country",
                    FatherName = "Michael Doe",
                    FatherNID = "17948578987654322",
                    FatherContactNumber = "9876543210",
                    MotherName = "Alice Doe",
                    MotherNID = "17948674987654322",
                    MotherContactNumber = "9876543220",
                    LocalGuardianName = "Jane Smith",
                    LocalGuardianContactNumber = "9876543230",
                    StandardId = 2,
                },
                new Student
                {
                    StudentId = 3,
                    AdmissionNo = 1002,
                    EnrollmentNo = 2002,
                    UniqueStudentAttendanceNumber = 1002,
                    StudentName = "John Doe",
                    StudentGender = GenderList.Male,
                    StudentBloodGroup = "A+",
                    StudentNationality = "American",
                    StudentNIDNumber = "17945678987654322",
                    StudentContactNumber1 = "1234567890",
                    StudentEmail = "john.doe@example.com",
                    PermanentAddress = "123 Main Street, City, Country",
                    TemporaryAddress = "456 Elm Street, City, Country",
                    FatherName = "Michael Doe",
                    FatherNID = "17345678987654322",
                    FatherContactNumber = "9876543210",
                    MotherName = "Alice Doe",
                    MotherNID = "12345678987654322",
                    MotherContactNumber = "9876543220",
                    LocalGuardianName = "Jane Smith",
                    LocalGuardianContactNumber = "9876543230",
                    StandardId = 3,
                }
            );
            #endregion
     // ----------------------------------------------- //
            #region Subject
            // Seed Subject data if required
            modelBuilder.Entity<Subject>().HasData(
                new Subject
                {
                    SubjectId = 1,
                    SubjectName = "Mathematics",
                    SubjectCode = 101,
                    StandardId = 1
                },
                new Subject
                {
                    SubjectId = 2,
                    SubjectName = "Physics",
                    SubjectCode = 102,
                    StandardId = 2
                },
                new Subject
                {
                    SubjectId = 3,
                    SubjectName = "Chemistry",
                    SubjectCode = 103,
                    StandardId = 3
                },
                new Subject
                {
                    SubjectId = 4,
                    SubjectName = "Biology",
                    SubjectCode = 104,
                    StandardId = 1
                },
                new Subject
                {
                    SubjectId = 5,
                    SubjectName = "Computer Science",
                    SubjectCode = 105,
                    StandardId = 2
                },
                new Subject
                {
                    SubjectId = 6,
                    SubjectName = "Electronics",
                    SubjectCode = 106,
                    StandardId = 3
                }

            );
            #endregion
     // ----------------------------------------------- //
            #region StudentAttendance_Excluded
            // Seed StudentAttendance data if required
            //modelBuilder.Entity<StudentAttendance>().HasData(
            //    new StudentAttendance
            //    {
            //        StudentAttendanceId = 1,
            //        WorkingDate = new DateOnly(2024, 04, 08)
            //    },

            //    new StudentAttendance
            //    {
            //        StudentAttendanceId = 2,
            //        WorkingDate = new DateOnly(2024, 04, 09)
            //    },
            //    new StudentAttendance
            //    {
            //        StudentAttendanceId = 3,
            //        WorkingDate = new DateOnly(2024, 04, 10)
            //    },
            //    new StudentAttendance
            //    {
            //        StudentAttendanceId = 4
            //    },
            //    new StudentAttendance
            //    {
            //        StudentAttendanceId = 5
            //    },
            //    new StudentAttendance
            //    {
            //        StudentAttendanceId = 6
            //    }

            //);
            #endregion
     // ----------------------------------------------- //
            #region StaffAttendance_Excluded
            // Seed StaffAttendance data if required
            //modelBuilder.Entity<StaffAttendance>().HasData(
            //    new StaffAttendance
            //    {
            //        StaffAttendanceId = 1,
            //        WorkingDate = new DateOnly(2024, 04, 08)
            //    },
            //    new StaffAttendance
            //    {
            //        StaffAttendanceId = 2,
            //        WorkingDate = new DateOnly(2024, 04, 09)
            //    },
            //    new StaffAttendance
            //    {
            //        StaffAttendanceId = 3,
            //        WorkingDate = new DateOnly(2024, 04, 10)
            //    },
            //    new StaffAttendance
            //    {
            //        StaffAttendanceId = 4
            //    },
            //    new StaffAttendance
            //    {
            //        StaffAttendanceId = 5
            //    },
            //    new StaffAttendance
            //    {
            //        StaffAttendanceId = 6
            //    }

            //); 
            #endregion

        }
    }
}