CREATE TABLE "products" (
    "id" int NOT NULL PRIMARY KEY UNIQUE,
    "name" varchar(30) NOT NULL,
    "description" TEXT NULL,
    "price" int NOT NULL
)
WITH (
  OIDS=FALSE
);
ALTER TABLE products
  OWNER TO postgres;

 INSERT INTO Products (id, name, price) VALUES(0, 'Bread', 10);