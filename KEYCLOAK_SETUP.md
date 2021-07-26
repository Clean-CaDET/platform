# Setup Keycloak

This document provides steps that need to be taken in order to successfully use Keycloak as IAM.

Setup contains 9 steps:
 - Adding a client.
 - Adding URIs.
 - Adding a client scope.
 - Adding mapper in client scope.
 - Adding mapper in client.
 - Adding client scopes to client.
 - Adding roles.
 - Mapping roles to users.
 - Adding default roles to users.

All these steps are required in order to run Keycloak successfully!

## Login as admin

In order to reach admin console go to: http://localhost:8080/keycloak/auth/admin/
After login with admin credentials (username: 'admin', password 'admin' in dev) you will be able to manage Keycloak settings.

## Adding a client

Just click on 'Clients'->'Create' and enter 'Client ID' ('smart-tutor-client' for example).

## Adding URIs

Go to 'smart-tutor-client' (or one that you created in step before) and add 'Root URL' ('http://127.0.0.1:8080'), 'Valid redirect URIs' ('http://127.0.0.1:8080/*'),
'Admin URL' ('http://localhost:8080/') and 'Web Origins' (just type '+'). On that page also turn on 'Implicit flow enabled'!

## Adding a client scope

Go to 'Client Scopes' and create new scope ('smart-tutor-scope' for example). Affter that go to 'Mappers' in scope you just created and add new 'Mapper'
(you can call it 'smart-tutor-audience'). Set 'Mapper Type' to 'Audience' set 'Included Client Audience' to client that is created in second step (in our example
'smart-tutor-client').

## Adding mapper in client
Go to client, after that go to 'Mappers' tab and create new mapper. Set 'Mapper Type' to 'User Client Role' you can call it as you wish but 'Token Claim Name' must be 'role'.
Choose client that you created and 'Claim JSON Type' to 'String'.

## Adding client scopes to client
Go to client then to 'Client Scopes' tab and choose scope that you want to add from 'Avaliable Client Scopes'.

## Adding roles in client
Go to client that is created in second step and click on 'Roles' tab.
Add new roles ('Learner' and 'Professor' for example).
Of course you will need to have some users registred in Keycloak (you can do that manually) if u want to map 'Roles' to 'Users'.

## Mapping roles to users
Affter adding some 'Users' you will be able to select 'Roles' for them.
Go to 'Users' in 'Master Realm', then choose one 'User', after that go to 'Role Mapping' tab. Select your 'Client' and choose which 'Role' you want to add to 'User'.

## Adding default roles to users
If u want to add default 'Roles' after 'User' registration to that user just go to 'Roles'->'Default Roles' choose 'Client' and after that choose 'Default Role'.

