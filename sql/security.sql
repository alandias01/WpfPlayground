
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


SSMS
 Create Login
  Server->Security->Logins->RightClk New Login
   General tab
    SQL Server Auth
   User Mapping
    select DB, bottom window allows selecting roles 

How to give permissions to individual vs all
How a service uses security to connect to SQL
 Windows and sql security

*/

CREATE LOGIN app_login WITH PASSWORD = 'StrongP@ssw0rd!';   --creates a server-level identity (authentication). SQL Server Authentication (username + password).

USE Database1;
CREATE USER app_user FOR LOGIN app_login;                   --maps server login to specific database user (authorization)

USE Database2;
CREATE USER app_user FOR LOGIN app_login;

CREATE ROLE gp_account_role;                                -- Create a role
ALTER ROLE gp_account_role ADD MEMBER app_user;             -- Add a user to the role
GRANT EXECUTE ON mystoredproc TO gp_account_role            --gives permission on a securable (the sp) to a role

  --defines a database role you can assign users to.

