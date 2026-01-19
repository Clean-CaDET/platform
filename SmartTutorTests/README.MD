Integration tests require a few manual steps:
1. Create the "smart-tutor-test" database and ensure its correct configuration, following the code in the Integration/TutorTestFactory class.
2. Run the commands from Integration/Scripts/dbschema.sql on the new database.
3. Run the tests.

If the schema for the data model changes, you need to run the "script-dbcontext" command in the Package manager console and copy the script to the dbschema.sql file.
Afterwards, drop the public schema from the database and repeat step 2. to reset the test database.

Eventually we will automate these manual steps.