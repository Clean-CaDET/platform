-- Currently we assume a one-to-many relationship regarding KN prerequisites. This might change to many-to-many or one-to-one.
DELETE FROM public."KnowledgeNodes";

INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (1, 'Navedi osnovne vodilje za definisanje značajnih imena.', 0, NULL, 1);
	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (2, 'Primeni heuristiku odbacivanja beznačajnih reči radi formiranje boljih imena u kodu.', 1, 1, 1);
	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (3, 'Primeni osnovne tehnike refaktorisanja za formiranje boljih imena u kodu.', 1, 2, 1);
	
INSERT INTO public."KnowledgeNodes"(
	"Id", "LearningObjective", "Type", "KnowledgeNodeId", "LectureId")
	VALUES (4, 'Razumi uticaj jasnih i misterioznih imena u kodu na rad programera.', 2, 3, 1);