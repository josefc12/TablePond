# 🐸 Table Pond
A web app that allows you to define your own tables, fields and values.  
Meant for small scale self-hosting or localhost deployment.  

### Features
1. Create tables and define columns that can be either text, number or link.
2. Editing functionality
3. Default authentication and autorization
4. Invitation system

Invitation system allows you to generate invitation codes that can be used by specific e-mail addresses to make a registration.  
All tables are private to the user that created them and can't be shared.

No emailing system had been implemented. **Send the generated code manually**.

### Installation/Setup

#### What you need:
1. NET 8.0
2. PostgreSQL

#### Setup PostgreSQL
1. CREATE USER tablepond WITH PASSWORD 'password123';
2. ALTER USER tablepond CREATEDB;

#### Installation:
1. Clone the project
2. Make sure you've installed all the nuget dependencies
3. Make sure you've setup the user and the password for the app in the database and changed the DefaultConnection string in appsettings.json if necessary
4. Run 'dotnet ef migrations add MyMigration' in the project folder
5. Run the project

#### First time setup:
1. Admin account had been created and is re-created if you delete it.
2. Login with 'admin' for email and Ap@T(^fhM3u*;mE5<:K_e for password
3. Go to admin -> Password and change this password.
4. Go to admin -> Create invite and enter your actual email
5. Copy the generated code
6. Logout of the admin account
7. Go to Register
8. Enter the code you copied (If you need to see it again you can find it on the Create invite page of the admin account)
9. Enter the email you generated the code for and choose your password.
10. Hit "Register"
11. You will be redirected to the login page and you now may login

Now you should be able to login with your own account and you can invite more people the same way, from your own account.

#### Basic usage:
1. Log in
2. Click on "Create a table" at the top-right side of the page.
3. Fill out the Create a table form. Add/Remove fields, choose their name and data type.
4. After hitting the Create button, you will be redirected to the table that you just created.
5. You can make changes to the table structure itself, by choosing "Edit table"
6. You can add records to the table by choosing "+ Add record"
7. If you have created a Link column, all valid web addresses in this column will be turned into a clickable link
8. By clicking the Edit button attached to each row, you can edit the values of the specific entry.
9. To create more tables, click on "Create a table" again.
10. All tables you create are accessible in the left hand side menu after logging in.

