DELETE FROM public."IssueAdviceLearningObjectSummary";
DELETE FROM public."Advice";

DELETE FROM public."ArrangeTaskContainerSubmissions";
DELETE FROM public."ArrangeTaskSubmissions";
DELETE FROM public."ChallengeSubmissions";
DELETE FROM public."QuestionSubmissions";
DELETE FROM public."NodeProgresses";
DELETE FROM public."LearningObjectFeedback";

DELETE FROM public."CourseEnrollment";
DELETE FROM public."Learners";

DELETE FROM public."Texts";
DELETE FROM public."Images";
DELETE FROM public."Videos";
DELETE FROM public."MetricRangeRules";
DELETE FROM public."BasicMetricCheckers";
DELETE FROM public."BasicNameCheckers";
DELETE FROM public."ChallengeHints";
DELETE FROM public."ChallengeFulfillmentStrategies";
DELETE FROM public."Challenges";
DELETE FROM public."QuestionAnswers";
DELETE FROM public."Questions";
DELETE FROM public."ArrangeTaskElements";
DELETE FROM public."ArrangeTaskContainers";
DELETE FROM public."ArrangeTasks";
DELETE FROM public."LearningObjects";
DELETE FROM public."LearningObjectSummaries";
DELETE FROM public."KnowledgeNodes";
DELETE FROM public."Lectures";
DELETE FROM public."Courses";

INSERT INTO public."Learners"(
	"Id", "StudentIndex", "VisualScore", "AuralScore", "ReadWriteScore", "KinaestheticScore")
	VALUES (1, 'SU-1-2021', 1, 2, 3, 4);
INSERT INTO public."Learners"(
	"Id", "StudentIndex", "VisualScore", "AuralScore", "ReadWriteScore", "KinaestheticScore")
	VALUES (2, 'SU-2-2021', 4, 3, 2, 1);
INSERT INTO public."Learners"(
	"Id", "StudentIndex", "VisualScore", "AuralScore", "ReadWriteScore", "KinaestheticScore")
	VALUES (3, 'SU-3-2021', 1, 4, 3, 2);

INSERT INTO public."Courses"(
	"Id")
	VALUES (1);

INSERT INTO public."CourseEnrollment"(
	"Id", "CourseId", "LearnerId")
	VALUES (1, 1, 1);
INSERT INTO public."CourseEnrollment"(
	"Id", "CourseId", "LearnerId")
	VALUES (2, 1, 2);
INSERT INTO public."CourseEnrollment"(
	"Id", "CourseId", "LearnerId")
	VALUES (3, 1, 3);

INSERT INTO public."Lectures"(
	"Id", "Name", "Description", "CourseId")
	VALUES (1, 'Meaningful Names', 'We rely on names in all segments of our software - through them we identify variables, functions, classes, libraries and applications. Given their prevelance, it is crucial we select meaningful names to describe our code elements. When considering a function'' name, we see that a clear name can free us from examining the function'' body. On the other hand, a mysterious name requires more time and mental effort to understand what the code element represents. In the worst case, a poor name leads us to false conclusions and significantly increases the development time while we figure out the truth. To avoid such situations, this lecture teaches good and bad naming practices.', 1);
	
INSERT INTO public."Lectures"(
	"Id", "Name", "Description", "CourseId")
	VALUES (2, 'Clear Functions', 'A common notion is that functions should be short and have a small number of code lines. The reasoning behind this notion is that such functions are focused and easy to understand and reuse. However, our goal shouldn''t be to create short functions. Instead, we should adhere to several good practices for creating clear functions, which will incidentally make many of our functions short. This lecture teaches these good practices for creating clean functions.', 1);
	
INSERT INTO public."Lectures"(
	"Id", "Name", "Description", "CourseId")
	VALUES (3, 'Class Cohesion', 'The learning objects for this lecture are in refinment, and a sample of this content can be found here: https://medium.com/clean-cadet/cohesion-in-software-two-perspectives-c992d503fcd8.', 1);
	
INSERT INTO public."Lectures"(
	"Id", "Name", "Description", "CourseId")
	VALUES (4, 'Class Coupling', 'The learning objects for this lecture are in refinment, and a sample of this content can be found here: https://medium.com/clean-cadet/coupling-between-software-modules-ee97899f6a31', 1);
	
	
	
--== Naming ==- FK Node	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "LectureId")
	VALUES (1, 'List the basic guidelines for creating meaningful names.', 0, 1);
	
--== Naming =- PK Node 1
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "LectureId")
	VALUES (2, 'Apply the "discard meaningless words" heuristic to formulate better names for code elements.', 1, 1);

--== Naming ==- PK Node 2
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "LectureId")
	VALUES (3, 'Apply the basic refactoring techniques for assigning meaningful names to code elements.', 1, 1);

--== Naming ==- CK Node	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "LectureId")
	VALUES (4, 'Understand the impact that meaningful and mysterious names have on the programmer'' productivity.', 2, 1);

--== Methods ==- FK Node	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "LectureId")
	VALUES (10, 'Analyze the idea that a function should "do one thing".', 0, 2);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId") -- Not sure what to do with this Description field.
	VALUES (30, 'Clean Function Definition', 10);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (31, 'Long Function Example', 10);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (32, 'Short Function Example', 10);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (33, 'One Thing', 10);
-- Methods - FK Node
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (30, 30);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (30, 'A function represents a *named* block of code that should address a meaningful task. In object-oriented programming, functions are often methods that define the behavior of some objects. The clean code principles that apply to functions are for the most part applicable to methods as well.

A clean function is focused *on completing a single task*. The task is described through the function''s header, including its name and the names of its parameters. Such focused functions often have a simple body that consists of easy-to-understand code. As a consequence, these functions usually have a small number of code lines.');
		
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (31, 31);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (31, 'https://i.ibb.co/dbthB3H/RS-Methods-Long.png', 'Take the time to describe at least three tasks that that the method "getFree" addresses. Try to identify one code segment that you would want to reuse in another context.');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (32, 32);
INSERT INTO public."ArrangeTasks"(
	"Id", "Text")
	VALUES (32, 'The following code presents an example of a clean function.

    public List<Doctor> GetSuitableDoctors(Operation operation){
    	List<Doctor> doctors = doctorRepository.FindAll();
    
    	List<Doctor> suitableDoctors = new ArrayList<>();
    	foreach(Doctor doctor in doctors){
    		if(IsCapable(doctor, operation.GetRequiredCapabilities())
    		    && IsAvailable(doctor, operation.GetTimeSlot())){
    			suitableDoctors.Add(doctor);
    		}
    	}
    
    	return suitableDoctors;
    }

Arrange the following requirements for software change, so that each requirement is tied to the function that we would most likely change to fulfill the new requirement.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (1, 32, 'None of the listed.');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (1, 1, 'Expand the data transfer object to show new metadata to a new client app.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (2, 32, 'FindAll');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (2, 2, 'We want to serialize data to a JSON file instead of an SQL database.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (3, 32, 'GetSuitableDoctors');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (3, 3, 'Out of the possible doctors, select the one that has the highest success rate for the given operation type, or the first in case of ties.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (4, 32, 'IsCapable');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (4, 4, 'For challenging operations, consider only physicians that have performed the operation at least once.');
INSERT INTO public."ArrangeTaskContainers"(
	"Id", "ArrangeTaskId", "Title")
	VALUES (5, 32, 'IsAvailable');
INSERT INTO public."ArrangeTaskElements"(
	"Id", "ArrangeTaskContainerId", "Text")
	VALUES (5, 5, 'Consider only physicians that are not in an important meeting at the time of the operation.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (33, 33);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (33, 'What does it mean to be focused on a single task? Ideally, a function that does one thing knows the steps required to complete the thing, without knowing the details of each step (i.e., without knowing the steps required to complete each step).

The method `getSuitableDoctors` knows the steps required to find suitable physicians by considering their capability and availability. This function does now know what makes a physician capable, nor the steps required to interact with some data storage. Likewise, `IsAvailable` knows the checks required to determine a physician'' availability (e.g., considering their work hours, other commitments, and vacation days). However, this function does not know the details of the logic for comparing the date and time ranges.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (34, 33);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (34, 'https://i.ibb.co/7WgctWZ/EN-Methods-One-Thing.png', '"A task" can be defined at different levels of abstraction - from "GetSuitableDoctors(operation)" to "Sum(a,b)". The logic required to address the task should follow the abstraction level, where high-level functions will consist mostly of function calls, while lower-level logic will work with the details of the SDK or library.');


--== Methods ==- PK Node 1
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "LectureId")
	VALUES (11, 'Apply the "extract method" refactoring technique to create shorter functions.', 1, 2);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId") -- Not sure what to do with this Description field.
	VALUES (40, 'Function Length Heuristic', 11);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (41, 'Function Length Challenge', 11);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description")
	VALUES (42, 'Function Length Solution'); -- Hidden

--== Methods ==- PK Node 2	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "LectureId")
	VALUES (12, 'Apply the "extract method" refactoring technique to create simpler functions.', 1, 2);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (43, 'Function Comments Heuristic', 12);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (44, 'Function Complexity Challenge', 12);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description")
	VALUES (45, 'Function Complexity Heuristic'); -- Hidden hint
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description")
	VALUES (46, 'Function Complexity Challenge Solution');
	
--== Methods ==- PK Node 3	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "LectureId")
	VALUES (13, 'Apply the strategies for shortening a function''s parameter list to create more focused functions.', 1, 3);--TODO: Move to 2 when translated
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (47, 'Parameter List Heuristic', 13);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (48, 'Parameter List Reduction Strategy', 13);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (49, 'Parameter List Challenge', 13);
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description")
	VALUES (50, 'Parameter List Challenge Solution');



-- Methods LO - PK Node 1
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (40, 40);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (40, 'The simplest heuristic for determining if a function does multiple things is to consider its length. While it is possible for a function with 20 lines of code to address multiple concerns, there is a high likelihood that a function with over 100 code lines does too much.
	
When dealing with long functions, we should identify regions of logically-related code that can be extracted into separate functions with meaningful names. Contemporary integrated development environments offer the *Extract Method* command that enable us to easily select and move some part of the code into a new function.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (41, 41);
INSERT INTO public."Challenges"(
	"Id", "Url", "Description", "TestSuiteLocation", "SolutionIdForeignKey")
	VALUES (41, 'https://github.com/Clean-CaDET/challenge-repository', 'To reiterate, our ultimate goal is not to make short functions. Rather, our functions become short as a consequence of adhering to various clean code principles. However, a function that surpases a few dozen code lines often is a good candidate for refactoring. Consider the "Methods/01. Small Methods" directory and extract logically-related code into separate methods, striving to define a meaninful name for each new method.', 'Methods.Small', 42);
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id", "ChallengeId")
	VALUES (3, 41);
INSERT INTO public."BasicMetricCheckers"(
	"Id")
	VALUES (3);

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (42, 42);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (42, 'https://youtu.be/79a8Zp6FBfU'); -- TODO: Record english
	
-- Methods LO - PK Node 2
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (43, 43);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (43, 'Comments and whitespace (e.g., two blank code lines) in a function'' body *often* (though not always) define a region of code that should be extracted into a separate method. Any existing comments can help formulate a meaningful name for the new function.');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (44, 44);
INSERT INTO public."Challenges"(
	"Id", "Url", "Description", "TestSuiteLocation", "SolutionIdForeignKey")
	VALUES (44, 'https://github.com/Clean-CaDET/challenge-repository', 'Complex functions require high mental effort to reason about and understand the control flow. Many aspects contribute to a function''s complexity - mysterious names, wide expressions, deep nesting. Consider the "Methods/02. Simple Methods" directory and refactor th functions to make them simpler and remove code duplication.', 'Methods.Simple', 46);
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id", "ChallengeId")
	VALUES (4, 44);
INSERT INTO public."BasicMetricCheckers"(
	"Id")
	VALUES (4);

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (45, 45);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (45, 'When we write small, focused functions, we can more easily abstract away the complexity of the function''s body behind a meaningful name. We can replace a sophisticated conidtional expression (e.g., the condition of a branch that has several different logical operators) with a function call that describes the intent behind the expression. Likewise, we can abstract away a loop or set of calculations behind a good name.
	
When we encounter code with deep nesting (e.g., a loop inside a branch, inside another loop, inside a try-catch block) we should consider if we can increase the readability of the code by extracting the inner most nested block or two into a separate function.');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (46, 46);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (46, 'https://youtu.be/-TF5b_R9JG4'); -- TODO: English
	
-- Methods LO - PK Node 3
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (47, 47);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (47, 'Razumljive funkcije treba da teže ka što manjem broju parametara. Ovaj broj je idealno nula, no svakako su funkcije sa jednim ili dva parametra prihvatljive i česte. Tako imamo funkcije koje prihvataju parametar da bi ga transformisali u novi objekat (npr. deserijalizacija stringa u objekat), funkcije koje ispituju neko svojstvo parametra, kao i funkcije koje izvršavaju komandu spram ulaznog podatka ili obrađuju prosleđeni događaj.

Sve preko dva parametra je ozbiljan kandidat za refaktorisanje. Takve funkcije obično rešavaju više zadataka i konfigurišu svoje ponašanje spram ulaznih podataka (tipičan primer su tzv. flag parametri bool tipa) što ih čini manje razumljivim. Izuzetak na ovo pravilo su konstruktori koji često prihvataju više podataka kako bi formirali složeni objekat.');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (48, 48);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (48, 'https://i.ibb.co/kDfx5DJ/a-RS-Methods-Params-Startegy.png', 'Kroz navedene strategije vidimo kako je broj parametra metode u interakciji sa klasama i može biti signal da li neku klasu treba uvesti ili izmeniti.');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (49, 49);
INSERT INTO public."Challenges"(
	"Id", "Url", "Description", "TestSuiteLocation", "SolutionIdForeignKey")
	VALUES (49, 'https://github.com/Clean-CaDET/challenge-repository', 'Redukcija broja parametra pozitivno utiče na razumevanje samog zaglavlja funkcije i informacije šta ona radi. Pored toga, redukcijom liste parametra često smanjujemo broj zadataka koje funkcija radi. U sklopu direktorijuma "Methods/03. Parameter Lists" primeni strategije za redukciju parametra i refaktoriši funkcije.', 'Methods.Params', 50);
INSERT INTO public."ChallengeFulfillmentStrategies"(
	"Id", "ChallengeId")
	VALUES (5, 49);
INSERT INTO public."BasicMetricCheckers"(
	"Id")
	VALUES (5);

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (50, 50);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (50, 'https://youtu.be/yKnxsH0CJzY');

-- HINTS
INSERT INTO public."ChallengeHints"(
	"Id", "Content", "LearningObjectSummaryId")
	VALUES (1, 'See if you can simplify the listed function by reorganizing the logic and extracting a meaningful subset of kod into a separate function.', 43);
INSERT INTO public."ChallengeHints"(
	"Id", "Content", "LearningObjectSummaryId")
	VALUES (2, 'Consider how you could simplify the method by reducing the depth of code nesting.', 45);
INSERT INTO public."ChallengeHints"(
	"Id", "Content", "LearningObjectSummaryId")
	VALUES (3, 'Do not forget to consider the number of code lines.', 40);
INSERT INTO public."ChallengeHints"(
	"Id", "Content", "LearningObjectSummaryId")
	VALUES (4, 'TODO: Too many paramters.', 48);
INSERT INTO public."ChallengeHints"(
	"Id", "Content", "LearningObjectSummaryId")
	VALUES (5, 'TODO: Too many params.', NULL);
	
-- Challenge rules
INSERT INTO public."MetricRangeRules"(
	"Id", "MetricName", "FromValue", "ToValue", "HintId", "ClassMetricCheckerForeignKey", "MethodMetricCheckerForeignKey")
	VALUES (1, 'MELOC', 1, 18, 1, NULL, 3);
INSERT INTO public."MetricRangeRules"(
	"Id", "MetricName", "FromValue", "ToValue", "HintId", "ClassMetricCheckerForeignKey", "MethodMetricCheckerForeignKey")
	VALUES (2, 'MELOC', 1, 12, 3, NULL, 4);
INSERT INTO public."MetricRangeRules"(
	"Id", "MetricName", "FromValue", "ToValue", "HintId", "ClassMetricCheckerForeignKey", "MethodMetricCheckerForeignKey")
	VALUES (3, 'CYCLO', 1, 5, 2, NULL, 4);
INSERT INTO public."MetricRangeRules"(
	"Id", "MetricName", "FromValue", "ToValue", "HintId", "ClassMetricCheckerForeignKey", "MethodMetricCheckerForeignKey")
	VALUES (4, 'NOP', 0, 1, 4, NULL, 5);
INSERT INTO public."MetricRangeRules"(
	"Id", "MetricName", "FromValue", "ToValue", "HintId", "ClassMetricCheckerForeignKey", "MethodMetricCheckerForeignKey")
	VALUES (5, 'NMD', 0, 2, 5, 5, NULL);
	
	
--== Methods ==- CK Node
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "LectureId")
	VALUES (14, 'Understand the conseuqnces of maintaining a code base that primarily consists of functions that perform a single task.', 2, 2);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (60, 'Refactoring Extract Method', 14);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (61, 'Function LINQ', 14);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (62, 'Function Hierarchy', 14);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (63, 'Refactoring One Thing', 14);

INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (64, 'Function Big Picture', 14);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (65, 'Function Recap', 14);
	
-- Methods LO - CK Node
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (60, 60);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (60, 'https://youtu.be/2goLaolzEV0'); -- TODO: Translate
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (61, 61);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (61, 'https://i.ibb.co/nm0PKLY/EN-LINQ.png', 'Consider how the "IsActive" function helps achieve the abstraction and encapsulation of the underlying logic.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (62, 62);
INSERT INTO public."Images"(
	"Id", "Url", "Caption")
	VALUES (62, 'https://i.ibb.co/6n4jhzk/EN-Hierarchy.png', 'We stated that clean methods work towards acomplishing a single task. But, how do we define a task? Consider one function of each color in the image above and define what it does in a sentence. Can you differentiate the level of abstraction between your descriptions?');
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (64, 63);
INSERT INTO public."Videos"(
	"Id", "Url")
	VALUES (64, 'https://youtu.be/JQPIh0VcLK4'); -- TODO: Translate
	
INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (65, 64);
INSERT INTO public."Texts"(
	"Id", "Content")
	VALUES (65, 'Imagine an application that has been in development for some time. It contains many classes that consist of multiple methods, most of which are focused on a single task. The application supports different uses cases by coordinating these small bits of logic.
A new requirement arrives that warrants a change to the logic that performs a specific task. The programmer starts with the affected user control (e.g., button or form) and examines the code that works with this control. They then recursively apply the following algorithm:

1. Examine the method''s body, looking for significant names (e.g., of method invocations) that lead to the logic that needs to change.
2. For each name, analyze its meaning. If the name is significant, jump to the underlying code (using IDE shortcuts) and go to step 1. Otherwise ignore the name.

Following this algorithm, the programmer avoids analyzing hundreds or thousands of code lines and instead interprets a handful of names. When they arrive at their destination, the required change is minimal, reducing the chance of regression. The programmer completes the task, having spent little of their mental energy.

Now imagine a similar application that is built from huge functions with hundreds of code lines filled with mysterious names. The required change can be made after much research. The programmer must invest a lot of effort to maintain a high focus while they traverse the sophisticated logic. The change is risky and a bug can be easily introduced without a high mental investment. At the end of this journey, the programmer is exhausted and burdened by thoughts of future bug reports.

Writing clean functions is more difficult than building code that only works. On the other hand, reading poorly written functions is harder than exploring small, focused functions with a clear intent. The question becomes - which aspect is more important? For any software that has been in development for more than a few months (or even weeks), most of the programmer''s time is spent reading code. Therefore, we should reduce the burden of reading as much as possible.');

INSERT INTO public."LearningObjects"(
	"Id", "LearningObjectSummaryId")
	VALUES (66, 65);
INSERT INTO public."Questions"(
	"Id", "Text")
	VALUES (66, 'Select all claims you consider true:');
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (20, 'We should strive to make functions with a few lines of code.', false, 'The claim is not correct. Firstly, this is not a goal but a likely coincidence that occurs when adhering to other best practices. Secondly, it is possible to go too far with functional decomposition, resulting in classes that have hundreds of shallow, single-line methods with little difference between their name and body. A function should be focused on solving a task, but it should also encapsulate nontrivial logic.', 66);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (21, 'A function should do one thing.', true, 'The claim is correct. A function that performs a single task does one thing.', 66);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (22, 'Conditional expressions with multiple logical operators are good candidates for method extraction.', true, 'The claim is correct. A sophisticated conditional expression often has an unclear intent and extracting it into a separate method forces us to define the semantic meaning behind the check.', 66);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (23, 'Methods should have four or fewere parameters.', false, 'The claim is not rigorous enough. Ideally, a method should have zero or one parameters, and two parameters are also acceptable. Everything above that is a candidate for refactoring, and there will be situations (e.g., constructors) where more parameters might be required.', 66);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (24, 'Focused functions with a clear name adhere to the OOP principle of abstraction.', true, 'The claim is correct. A clear function name abstracts away the logic of the function''s body, which can help the programmer conclude if they should examine a function''s body or skip it.', 66);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (25, 'Creating clean functions is challenging, while examining a lot of focused functions is comparatively simple.', true, 'The claim is correct. It often takes additional effort to decompose complex logic and then identify and properly name subsets of that logic.', 66);
INSERT INTO public."QuestionAnswers"(
	"Id", "Text", "IsCorrect", "Feedback", "QuestionId")
	VALUES (26, 'When we write clean functions, our clients can read and understand our code.', false, 'The claim is not correct. Clean functions that describe business logic should be readable by our clients, as they mostly consist of names (e.g., objects and method invocations). However, there is a significant part of an application''s code that is technically-focused - HTTP request processing, database interaction, cryptography, message queues are all valid concerns not understood by most of our clients.', 66);


--=== CODE QUALITY ADVICE
INSERT INTO public."Advice"(
	"Id", "IssueType")
	VALUES (1, 'LONG_METHOD');
	
INSERT INTO public."IssueAdviceLearningObjectSummary"(
	"AdviceId", "SummariesId")
	VALUES (1, 30);
INSERT INTO public."IssueAdviceLearningObjectSummary"(
	"AdviceId", "SummariesId")
	VALUES (1, 33);
INSERT INTO public."IssueAdviceLearningObjectSummary"(
	"AdviceId", "SummariesId")
	VALUES (1, 40);
INSERT INTO public."IssueAdviceLearningObjectSummary"(
	"AdviceId", "SummariesId")
	VALUES (1, 43);
INSERT INTO public."IssueAdviceLearningObjectSummary"(
	"AdviceId", "SummariesId")
	VALUES (1, 45);
INSERT INTO public."IssueAdviceLearningObjectSummary"(
	"AdviceId", "SummariesId")
	VALUES (1, 60);
INSERT INTO public."IssueAdviceLearningObjectSummary"(
	"AdviceId", "SummariesId")
	VALUES (1, 62);
INSERT INTO public."IssueAdviceLearningObjectSummary"(
	"AdviceId", "SummariesId")
	VALUES (1, 63);

INSERT INTO public."KnowledgeComponents"(
    "Name", "KnowledgeComponentId")
VALUES ('Clean functions', NULL);
INSERT INTO public."KnowledgeComponents"(
    "Name", "KnowledgeComponentId")
VALUES ('Parameter list reduction', 1);
INSERT INTO public."KnowledgeComponents"(
    "Name", "KnowledgeComponentId")
VALUES ('Clean names', 1);