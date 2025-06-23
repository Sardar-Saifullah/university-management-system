# UMS backend project 
📚 Learning Management System (LMS)
🛠 Tech Stack
Frontend: React.js
Backend: ASP.NET Core 6 (Web API)
Database: MySQL
🧩 MODULE STRUCTURE + FEATURES
1️⃣ Authentication & Authorization Module
Features:

Register users (Student, Teacher, Admin) using stored procedures
Login using stored procedures
Generate JWT tokens on login
Role-based authorization
Optional: Refresh tokens
Log login attempts (success/failure)
Secure password hashing (BCrypt or SHA256)
2️⃣ Profile Management Module (Per Role)
Features:

View personal profile

Update personal information (name, email, password, etc.)

Role-specific fields

e.g., Subject expertise for teachers
3️⃣ Course Management Module
👤 Admin:
Create, update, delete courses
Assign teachers to courses
👨‍🏫 Teacher:
View assigned courses
👨‍🎓 Student:
View available/enrolled courses
General Features:

Full CRUD operations
Assignment logic with validation
Pagination, search, and filtering
4️⃣ Course Enrollment Module
👤 Admin:
Manually enroll/unenroll students
👨‍🎓 Student:
Request enrollment / auto-enroll
👨‍🏫 Teacher:
View list of enrolled students
Features:

Role-based CRUD operations for enrollments
5️⃣ Content Delivery Module
Features:

Upload lessons (PDF, DOC, MP4)
CRUD operations on lessons (based on roles)
Organize lessons by topic/module
Support for downloads
Display content hierarchy: Course → Topic → Lessons
Roles:

Admin/Teacher: Upload & manage content
Student: View/download lessons
6️⃣ Quiz Engine (Core Feature)
Features:

Create question bank per course

Add/edit/delete questions with difficulty tags

Tags: Easy, Medium, Hard
Adaptive quiz logic:

Start at medium
Adjust difficulty based on answers
Randomized questions for each quiz session

Built-in timer

Auto score calculation

Roles:

Admin/Teacher: Manage question bank
Student: Attempt quizzes
7️⃣ Results & Analytics Module
Features:

View individual quiz history
Track progress per course/module
Optional: Export results to CSV/PDF
Admin dashboard to view user activity