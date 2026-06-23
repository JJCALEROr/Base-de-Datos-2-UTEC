CREATE DATABASE GestorInventario;
GO

USE GestorInventario;
GO

-- ============================================================
--  TABLAS CATÁLOGO (sin dependencias)
-- ============================================================

CREATE TABLE Categoria (
    id_categoria     INT IDENTITY(1,1) PRIMARY KEY,
    nombre_categoria VARCHAR(50)  NOT NULL UNIQUE,
    descripcion      VARCHAR(150)
);

CREATE TABLE Proveedor (
    id_proveedor     INT IDENTITY(1,1) PRIMARY KEY,
    nombre_proveedor VARCHAR(100) NOT NULL,
    telefono         VARCHAR(20)  UNIQUE,
    correo           VARCHAR(100) UNIQUE,
    direccion        VARCHAR(150),
    empresa          VARCHAR(100)
);

CREATE TABLE Medidas (
    id_medida     INT IDENTITY(1,1) PRIMARY KEY,
    unidad_medida VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE Roles (
    id_rol     INT IDENTITY(1,1) PRIMARY KEY,
    nombre_rol VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE MotivoMovimiento (
    id_motivo INT IDENTITY(1,1) PRIMARY KEY,
    motivo    VARCHAR(100) NOT NULL UNIQUE
);

-- ============================================================
--  TABLAS PRINCIPALES
-- ============================================================

CREATE TABLE Producto (
    id_producto     INT IDENTITY(1,1) PRIMARY KEY,
    nombre_producto VARCHAR(100)   NOT NULL UNIQUE,
    descripcion     VARCHAR(200),
    id_categoria    INT            NOT NULL,
    id_proveedor    INT            NOT NULL,
    id_medida       INT            NOT NULL,
    stock_actual    INT            DEFAULT 0,
    stock_minimo    INT            DEFAULT 0,
    fecha_caducidad DATE,
    precio_compra   DECIMAL(10,2)  NOT NULL,
    precio_venta    DECIMAL(10,2)  NOT NULL,
    estado          VARCHAR(20)    NOT NULL DEFAULT 'Activo',

    FOREIGN KEY (id_categoria) REFERENCES Categoria(id_categoria),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedor(id_proveedor),
    FOREIGN KEY (id_medida)    REFERENCES Medidas(id_medida)
);

CREATE TABLE Usuario (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre     VARCHAR(50)  NOT NULL,
    apellido   VARCHAR(50)  NOT NULL,
    correo     VARCHAR(100) NOT NULL UNIQUE,   -- corregido a VARCHAR(100)
    contrasena VARCHAR(100) NOT NULL,
    id_rol     INT          NOT NULL,
    estado     VARCHAR(20)  NOT NULL DEFAULT 'Activo',

    FOREIGN KEY (id_rol) REFERENCES Roles(id_rol)
);

CREATE TABLE Compra (
    id_compra    INT IDENTITY(1,1) PRIMARY KEY,
    fecha_compra DATE          NOT NULL,
    total_compra DECIMAL(10,2),
    id_usuario   INT           NOT NULL,
    estado       VARCHAR(20)   NOT NULL DEFAULT 'Pendiente',  -- nuevo

    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

CREATE TABLE DetalleCompra (
    id_detalle_compra INT IDENTITY(1,1) PRIMARY KEY,
    id_compra         INT           NOT NULL,
    id_producto       INT           NOT NULL,
    cantidad          INT           NOT NULL,
    precio_unitario   DECIMAL(10,2) NOT NULL,
    subtotal          DECIMAL(10,2),
    comentario        VARCHAR(200)  NULL,

    FOREIGN KEY (id_compra)   REFERENCES Compra(id_compra),
    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto)
);

CREATE TABLE Venta (
    id_venta    INT IDENTITY(1,1) PRIMARY KEY,
    fecha_venta DATE          NOT NULL,
    total_venta DECIMAL(10,2),
    id_usuario  INT           NOT NULL,
    estado      VARCHAR(20)   NOT NULL DEFAULT 'Pendiente',   -- nuevo

    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

CREATE TABLE DetalleVenta (
    id_detalle_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_venta         INT           NOT NULL,
    id_producto      INT           NOT NULL,
    cantidad         INT           NOT NULL,
    precio_unitario  DECIMAL(10,2) NOT NULL,
    subtotal         DECIMAL(10,2),

    FOREIGN KEY (id_venta)    REFERENCES Venta(id_venta),
    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto)
);

CREATE TABLE MovimientoInventario (
    id_movimiento    INT IDENTITY(1,1) PRIMARY KEY,
    id_producto      INT          NOT NULL,
    tipo_movimiento  VARCHAR(20)  NOT NULL,
    cantidad         INT          NOT NULL,
    fecha_movimiento DATE         NOT NULL,
    id_motivo        INT          NOT NULL,
    comentario       VARCHAR(200),

    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto),
    FOREIGN KEY (id_motivo)   REFERENCES MotivoMovimiento(id_motivo)
);

-- ============================================================
--  PRODUCTO
-- ============================================================

-- Precios siempre positivos
ALTER TABLE Producto
ADD CONSTRAINT chk_precio_compra
    CHECK (precio_compra > 0);

ALTER TABLE Producto
ADD CONSTRAINT chk_precio_venta
    CHECK (precio_venta > 0);

-- Precio de venta debe ser >= al de compra
ALTER TABLE Producto
ADD CONSTRAINT chk_precio_venta_mayor
    CHECK (precio_venta >= precio_compra);

-- Stock nunca negativo
ALTER TABLE Producto
ADD CONSTRAINT chk_stock_actual
    CHECK (stock_actual >= 0);

ALTER TABLE Producto
ADD CONSTRAINT chk_stock_minimo
    CHECK (stock_minimo >= 0);

-- Estado controlado
ALTER TABLE Producto
ADD CONSTRAINT chk_estado_producto
    CHECK (estado IN ('Activo', 'Inactivo'));


-- ============================================================
--  USUARIO
-- ============================================================

-- Estado controlado
ALTER TABLE Usuario
ADD CONSTRAINT chk_estado_usuario
    CHECK (estado IN ('Activo', 'Inactivo'));

-- Contraseña mínimo 8 caracteres
ALTER TABLE Usuario
ADD CONSTRAINT chk_contrasena_no_vacia
    CHECK (LEN(contrasena) >= 8);


-- ============================================================
--  COMPRA
-- ============================================================

-- Total positivo
ALTER TABLE Compra
ADD CONSTRAINT chk_total_compra
    CHECK (total_compra > 0);

-- Fecha no futura
ALTER TABLE Compra
ADD CONSTRAINT chk_fecha_compra
    CHECK (fecha_compra <= CAST(GETDATE() AS DATE));

-- Estado controlado (campo nuevo agregado en el rediseño)
ALTER TABLE Compra
ADD CONSTRAINT chk_estado_compra
    CHECK (estado IN ('Pendiente', 'Completada', 'Anulada'));


-- ============================================================
--  DETALLE COMPRA
-- ============================================================

-- Cantidad positiva
ALTER TABLE DetalleCompra
ADD CONSTRAINT chk_cantidad_compra
    CHECK (cantidad > 0);

-- Precio unitario positivo
ALTER TABLE DetalleCompra
ADD CONSTRAINT chk_precio_unitario_compra
    CHECK (precio_unitario > 0);

-- Subtotal positivo
ALTER TABLE DetalleCompra
ADD CONSTRAINT chk_subtotal_compra
    CHECK (subtotal > 0);


-- ============================================================
--  VENTA
-- ============================================================

-- Total positivo
ALTER TABLE Venta
ADD CONSTRAINT chk_total_venta
    CHECK (total_venta > 0);

-- Fecha no futura
ALTER TABLE Venta
ADD CONSTRAINT chk_fecha_venta
    CHECK (fecha_venta <= CAST(GETDATE() AS DATE));

-- Estado controlado (campo nuevo agregado en el rediseño)
ALTER TABLE Venta
ADD CONSTRAINT chk_estado_venta
    CHECK (estado IN ('Pendiente', 'Completada', 'Anulada'));


-- ============================================================
--  DETALLE VENTA
-- ============================================================

-- Cantidad positiva
ALTER TABLE DetalleVenta
ADD CONSTRAINT chk_cantidad_venta
    CHECK (cantidad > 0);

-- Precio unitario positivo
ALTER TABLE DetalleVenta
ADD CONSTRAINT chk_precio_unitario_venta
    CHECK (precio_unitario > 0);

-- Subtotal positivo
ALTER TABLE DetalleVenta
ADD CONSTRAINT chk_subtotal_venta
    CHECK (subtotal > 0);


-- ============================================================
--  MOVIMIENTO INVENTARIO
-- ============================================================
-- Solo dos tipos válidos
ALTER TABLE MovimientoInventario
ADD CONSTRAINT chk_tipo_movimiento
    CHECK (tipo_movimiento IN ('Entrada', 'Salida'));

-- Cantidad siempre positiva
ALTER TABLE MovimientoInventario
ADD CONSTRAINT chk_cantidad_movimiento
    CHECK (cantidad > 0);

-- Fecha no futura
ALTER TABLE MovimientoInventario
ADD CONSTRAINT chk_fecha_movimiento
    CHECK (fecha_movimiento <= CAST(GETDATE() AS DATE));


