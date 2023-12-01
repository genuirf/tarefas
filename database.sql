# mysql 8.0

CREATE schema `tarefas`;


CREATE TABLE `grupo` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Descricao` varchar(45) DEFAULT NULL,
  `ordem` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `tarefa` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ordem` int NOT NULL DEFAULT '0',
  `Titulo` varchar(45) DEFAULT NULL,
  `Descricao` varchar(1024) DEFAULT NULL,
  `DataCadastro` date DEFAULT NULL,
  `DataConclusao` date DEFAULT NULL,
  `grupo_Id` int NOT NULL,
  `concluido` tinyint(1) DEFAULT '0',
  `arquivado` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `tarefa_fk1_idx` (`grupo_Id`),
  CONSTRAINT `tarefa_fk1` FOREIGN KEY (`grupo_Id`) REFERENCES `grupo` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
