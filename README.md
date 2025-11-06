Project Title: University Management System (UMS)
Technology Stack: ASP.NET Core Web API | Plain ADO.NET | MySQL | Stored Procedures | Layered Architecture

Overview:

A complete University Management System designed to streamline and automate core academic and administrative workflows.

Built with ASP.NET Core Web API following a clean layered architecture for scalability and maintainability.

Uses Plain ADO.NET for direct database interaction with MySQL and optimized stored procedures for business logic execution.

Key Features:

🔐 Authentication & Authorization:

Implemented JWT-based authentication for secure user sessions.

Added role-based access control (RBAC) via activity-profile mapping.

Permission filter middleware for action-level authorization.

Session tracking with token revocation and device info logging.

👥 User Management:

CRUD operations for Users, Profiles, and Permissions.

Supports Admin, Teacher, and Student registration and management.

Integrated password hashing for enhanced security.

🏫 Academic Management:

Full CRUDs for Departments, Programs, and Levels.

Course Management with Course Prerequisites and Semester Offerings.

Tracks Teacher Assignments, Enrollments, and Student Course Histories.

🎓 Enrollment & Registration:

Supports student registration, course enrollment, and status tracking (pending, approved, dropped, withdrawn).

Admin approval system for managing course enrollments.

Automated history recording through enrollment status history.

⚙️ Credit Policy System:

Credit Limit Policies with overrides per student.

Supports department, program, or level-based application.

Approval and expiration tracking for override requests.

🖼️ Media Management:

Built-in Image Processor for Profile Picture Uploads.

Handles file metadata (type, size, path) with database tracking.

📦 Data Management & Utilities:

Bulk Upload Processor (JSON-based) for mass data insertion.

Centralized Error Handling and Response Formatting.

Logging and auditing with created_by, modified_by, timestamps, and activity history.

🧩 Architecture & Code Design:

Multi-layered architecture:

API Layer: Controllers for handling endpoints.

Business Layer: Services and business rules.

Data Access Layer: Repository with ADO.NET and stored procedures.

Ensures loose coupling and separation of concerns.

Uses dependency injection and asynchronous data operations.

🧠 Database:

Fully normalized MySQL schema with foreign keys and constraints.

Over 20+ tables covering users, academic entities, and permissions.

Stored procedures encapsulate insert/update logic and validation.

🧰 Additional Functionalities:

Session Management System with user_session table for JWT tracking.

Export features for academic data.

Activity logging for all CRUD operations.
