CREATE TABLE "products" (
    "id" uuid NOT NULL PRIMARY KEY,
    "name" varchar(30) NOT NULL,
    "description" TEXT NULL,
    "price" int NOT NULL,
	"quantity" bigint NOT NULL
)
WITH (
  OIDS=FALSE
);
ALTER TABLE products
  OWNER TO postgres;
