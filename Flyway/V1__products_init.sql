CREATE TABLE "products" (
    "id" BIGSERIAL NOT NULL PRIMARY KEY,
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