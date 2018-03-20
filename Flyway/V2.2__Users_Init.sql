CREATE TABLE "users" (
    "id" uuid NOT NULL PRIMARY KEY,
    "fullName" varchar(30) NOT NULL,
	"email" varchar(255) NOT NULL,
	"password" varchar(255) NOT NULL,
	"rights" smallint NOT NULL
)
WITH (
  OIDS=FALSE
);
ALTER TABLE users
  OWNER TO postgres;
