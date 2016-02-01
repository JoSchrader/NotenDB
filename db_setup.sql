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

CREATE TABLE Piece(piece_id int AUTO_INCREMENT, title varchar(50), primary key(piece_id));
CREATE TABLE Part(part_id int AUTO_INCREMENT, piece_id int, primary key(part_id));
CREATE TABLE Instrument(instrument_id int, name varchar(50), parent int, primary key(instrument_id));
CREATE TABLE Interpret(interpret_id int AUTO_INCREMENT, name varchar(50), primary key(interpret_id));
CREATE TABLE ArchiveType(archive_id int AUTO_INCREMENT, title varchar(50), primary key(archive_id));
CREATE TABLE Genre(genre_id int AUTO_INCREMENT, title char(50), primary key(genre_id));
CREATE TABLE Content(content_id int AUTO_INCREMENT, title varchar(50), primary key(content_id));

CREATE TABLE Genre_Piece(genre_id int REFERENCES Genre(genre_id), piece_id int REFERENCES Piece(piece_id));
CREATE TABLE Content_Piece(content_id int REFERENCES Content(content_id), piece_id int REFERENCES Piece(piece_id));
CREATE TABLE Interpret_Piece(genre_id int REFERENCES Genre(genre_id), piece_id int REFERENCES Piece(piece_id));
CREATE TABLE Part_Instrument(part_id int REFERENCES Part(part_id), instrument_id int REFERENCES Instrument(instrument_id));

ALTER TABLE Instrument ADD FOREIGN KEY(parent) REFERENCES Instrument(instrument_id);
ALTER TABLE Part ADD FOREIGN KEY(piece_id) REFERENCES Piece(piece_id);

INSERT INTO Piece(title) VALUES("Hard as Rock");
INSERT INTO Piece(title) VALUES("21 Guns");
INSERT INTO Piece(title) VALUES("American Idiot");

INSERT INTO CONTENT(title) VALUES ("Schwarze Mappe");
INSERT INTO CONTENT(title) VALUES ("Blaues Marschheft");
INSERT INTO CONTENT(title) VALUES ("Grünes Marschheft");

INSERT INTO ArchiveType(title) VALUES("Konzertstücke");
INSERT INTO ArchiveType(title) VALUES("Weihnachtsmusik");
INSERT INTO ArchiveType(title) VALUES("Konzertmärsche");

INSERT INTO Genre(title) VALUES("Swing");
INSERT INTO Genre(title) VALUES("Rock");
INSERT INTO Genre(title) VALUES("Pop");
INSERT INTO Genre(title) VALUES("Marsch");
INSERT INTO Genre(title) VALUES("Konzertmarsch");
INSERT INTO Genre(title) VALUES("Choral");
INSERT INTO Genre(title) VALUES("Weihnachtsmusik");

INSERT INTO Interpret(name) VALUES ("Jacob de Haahn");
INSERT INTO Interpret(name) VALUES ("John Williams");

INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(1,"Blasinstrument",Null);
INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(2,"Hohes Blech",1);
INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(3,"Tiefes Blech",1);
INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(4,"Schlagzeug",Null);
INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(5,"Drumset",4);
INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(6,"Snaredrum",4);
INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(7,"Basedrum",4);
INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(8,"Bongos",4);
INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(9,"Triangle",4);
INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(10,"Posaune",3);
INSERT INTO INSTRUMENT(instrument_id,name,parent) VALUES(11,"Tuba",3);

INSERT INTO Part(piece_id) VALUES(1);
INSERT INTO Part(piece_id) VALUES(2);