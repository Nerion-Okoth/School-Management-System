using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolManagementBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades");

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "ProfileId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Relationships",
                keyColumns: new[] { "ParentId", "StudentId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Relationships",
                keyColumns: new[] { "ParentId", "StudentId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Parents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Parents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Course",
                table: "Teachers");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades");

            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "Teachers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Parents",
                columns: new[] { "Id", "Email", "FullName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "johndoe@school.com", "John Doe", "1234567890" },
                    { 2, "janesmith@school.com", "Jane Smith", "0987654321" }
                });

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Course",
                value: "Mathematics");

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Course", "Email", "FullName", "PhoneNumber" },
                values: new object[] { 2, "Science", "msbrown@school.com", "Ms. Brown", "5559876543" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 2, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "parent1@school.com", "hashedpassword", "Parent", "parent1" },
                    { 3, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "teacher1@school.com", "hashedpassword", "Teacher", "teacher1" },
                    { 4, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "student1@school.com", "hashedpassword", "Student", "student1" }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "Description", "Name", "TeacherId" },
                values: new object[] { 2, "Science-focused class", "Grade 9 - Section B", 2 });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreditHours", "Description", "Name", "TeacherId" },
                values: new object[] { 2, 4, "Basic concepts of Physics", "Physics", 2 });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "ProfileId", "Address", "AdminUserUserId", "Contact", "DateOfBirth", "ImageUrl", "Name", "UserId" },
                values: new object[,]
                {
                    { 2, "123 Parent St", null, "parent1@school.com", null, "/images/parent1.jpg", "Parent User", 2 },
                    { 3, "123 Teacher St", null, "teacher1@school.com", null, "/images/teacher1.jpg", "Teacher User", 3 },
                    { 4, "123 Student St", null, "student1@school.com", null, "/images/student1.jpg", "Student User", 4 }
                });

            migrationBuilder.InsertData(
                table: "Relationships",
                columns: new[] { "ParentId", "StudentId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Age", "ClassId", "CourseId", "CreatedAt", "FirstName", "LastName", "ParentId", "UpdatedAt" },
                values: new object[] { 2, 14, 2, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emily", "Davis", null, null });

            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "Id", "CourseId", "Date", "IsPresent", "StudentId" },
                values: new object[] { 2, 2, new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "CourseId", "Score", "StudentId" },
                values: new object[] { 2, 2, 92.0, 2 });

            migrationBuilder.InsertData(
                table: "Relationships",
                columns: new[] { "ParentId", "StudentId" },
                values: new object[] { 2, 2 });

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
