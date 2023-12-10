CREATE TABLE "Customer" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "first_name" varchar,
  "last_name" varchar,
  "email" varchar,
  "password" varchar,
  "phone_number" varchar,
  "birth_date" date,
  "gender" bool,
  "customer_image" varchar,
  "is_deleted" bool
);

CREATE TABLE "Address_customer" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "customer_id" int,
  "address_customer_name" varchar,
  "phone_number" varchar,
  "city" varchar,
  "district" varchar,
  "sub_district" varchar,
  "address" varchar,
  "is_deleted" bool
);

CREATE TABLE "Employee" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "first_name" varchar,
  "last_name" varchar,
  "email" varchar,
  "password" varchar,
  "phone_number" varchar,
  "birth_date" date,
  "gender" bool,
  "employee_image" varchar,
  "address" varchar,
  "hire_date" date,
  "job_title_id" int,
  "salary" decimal,
  "is_deleted" bool
);

CREATE TABLE "Job_Title" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "job_title_name" varchar,
  "is_deleted" bool
);

CREATE TABLE "Payment_method" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "payment_method_name" varchar,
  "is_deleted" bool
);

CREATE TABLE "Order" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "customer_id" int,
  "employee_id" int,
  "order_date_time" timestamp,
  "address_customer_id" int,
  "total_price" decimal,
  "shipping_cost" decimal,
  "payment_method_id" int,
  "order_status" varchar,
  "is_deleted" bool
);

CREATE TABLE "OrderDetail" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "order_id" int,
  "product_id" int,
  "quantity" int,
  "unit_price" decimal,
  "is_deleted" bool
);

CREATE TABLE "Product" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "product_name" varchar,
  "description" varchar,
  "rating" decimal,
  "unit_price" decimal,
  "stock_quantity" int,
  "product_image" varchar,
  "product_category_id" int,
  "provider_id" int,
  "is_deleted" bool
);

CREATE TABLE "Product_category" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "product_category_name" varchar,
  "is_deleted" bool
);

CREATE TABLE "Provider" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "provider_name" varchar,
  "is_deleted" bool
);

CREATE TABLE "Comment" (
  "id" INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
  "customer_id" int,
  "product_id" int,
  "content" varchar,
  "rating" int,
  "comment_image" varchar,
  "is_deleted" bool
);

ALTER TABLE "Address_customer" ADD FOREIGN KEY ("customer_id") REFERENCES "Customer" ("id");

ALTER TABLE "Employee" ADD FOREIGN KEY ("job_title_id") REFERENCES "Job_Title" ("id");

ALTER TABLE "Order" ADD FOREIGN KEY ("employee_id") REFERENCES "Employee" ("id");

ALTER TABLE "Order" ADD FOREIGN KEY ("address_customer_id") REFERENCES "Address_customer" ("id");

ALTER TABLE "OrderDetail" ADD FOREIGN KEY ("order_id") REFERENCES "Order" ("id");

ALTER TABLE "OrderDetail" ADD FOREIGN KEY ("product_id") REFERENCES "Product" ("id");

ALTER TABLE "Order" ADD FOREIGN KEY ("customer_id") REFERENCES "Customer" ("id");

ALTER TABLE "Order" ADD FOREIGN KEY ("payment_method_id") REFERENCES "Payment_method" ("id");

ALTER TABLE "Product" ADD FOREIGN KEY ("product_category_id") REFERENCES "Product_category" ("id");

ALTER TABLE "Product" ADD FOREIGN KEY ("provider_id") REFERENCES "Provider" ("id");

ALTER TABLE "Customer" ADD FOREIGN KEY ("id") REFERENCES "Comment" ("customer_id");

ALTER TABLE "Product" ADD FOREIGN KEY ("id") REFERENCES "Comment" ("product_id");
