CREATE TABLE "categories" (
    "id" uuid NOT NULL PRIMARY KEY,
    "name" varchar(30) NOT NULL
)
WITH (
  OIDS=FALSE
);
ALTER TABLE categories
  OWNER TO postgres;

ALTER TABLE products ALTER COLUMN quantity SET DEFAULT 0;

ALTER TABLE products ADD categoryId uuid;
ALTER TABLE products
   ADD CONSTRAINT FK_Product_Category
   FOREIGN KEY (categoryId)
   REFERENCES categories(id);