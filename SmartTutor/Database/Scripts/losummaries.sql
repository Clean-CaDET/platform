DELETE FROM public."LearningObjectSummaries";

-- Naming - FK Node
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (1, 'Definition', 1);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (2, 'Basic Example', 1);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (3, 'Word Type Heuristic', 1);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (4, 'Example', 1);

-- Naming - PK Node 1
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (5, 'Algorithm', 2);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (9, 'Noise Word Heuristic', 2);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (10, 'Challenge Noise', 2);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description") -- Hidden
	VALUES (11, 'Challenge Noise Solution');

-- Naming - PK Node 2
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (6, 'Challenge Meaning', 3);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (7, 'Magic Numbers Heuristic', 3);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description") -- Hidden
	VALUES (8, 'Challenge Meaning Solution');
	

-- Naming - CK Node	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (12, 'Complex Example', 4);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (13, 'Programmer Motivation', 4);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (14, 'Conclusion', 4);
	
INSERT INTO public."LearningObjectSummaries"(
	"Id", "Description", "KnowledgeNodeId")
	VALUES (15, 'Recap', 4);

