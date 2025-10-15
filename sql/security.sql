
/*

SQL Command to give Permissions (how)
 GRANT, DENY, REVOKE
 SQL Server command to give (grant,deny, or revoke) permissions on a securable to a principal (logins, users, roles)
 GRANT <permission type> ON <securable> TO <principal>;
 ex. GRANT EXECUTE ON mystoredproc TO gp_account_role -> A user in this role can execute the stored proc

Permission type
 Execute (for SP's, functions)
 SELECT, INSERT, UPDATE, DELETE (for tables/views)
 ALTER, CONTROL, REFERENCES

Securables (what you secure)
 What you secure with permissions (server, database, schema, objects like (tables, views, stored procedures, functions)). 

Principals (who)
 Who's given permission on a securable
 logins, users, roles
 Login = server-level identity (who can connect).
  Authenticates you to the SQL Server instance.
 User = database-level identity (who can access a database).
  Authorizes you inside a specific database.
 Role = group of permissions (easier to manage).
 
 You separate authentication (login) from authorization (user + role + permissions)
 A login by itself cannot directly access a database until it is mapped to a user in that database.
 Therefore, if you want one login to access 10 databases, you must create 10 users (one in each database) 
 mapped to that login.

 At the server level, principals are logins and server roles.
 At the database level, principals are users, database roles, and application roles.
 Principals interact with securables through permissions.

 Server roles
 database roles 



*/

CREATE LOGIN app_login WITH PASSWORD = 'StrongP@ssw0rd!';   --craetes a server-level identity (authentication).

USE Database1;
CREATE USER app_user FOR LOGIN app_login;                   --maps login to specific database (authorization)
EXEC sp_addrolemember 'db_datareader', 'app_user';

USE Database2;
CREATE USER app_user FOR LOGIN app_login;
EXEC sp_addrolemember 'db_datareader', 'app_user';

GRANT EXECUTE ON mystoredproc TO gp_account_role            --gives permission on a securable (the sp) to a role

  --defines a database role you can assign users to.