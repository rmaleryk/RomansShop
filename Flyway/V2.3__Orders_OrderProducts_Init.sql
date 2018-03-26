CREATE TABLE "orders" (
    "id" uuid NOT NULL PRIMARY KEY,
	"userId" uuid NULL REFERENCES users(id),
    "customerName" varchar(30) NOT NULL,
	"customerEmail" varchar(255) NOT NULL,
	"address" varchar(255) NOT NULL,
	"price" decimal NOT NULL,
	"status" smallint NOT NULL
)
WITH (
  OIDS=FALSE
);
ALTER TABLE orders
  OWNER TO postgres;

CREATE TABLE "order_products" (
    "id" uuid NOT NULL PRIMARY KEY,
	"orderId" uuid NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
	"productId" uuid NOT NULL REFERENCES products(id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE order_products
  OWNER TO postgres;
