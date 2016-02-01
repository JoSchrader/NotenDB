SET FOREIGN_KEY_CHECKS=0; 
DROP TABLE Piece;
DROP TABLE Part;
DROP TABLE Instrument;
DROP TABLE Interpret;
DROP TABLE ArchiveType;
DROP TABLE Genre;
DROP TABLE Content;
DROP TABLE Piece_Genre;
DROP TABLE Content_Piece;
DROP TABLE Interpret_Piece;
DROP TABLE Part_Instrument;
SET FOREIGN_KEY_CHECKS=1; 

CREATE TABLE Piece(piece_id int, piece_title varchar(50), primary key(piece_id));
CREATE TABLE Part(part_id int, piece_id int, primary key(part_id));
CREATE TABLE Instrument(instrument_id int, instrument_name varchar(50), parent int, primary key(instrument_id));
CREATE TABLE Interpret(interpret_id int, interpret_name varchar(50), primary key(interpret_id));
CREATE TABLE ArchiveType(archivetype_id int, archivetype_title varchar(50), primary key(archivetype_id));
CREATE TABLE Genre(genre_id int , genre_name char(50), primary key(genre_id));
CREATE TABLE Content(content_id int , content_name varchar(50), primary key(content_id));

CREATE TABLE Piece_Genre(piece_id int REFERENCES Piece(piece_id), genre_id int REFERENCES Genre(genre_id));
CREATE TABLE Content_Piece(content_id int REFERENCES Content(content_id), piece_id int REFERENCES Piece(piece_id));
CREATE TABLE Interpret_Piece(interpret_id int REFERENCES Interpret(interpret_id), piece_id int REFERENCES Piece(piece_id));
CREATE TABLE Part_Instrument(part_id int REFERENCES Part(part_id), instrument_id int REFERENCES Instrument(instrument_id));

ALTER TABLE Instrument ADD FOREIGN KEY(parent) REFERENCES Instrument(instrument_id);
ALTER TABLE Part ADD FOREIGN KEY(piece_id) REFERENCES Piece(piece_id);


INSERT INTO Piece(piece_id, piece_title) VALUES(1, "21 Guns");
INSERT INTO Piece(piece_id, piece_title) VALUES(2, "American Idiot");
INSERT INTO Piece(piece_id, piece_title) VALUES(3, "Gonna Fly Now");
INSERT INTO Piece(piece_id, piece_title) VALUES(4, "Joanna");
INSERT INTO Piece(piece_id, piece_title) VALUES(5, "Präsentiermarsch");
INSERT INTO Piece(piece_id, piece_title) VALUES(6, "Bergsteiger Marsch");

INSERT INTO CONTENT(content_id, content_name) VALUES (1, "Schwarze Mappe");
INSERT INTO CONTENT(content_id, content_name) VALUES (2, "Blaues Marschheft");
INSERT INTO CONTENT(content_id, content_name) VALUES (3, "Grünes Marschheft");

INSERT INTO ArchiveType(archivetype_id, archivetype_title) VALUES(1, "Konzertstücke");
INSERT INTO ArchiveType(archivetype_id, archivetype_title) VALUES(2, "Weihnachtsmusik");
INSERT INTO ArchiveType(archivetype_id, archivetype_title) VALUES(3, "Konzertmärsche");

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

INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(1,"Blasinstrument",Null);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(2,"Hohes Blech",1);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(3,"Tiefes Blech",1);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(4,"Schlagzeug",Null);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(5,"Drumset",4);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(6,"Snaredrum",4);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(7,"Basedrum",4);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(8,"Bongos",4);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(9,"Triangle",4);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(10,"Posaune",3);
INSERT INTO INSTRUMENT(instrument_id, instrument_name, parent) VALUES(11,"Tuba",3);

INSERT INTO Part(part_id, piece_id) VALUES(1, 1);
INSERT INTO Part(part_id, piece_id) VALUES(2, 2);
INSERT INTO Part(part_id, piece_id) VALUES(3, 3);
INSERT INTO Part(part_id, piece_id) VALUES(4, 4);
INSERT INTO Part(part_id, piece_id) VALUES(5, 5);
INSERT INTO Part(part_id, piece_id) VALUES(6, 6);
INSERT INTO Part(part_id, piece_id) VALUES(7, 6);
INSERT INTO Part(part_id, piece_id) VALUES(8, 6);

INSERT INTO Piece_Genre VALUES(1, 2);
INSERT INTO Piece_Genre VALUES(2, 2);
INSERT INTO Piece_Genre VALUES(3, 4);
INSERT INTO Piece_Genre VALUES(4, 3);
INSERT INTO Piece_Genre VALUES(5, 5);
INSERT INTO Piece_Genre VALUES(6, 5);

INSERT INTO Content_Piece VALUES(1, 1);
INSERT INTO Content_Piece VALUES(1, 2);
INSERT INTO Content_Piece VALUES(1, 3);
INSERT INTO Content_Piece VALUES(1, 4);
INSERT INTO Content_Piece VALUES(2, 5);
INSERT INTO Content_Piece VALUES(2, 6);

INSERT INTO Interpret_Piece VALUES(3, 1);
INSERT INTO Interpret_Piece VALUES(3, 2);
INSERT INTO Interpret_Piece VALUES(1, 3);
INSERT INTO Interpret_Piece VALUES(2, 4);
INSERT INTO Interpret_Piece VALUES(4, 5);
INSERT INTO Interpret_Piece VALUES(4, 6);

INSERT INTO Part_Instrument Values(1, 4);
INSERT INTO Part_Instrument Values(2, 10);
INSERT INTO Part_Instrument Values(3, 11);
INSERT INTO Part_Instrument Values(4, 1);
INSERT INTO Part_Instrument Values(5, 4);
INSERT INTO Part_Instrument Values(6, 4);
INSERT INTO Part_Instrument Values(7, 5);
INSERT INTO Part_Instrument Values(8, 6);

Select piece_title, interpret_name, genre_name, instrument_name from interpret natural join Interpret_Piece natural join piece natural join Piece_Genre natural join genre natural join part natural join part_instrument natural join instrument;
