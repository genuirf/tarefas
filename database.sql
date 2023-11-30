# mysql 8.0

CREATE schema `tarefas`;


CREATE TABLE `grupo` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Descricao` varchar(45) DEFAULT NULL,
  `ordem` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
