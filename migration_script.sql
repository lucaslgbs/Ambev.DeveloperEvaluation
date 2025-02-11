CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Users" (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Username" character varying(50) NOT NULL,
    "Password" character varying(100) NOT NULL,
    "Phone" character varying(20) NOT NULL,
    "Email" character varying(100) NOT NULL,
    "Status" character varying(20) NOT NULL,
    "Role" character varying(20) NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241014011203_InitialMigrations', '8.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE "Users" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE "Users" ADD "UpdatedAt" timestamp with time zone;

CREATE TABLE "Orders" (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "OrderNumber" character varying(30) NOT NULL,
    "OrderDate" timestamp with time zone NOT NULL,
    "Customer" character varying(100) NOT NULL,
    "Branch" character varying(100) NOT NULL,
    "IsCancelled" boolean NOT NULL,
    CONSTRAINT "PK_Orders" PRIMARY KEY ("Id")
);

CREATE TABLE "OrderItems" (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "ProductCode" character varying(50) NOT NULL,
    "ProductDescription" character varying(50) NOT NULL,
    "Quantity" integer NOT NULL,
    "UnitPrice" numeric NOT NULL,
    "Discount" numeric NOT NULL,
    "OrderId" uuid NOT NULL,
    "IsCancelled" boolean NOT NULL,
    CONSTRAINT "PK_OrderItems" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250211165814_AddOrdersTable', '8.0.10');

COMMIT;

