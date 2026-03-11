CREATE DATABASE GestorInventario;
GO

USE GestorInventario;
GO

CREATE TABLE Usuario (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    usuario VARCHAR(50) UNIQUE NOT NULL,
    contrasena VARCHAR(100) NOT NULL,
    rol VARCHAR(20) NOT NULL,
    estado VARCHAR(20) DEFAULT 'Activo'
);

CREATE TABLE Proveedor (
    id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    nombre_proveedor VARCHAR(100) NOT NULL,
    telefono VARCHAR(20),
    correo VARCHAR(100),
    direccion VARCHAR(150),
    empresa VARCHAR(100)
);

CREATE TABLE Producto (
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    nombre_producto VARCHAR(100) NOT NULL,
    descripcion VARCHAR(200),
    categoria VARCHAR(50),
    unidad_medida VARCHAR(20),
    stock_actual INT DEFAULT 0,
    stock_minimo INT DEFAULT 0,
    fecha_caducidad DATE,
    precio_compra DECIMAL(10,2) NOT NULL,
    precio_venta DECIMAL(10,2) NOT NULL,
    estado VARCHAR(20) DEFAULT 'Activo'
);

CREATE TABLE Compra (
    id_compra INT IDENTITY(1,1) PRIMARY KEY,
    fecha_compra DATE NOT NULL,
    total_compra DECIMAL(10,2),
    id_proveedor INT NOT NULL,
    id_usuario INT NOT NULL,
    
    FOREIGN KEY (id_proveedor) REFERENCES Proveedor(id_proveedor),
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

CREATE TABLE DetalleCompra (
    id_detalle_compra INT IDENTITY(1,1) PRIMARY KEY,
    id_compra INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2),

    FOREIGN KEY (id_compra) REFERENCES Compra(id_compra),
    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto)
);

CREATE TABLE Venta (
    id_venta INT IDENTITY(1,1) PRIMARY KEY,
    fecha_venta DATE NOT NULL,
    total_venta DECIMAL(10,2),
    id_usuario INT NOT NULL,

    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

CREATE TABLE DetalleVenta (
    id_detalle_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2),

    FOREIGN KEY (id_venta) REFERENCES Venta(id_venta),
    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto)
);

CREATE TABLE MovimientoInventario (
    id_movimiento INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT NOT NULL,
    tipo_movimiento VARCHAR(20) NOT NULL,
    cantidad INT NOT NULL,
    fecha_movimiento DATE NOT NULL,
    motivo VARCHAR(100),
    referencia VARCHAR(100),

    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto)
);

CREATE TABLE Reporte (
    id_reporte INT IDENTITY(1,1) PRIMARY KEY,
    tipo_reporte VARCHAR(50),
    fecha_inicio DATE,
    fecha_fin DATE,
    fecha_generacion DATE,
    id_usuario INT,

    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);


ALTER TABLE Producto
ADD CONSTRAINT chk_precio_compra
CHECK (precio_compra > 0);

ALTER TABLE Producto
ADD CONSTRAINT chk_precio_venta
CHECK (precio_venta > 0);

ALTER TABLE DetalleCompra
ADD CONSTRAINT chk_cantidad_compra
CHECK (cantidad > 0);

ALTER TABLE DetalleVenta
ADD CONSTRAINT chk_cantidad_venta
CHECK (cantidad > 0);