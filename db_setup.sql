delimiter ;
SET FOREIGN_KEY_CHECKS=0; 
DROP TABLE Piece;
DROP TABLE Part;
DROP TABLE Instrument;
DROP TABLE Interpret;
DROP TABLE ArchiveType;
DROP TABLE Genre;
DROP TABLE Content;
DROP TABLE Genre_Piece;
DROP TABLE Content_Piece;
DROP TABLE Interpret_Piece;
DROP TABLE Part_Instrument;
SET FOREIGN_KEY_CHECKS=1;

CREATE TABLE Piece(piece_id int AUTO_INCREMENT, piece_title varchar(50), piece_comment varchar(50) default "", archivetype_id int default 1, archive_number int default 0,  primary key(piece_id));
CREATE TABLE Part(part_id int AUTO_INCREMENT, piece_id int, part_comment varchar(50), primary key(part_id));
CREATE TABLE Instrument(instrument_id int AUTO_INCREMENT, instrument_name varchar(50) UNIQUE, isPlayable bool, parent int, primary key(instrument_id));
CREATE TABLE Interpret(interpret_id int AUTO_INCREMENT , interpret_name varchar(50) UNIQUE, primary key(interpret_id) );
CREATE TABLE ArchiveType(archivetype_id int AUTO_INCREMENT, archivetype_name varchar(50) UNIQUE, primary key(archivetype_id));
CREATE TABLE Genre(genre_id int AUTO_INCREMENT, genre_name char(50) UNIQUE, primary key(genre_id));
CREATE TABLE Content(content_id int AUTO_INCREMENT, content_name varchar(50), primary key(content_id));

CREATE TABLE Genre_Piece(genre_id int, piece_id int, FOREIGN KEY (genre_id) REFERENCES Genre(genre_id), FOREIGN KEY (piece_id) REFERENCES Piece(piece_id), primary key(piece_id, genre_id));
CREATE TABLE Content_Piece(content_id int, piece_id int,FOREIGN KEY (content_id) REFERENCES Content(content_id),FOREIGN KEY (piece_id) REFERENCES Piece(piece_id), primary key(content_id, piece_id));
CREATE TABLE Interpret_Piece(interpret_id int, piece_id int, role varchar(50) default "", FOREIGN KEY (interpret_id) REFERENCES Interpret(interpret_id), FOREIGN KEY (piece_id) REFERENCES Piece(piece_id), primary key(interpret_id, piece_id));
CREATE TABLE Part_Instrument(part_id int, instrument_id int, FOREIGN KEY (part_id) REFERENCES Part(part_id), FOREIGN KEY (instrument_id) REFERENCES Instrument(instrument_id), primary key(part_id, instrument_id));

ALTER TABLE Instrument ADD FOREIGN KEY(parent) REFERENCES Instrument(instrument_id);
ALTER TABLE Part ADD FOREIGN KEY(piece_id) REFERENCES Piece(piece_id);
ALTER TABLE Piece ADD FOREIGN KEY(archivetype_id) REFERENCES ArchiveType(archivetype_id);

delimiter //
CREATE TRIGGER CheckInstrumentPlayableInsert BEFORE INSERT ON Part_Instrument for each row 
BEGIN
	DECLARE msg varchar(250);
    DECLARE t_isPlayable bool;
    
    SET t_isPlayable = (select max(isPlayable) from instrument where instrument_id = NEW.instrument_id);
    IF ( t_isPlayable is false) THEN 
		set msg = "Instrument is not playable";
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = msg ;
    END IF;
END//
delimiter ;

delimiter //
CREATE TRIGGER CheckInstrumentPlayableUpdate BEFORE UPDATE ON Part_Instrument for each row 
BEGIN
	DECLARE msg varchar(250);
    DECLARE t_isPlayable bool;
    
    SET t_isPlayable = (select max(isPlayable) from instrument where instrument_id = NEW.instrument_id);
    IF ( t_isPlayable is false) THEN 
		set msg = "Instrument is not playable";
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = msg ;
    END IF;
END//
delimiter ;

delimiter //
CREATE TRIGGER CheckNotPlayed BEFORE UPDATE ON Instrument for each row 
BEGIN
	DECLARE msg varchar(250);
    DECLARE amountPlayed int;
    
    IF (OLD.isPlayable is true AND NEW.isPlayable is false) THEN
		SET amountPlayed = (select count(instrument_id) from Part_Instrument where instrument_id = NEW.instrument_id);
        IF (amountPlayed > 0) THEN        
			set msg = "Instrument wird noch gespielt";
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = msg ;        
        END IF;
		
    END IF;
END//
delimiter ;

INSERT INTO ArchiveType(archivetype_id, archivetype_name) VALUES(1, "Unbekannt");
INSERT INTO ArchiveType(archivetype_id, archivetype_name) VALUES(2, "Konzertstücke");
INSERT INTO ArchiveType(archivetype_id, archivetype_name) VALUES(3, "Weihnachtsmusik");
INSERT INTO ArchiveType(archivetype_id, archivetype_name) VALUES(4, "Konzertmärsche");

INSERT INTO Piece(piece_id, piece_title) VALUES(1, "21 Guns");
INSERT INTO Piece(piece_id, piece_title) VALUES(2, "American Idiot");
INSERT INTO Piece(piece_id, piece_title) VALUES(3, "Gonna Fly Now");
INSERT INTO Piece(piece_id, piece_title) VALUES(4, "Joanna");
INSERT INTO Piece(piece_id, piece_title) VALUES(5, "Präsentiermarsch");
INSERT INTO Piece(piece_id, piece_title) VALUES(6, "Bergsteiger Marsch");

INSERT INTO CONTENT(content_id, content_name) VALUES (1, "Schwarze Mappe");
INSERT INTO CONTENT(content_id, content_name) VALUES (2, "Blaues Marschheft");
INSERT INTO CONTENT(content_id, content_name) VALUES (3, "Grünes Marschheft");

INSERT INTO Genre(genre_id, genre_name) VALUES(1, "Swing");
INSERT INTO Genre(genre_id, genre_name) VALUES(2, "Rock");
INSERT INTO Genre(genre_id, genre_name) VALUES(3, "Pop");
INSERT INTO Genre(genre_id, genre_name) VALUES(4, "Filmmusik");
INSERT INTO Genre(genre_id, genre_name) VALUES(5, "Marsch");
INSERT INTO Genre(genre_id, genre_name) VALUES(6, "Konzertmarsch");
INSERT INTO Genre(genre_id, genre_name) VALUES(7, "Choral");
INSERT INTO Genre(genre_id, genre_name) VALUES(8, "Weihnachtsmusik");

INSERT INTO Interpret(interpret_id, interpret_name) VALUES (1, "Jacob de Haahn");
INSERT INTO Interpret(interpret_id, interpret_name) VALUES (2, "John Williams");
INSERT INTO Interpret(interpret_id, interpret_name) VALUES (3, "Green day");
INSERT INTO Interpret(interpret_id, interpret_name) VALUES (4, "Marsch komponist xy");

INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(2,"Blasinstrument", false, Null);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(3,"Hohes Blech", false, 2);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(4,"Tiefes Blech", false, 2);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(5,"Schlagwerk",false,Null);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(6,"Drumset",true,5);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(7,"Snaredrum",true,5);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(8,"Basedrum",true,5);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(9,"Bongos",true,5);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(10,"Triangle", true,5);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(11,"Posaune",true,4);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, isPlayable, parent) VALUES(12,"Tuba",true,4);

INSERT INTO Part(part_id, piece_id) VALUES(1, 1);
INSERT INTO Part(part_id, piece_id) VALUES(2, 2);
INSERT INTO Part(part_id, piece_id) VALUES(3, 3);
INSERT INTO Part(part_id, piece_id) VALUES(4, 4);
INSERT INTO Part(part_id, piece_id) VALUES(5, 5);
INSERT INTO Part(part_id, piece_id) VALUES(6, 6);
INSERT INTO Part(part_id, piece_id) VALUES(7, 6);
INSERT INTO Part(part_id, piece_id) VALUES(8, 6);

INSERT INTO Genre_Piece(piece_id,genre_id) VALUES(1, 2);
INSERT INTO Genre_Piece(piece_id,genre_id) VALUES(2, 2);
INSERT INTO Genre_Piece(piece_id,genre_id) VALUES(3, 4);
INSERT INTO Genre_Piece(piece_id,genre_id) VALUES(4, 3);
INSERT INTO Genre_Piece(piece_id,genre_id) VALUES(5, 5);
INSERT INTO Genre_Piece(piece_id,genre_id) VALUES(6, 5);

INSERT INTO Content_Piece VALUES(1, 1);
INSERT INTO Content_Piece VALUES(1, 2);
INSERT INTO Content_Piece VALUES(1, 3);
INSERT INTO Content_Piece VALUES(1, 4);
INSERT INTO Content_Piece VALUES(2, 5);
INSERT INTO Content_Piece VALUES(2, 6);

INSERT INTO Interpret_Piece VALUES(1, 1,"Komponist");
INSERT INTO Interpret_Piece VALUES(2, 1,"Komponist");
INSERT INTO Interpret_Piece VALUES(3, 1,"Komponist");
INSERT INTO Interpret_Piece VALUES(3, 2,"Komponist");
INSERT INTO Interpret_Piece VALUES(1, 3,"Komponist");
INSERT INTO Interpret_Piece VALUES(2, 4,"Komponist");
INSERT INTO Interpret_Piece VALUES(4, 5,"Komponist");
INSERT INTO Interpret_Piece VALUES(4, 6,"Komponist");

INSERT INTO Part_Instrument Values(1, 11);
INSERT INTO Part_Instrument Values(2, 11);
INSERT INTO Part_Instrument Values(3, 11);
INSERT INTO Part_Instrument Values(4, 11);
INSERT INTO Part_Instrument Values(5, 11);
INSERT INTO Part_Instrument Values(6, 11);
INSERT INTO Part_Instrument Values(7, 11);
INSERT INTO Part_Instrument Values(8, 11);