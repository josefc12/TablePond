# 🐸 Data Pond
A web app that allows you to define your own tables, fields and values.  
Meant for small scale self-hosting or localhost deployment.  

### Features
1. Create tables and define columns that can be either text, number or link.
2. Editing functionality
3. Default authentication and autorization
4. Invitation system

Invitation system allows you to generate invitation codes that can be used by specific e-mail addresses to make a registration.  
All tables are private to the user that created them and can't be shared.

No emailing system had been implemented. Send the generated code manually.

### Installation/Setup

#### What you need:
1. NET 8.0
2. A database of your choosing. Project is setup for PostgreSQL by default (localhost:5432, datapond, password123)

#### Installation:
1. Clone the project
2. Make sure you've setup your database and the DefaultConnection string in appsettings.json