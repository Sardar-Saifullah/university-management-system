# University Management System (UMS) - Backend API

A robust, secure, and scalable backend for a University Management System built with ASP.NET Core Web API, ADO.NET and MySQL.

🚀 Features & Functionalities
Comprehensive Core Module CRUD Operations:

Academic Program Management: Create and manage degree programs, including duration and credit requirements.

Course & Curriculum Management: Handle courses, assign them to programs and departments, and define prerequisites.

Semester & Course Offering System: Schedule semesters and offer specific courses within them, managing enrollment capacities.

Student Enrollment & Registration: Full workflow for course enrollment requests, admin approval/rejection, and status tracking with a complete history log.

Teacher & Assignment Management: Manage teacher profiles and assign them to courses as primary or secondary instructors.

User Management (Admin, Student, Teacher): Centralized user management with distinct profiles for each role.

Credit Limit Policy Engine: Define flexible credit hour policies (by level, program, department) and grant individual student overrides with approval tracking.

Academic History & GPA Calculation: Track student course history, grades, and automatically calculate Term GPA and overall CGPA via stored procedures.

Advanced Security & Authorization:

JWT-based Authentication: Secure login for Admin, Teacher, and Student roles.

Role-Based Access Control (RBAC) with Fine-Grained Permissions: A dynamic permission system where activities (API endpoints) are mapped to profiles, controlling Create, Read, Update, Delete, and Export rights.

Session Management: Track user logins, devices, and allow session revocation for enhanced security.

Supporting Services & Architecture:

Stored Procedure-Centric Data Layer: Core business logic (enrollment checks, GPA calculation) is implemented within MySQL stored procedures for performance and data integrity.

Bulk Upload System: Efficiently process large datasets (e.g., student lists, courses) using JSON-based import functionality.

Profile Picture Management: Upload, store, and serve user profile images with an image processor.

Layered Architecture: Clean separation of concerns (Controller, Service, Repository) for maintainability and testability.

Plain ADO.NET for Data Access: Demonstrates deep understanding of database connectivity and performance optimization without relying on heavy ORMs.

🛠️ Technology Stack
Backend Framework: ASP.NET Core Web API

Database: MySQL

Data Access: Plain ADO.NET (Dapper principles)

Authentication: JWT (JSON Web Tokens)

Security: Password Hashing, RBAC, Session Management

Architecture: Layered (N-Layer)

Key Features: Bulk Upload (JSON), Image Processing, Stored Procedures

📁 Key Database Entities
The system manages a wide range of entities, including Users, Profiles, Activities, StudentProfile, TeacherProfile, AdminProfile, Course, Program, Enrollment, CourseHistory, Semester, and more, with well-defined relationships and constraints.
