

# 🎓 University Management System (UMS)

## 🧩 Overview

* A complete **University Management System** built using **ASP.NET Core Web API** and **Plain ADO.NET**.
* Designed with a **clean layered architecture** for scalability, maintainability, and modularity.
* Integrates **MySQL** as the backend database with **stored procedures** implementing core business logic.
* Implements **authentication, authorization, and permission-based access control** across all entities.

---

## ⚙️ Technology Stack

* **Backend:** ASP.NET Core Web API
* **Data Access:** Plain ADO.NET with Stored Procedures
* **Database:** MySQL 8.0
* **Architecture:** Layered (API Layer → Business Layer → Data Layer)
* **Authentication:** JWT (JSON Web Tokens)
* **Authorization:** Role & Activity-based Permission Filters

---

## 🔐 Authentication & Authorization

* Secure **JWT-based authentication** with refresh and access token mechanism.
* **Session management** via `user_session` table for tracking and revoking tokens.
* **Role-based access control (RBAC)** using activity-profile mappings.
* Custom **permission filters** implemented at controller and action levels.
* Integrated **password hashing** and validation for all users.

---

## 👥 User Management

* CRUD operations for **Users**, **Profiles**, and **Permissions**.
* Separate registration and management flows for **Admins**, **Teachers**, and **Students**.
* Profile picture upload and linking through the **Profile Picture Processor**.
* Maintains audit trail with `created_by`, `modified_by`, and timestamps.

---

## 🏫 Academic Management

* CRUD operations for **Departments**, **Programs**, and **Levels**.
* Comprehensive **Course Management** with prerequisites and semester offerings.
* **Teacher Assignment Module** for linking instructors with offered courses.
* Tracks **Student Course History** including grades, GPA, and retakes.

---

## 🎓 Enrollment & Registration

* Complete **Enrollment Management System** for course registration.
* Supports **enrollment status flow**: pending → approved → dropped/withdrawn.
* **Admin approval and rejection** tracking with reason and timestamps.
* Automatic logging in **Enrollment Status History** for audit and transparency.

---

## 📏 Credit Policy System

* Implements **Credit Limit Policy** for academic credit hour restrictions.
* Supports **department**, **program**, or **level-specific** policies.
* **Override system** for students with approved exceptions and expiration handling.

---

## 🖼️ Media Management

* Integrated **Profile Image Processor** for upload, resize, and metadata tracking.
* Stores **file name**, **path**, **type**, and **size** with reference in `profile_pictures` table.
* Enforces **unique active image per user** through database constraints.

---

## 📦 Data Management & Utilities

* **Bulk Upload Processor** for importing data via JSON input.
* Centralized **error handling** and **standardized API responses**.
* Integrated **logging and auditing** for CRUD operations.
* Built-in **export and reporting** capabilities for admin users.

---

## 🧠 Database Design

* **Fully normalized MySQL schema** with strong referential integrity.
* Includes 20+ interrelated tables covering all university entities.
* Enforced constraints, foreign keys, and validation checks.
* Stored procedures encapsulate all business logic for CRUD operations.

---

## 🧩 Architecture Design

* **Layered architecture pattern** ensuring modularity and clean separation of concerns.

  * **API Layer:** Controllers managing HTTP requests and responses.
  * **Business Layer:** Services containing core business logic.
  * **Data Access Layer:** Repositories using ADO.NET and stored procedures.
* Uses **Dependency Injection** for services and repositories.
* Fully **asynchronous** database interaction for performance optimization.

---

## 🧰 Additional Features

* **Session management system** for secure token lifecycle control.
* **Permission-based filtering** for fine-grained access.
* **Activity logging** for user operations and audit tracking.
* **Bulk data processing utilities** for efficient data import/export.
* **Complete CRUD implementations** for all university entities.

---

## 🧾 Modules Implemented

* Academic Program Management
* Authentication & Authorization
* Course & Course History Management
* Course Offering & Prerequisite Management
* Credit Limit Policy and Override Management
* Department Management
* Enrollment Management
* Level & Profile Management
* Profile Picture Handling
* Admin, Teacher, and Student Registration
* Semester Management
* Teacher Assignment Management
* User & Permission Management

---




