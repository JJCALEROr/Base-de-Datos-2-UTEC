CREATE DATABASE GestorInventario;
GO

USE GestorInventario;
GO

-- ============================================================
--  TABLAS PRINCIPALES
-- ============================================================

CREATE TABLE Categoria (
    id_categoria   INT IDENTITY(1,1) PRIMARY KEY,
    nombre_categoria VARCHAR(50) NOT NULL,
    descripcion    VARCHAR(150)
);

CREATE TABLE Proveedor (
    id_proveedor   INT IDENTITY(1,1) PRIMARY KEY,
    nombre_proveedor VARCHAR(100) NOT NULL,
    telefono       VARCHAR(20),
    correo         VARCHAR(100),
    direccion      VARCHAR(150),
    empresa        VARCHAR(100)
);

CREATE TABLE Producto (
    id_producto    INT IDENTITY(1,1) PRIMARY KEY,
    nombre_producto VARCHAR(100) NOT NULL,
    descripcion    VARCHAR(200),
    id_categoria   INT NOT NULL,          -- FK → Categoria
    id_proveedor   INT NOT NULL,          -- FK → Proveedor
    unidad_medida  VARCHAR(20),
    stock_actual   INT DEFAULT 0,
    stock_minimo   INT DEFAULT 0,
    fecha_caducidad DATE,
    precio_compra  DECIMAL(10,2) NOT NULL,
    precio_venta   DECIMAL(10,2) NOT NULL,
    estado         VARCHAR(20) DEFAULT 'Activo',

    FOREIGN KEY (id_categoria) REFERENCES Categoria(id_categoria),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedor(id_proveedor)
);

CREATE TABLE Usuario (
    id_usuario  INT IDENTITY(1,1) PRIMARY KEY,
    nombre      VARCHAR(50) NOT NULL,
    apellido    VARCHAR(50) NOT NULL,
    usuario     VARCHAR(50) UNIQUE NOT NULL,
    contrasena  VARCHAR(100) NOT NULL,
    rol         VARCHAR(20) NOT NULL,
    estado      VARCHAR(20) DEFAULT 'Activo'
);

CREATE TABLE Compra (
    id_compra    INT IDENTITY(1,1) PRIMARY KEY,
    fecha_compra DATE NOT NULL,
    total_compra DECIMAL(10,2),
    id_usuario   INT NOT NULL,

    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

CREATE TABLE DetalleCompra (
    id_detalle_compra INT IDENTITY(1,1) PRIMARY KEY,
    id_compra         INT NOT NULL,
    id_producto       INT NOT NULL,
    cantidad          INT NOT NULL,
    precio_unitario   DECIMAL(10,2) NOT NULL,
    subtotal          DECIMAL(10,2),

    FOREIGN KEY (id_compra)   REFERENCES Compra(id_compra),
    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto)
);

CREATE TABLE Venta (
    id_venta    INT IDENTITY(1,1) PRIMARY KEY,
    fecha_venta DATE NOT NULL,
    total_venta DECIMAL(10,2),
    id_usuario  INT NOT NULL,

    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

CREATE TABLE DetalleVenta (
    id_detalle_venta INT IDENTITY(1,1) PRIMARY KEY,
    id_venta         INT NOT NULL,
    id_producto      INT NOT NULL,
    cantidad         INT NOT NULL,
    precio_unitario  DECIMAL(10,2) NOT NULL,
    subtotal         DECIMAL(10,2),

    FOREIGN KEY (id_venta)    REFERENCES Venta(id_venta),
    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto)
);

CREATE TABLE MovimientoInventario (
    id_movimiento    INT IDENTITY(1,1) PRIMARY KEY,
    id_producto      INT NOT NULL,
    tipo_movimiento  VARCHAR(20) NOT NULL,
    cantidad         INT NOT NULL,
    fecha_movimiento DATE NOT NULL,
    motivo           VARCHAR(100),
    referencia       VARCHAR(100),

    FOREIGN KEY (id_producto) REFERENCES Producto(id_producto)
);

CREATE TABLE Reporte (
    id_reporte       INT IDENTITY(1,1) PRIMARY KEY,
    tipo_reporte     VARCHAR(50),
    fecha_inicio     DATE,
    fecha_fin        DATE,
    fecha_generacion DATE,
    id_usuario       INT,

    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

-- ============================================================
--  CONSTRAINTS
-- ============================================================

ALTER TABLE Producto
ADD CONSTRAINT chk_precio_compra CHECK (precio_compra > 0);

ALTER TABLE Producto
ADD CONSTRAINT chk_precio_venta CHECK (precio_venta > 0);

ALTER TABLE DetalleCompra
ADD CONSTRAINT chk_cantidad_compra CHECK (cantidad > 0);

ALTER TABLE DetalleVenta
ADD CONSTRAINT chk_cantidad_venta CHECK (cantidad > 0);

-- ============================================================
--  INSERCIÓN DE DATOS
-- ============================================================

-- ------------------------------------------------------------
-- 1. PROVEEDOR
-- ------------------------------------------------------------
INSERT INTO Proveedor (nombre_proveedor, telefono, correo, direccion, empresa) VALUES
('Carlos Mendoza',    '2222-1111', 'cmendoza@distrib.com',   'Av. Los Héroes #45, San Salvador',  'Distribuidora El Mercado'),
('Ana Flores',        '2233-4455', 'aflores@frutasverde.com','Calle Principal #12, Santa Ana',     'Frutas Verdes S.A.'),
('Roberto Chávez',    '2244-5566', 'rchavez@lacteos.com',   'Blvd. Constitución #78, Soyapango', 'Lácteos del Valle'),
('María López',       '2255-6677', 'mlopez@bebidas.com',    'Calle Delgado #33, San Miguel',      'Bebidas Nacionales'),
('Jorge Hernández',   '2266-7788', 'jhernandez@carnes.com', 'Av. Independencia #9, Usulután',    'Carnes Selectas');
GO

-- ------------------------------------------------------------
-- 2. CATEGORIA
-- ------------------------------------------------------------
INSERT INTO Categoria (nombre_categoria, descripcion) VALUES
('Bebidas',    'Jugos, aguas, refrescos y bebidas en general'),
('Frutas',     'Frutas frescas de temporada y tropicales'),
('Verduras',   'Hortalizas y verduras frescas'),
('Lácteos',    'Leche, queso, crema y derivados'),
('Carnes',     'Carnes rojas, aves y embutidos'),
('Abarrotes',  'Granos, enlatados y productos de canasta básica'),
('Panadería',  'Pan dulce, pan francés y repostería');
GO

-- ------------------------------------------------------------
-- 3. PRODUCTO  (referencia id_categoria e id_proveedor ya insertados)
-- ------------------------------------------------------------
INSERT INTO Producto (nombre_producto, descripcion, id_categoria, id_proveedor, unidad_medida, stock_actual, stock_minimo, fecha_caducidad, precio_compra, precio_venta, estado) VALUES
-- Bebidas  (id_categoria=1, proveedor María López id=4)
('Agua Purificada 1L',   'Agua purificada en botella de 1 litro',          1, 4, 'Unidad',   100, 20, '2026-12-31', 0.35, 0.75, 'Activo'),
('Jugo de Naranja 500ml','Jugo natural de naranja sin conservantes',        1, 4, 'Unidad',    60, 10, '2026-06-30', 0.80, 1.50, 'Activo'),
('Refresco Cola 355ml',  'Bebida gaseosa sabor cola en lata',               1, 4, 'Unidad',   150, 30, '2027-01-15', 0.60, 1.25, 'Activo'),
-- Frutas   (id_categoria=2, proveedor Ana Flores id=2)
('Mango Tommy',          'Mango de primera calidad, dulce y jugoso',        2, 2, 'Libra',     80, 15, '2026-04-10', 0.50, 1.00, 'Activo'),
('Banano',               'Racimo de bananos maduros',                       2, 2, 'Racimo',    50, 10, '2026-03-25', 0.75, 1.50, 'Activo'),
('Sandía',               'Sandía entera de aproximadamente 5 kg',           2, 2, 'Unidad',    30,  5, '2026-03-30', 2.00, 4.00, 'Activo'),
-- Verduras (id_categoria=3, proveedor Carlos Mendoza id=1)
('Tomate',               'Tomate rojo fresco por libra',                    3, 1, 'Libra',    120, 25, '2026-03-28', 0.40, 0.90, 'Activo'),
('Chile Verde',          'Chile verde fresco por libra',                    3, 1, 'Libra',     90, 20, '2026-03-27', 0.35, 0.80, 'Activo'),
('Cebolla Blanca',       'Cebolla blanca fresca por libra',                 3, 1, 'Libra',    100, 20, '2026-04-05', 0.30, 0.70, 'Activo'),
-- Lácteos  (id_categoria=4, proveedor Roberto Chávez id=3)
('Leche Entera 1L',      'Leche entera pasteurizada en bolsa de 1 litro',  4, 3, 'Unidad',    70, 15, '2026-03-22', 0.90, 1.50, 'Activo'),
('Queso Duro Blando',    'Queso duro blando por libra',                     4, 3, 'Libra',     40, 10, '2026-04-15', 1.80, 3.50, 'Activo'),
('Crema Ácida 250ml',    'Crema ácida en bolsa de 250 ml',                  4, 3, 'Unidad',    55, 10, '2026-03-29', 0.70, 1.25, 'Activo'),
-- Carnes   (id_categoria=5, proveedor Jorge Hernández id=5)
('Pechuga de Pollo',     'Pechuga de pollo fresca sin hueso por libra',    5, 5, 'Libra',     60, 10, '2026-03-21', 1.50, 3.00, 'Activo'),
('Carne Molida',         'Carne molida de res por libra',                   5, 5, 'Libra',     50, 10, '2026-03-20', 2.00, 4.00, 'Activo'),
-- Abarrotes (id_categoria=6, proveedor Carlos Mendoza id=1)
('Frijoles Rojos 1lb',   'Frijoles rojos secos de primera calidad',        6, 1, 'Libra',    200, 50, '2027-06-30', 0.60, 1.20, 'Activo'),
('Arroz Blanco 1lb',     'Arroz blanco grano largo por libra',              6, 1, 'Libra',    250, 50, '2027-12-31', 0.55, 1.10, 'Activo');
GO

-- ------------------------------------------------------------
-- 4. USUARIO
-- ------------------------------------------------------------
INSERT INTO Usuario (nombre, apellido, usuario, contrasena, rol, estado) VALUES
('Admin',    'Sistema',   'admin',     'Admin@2025',    'Administrador', 'Activo'),
('Laura',    'Rivas',     'lrivas',    'Laura@2025',    'Vendedor',      'Activo'),
('Pedro',    'Castillo',  'pcastillo', 'Pedro@2025',    'Vendedor',      'Activo'),
('Sofía',    'Martínez',  'smartinez', 'Sofia@2025',    'Bodeguero',     'Activo'),
('Diego',    'Guzmán',    'dguzman',   'Diego@2025',    'Supervisor',    'Activo');
GO

-- ------------------------------------------------------------
-- 5. COMPRA
-- ------------------------------------------------------------
INSERT INTO Compra (fecha_compra, total_compra, id_usuario) VALUES
('2026-03-01', 250.00, 1),
('2026-03-05', 180.50, 5),
('2026-03-10', 320.75, 1);
GO

-- ------------------------------------------------------------
-- 6. DETALLE COMPRA
-- ------------------------------------------------------------
INSERT INTO DetalleCompra (id_compra, id_producto, cantidad, precio_unitario, subtotal) VALUES
(1,  1, 50, 0.35,  17.50),
(1,  4, 30, 0.50,  15.00),
(1, 15, 80, 0.60,  48.00),
(1, 16, 80, 0.55,  44.00),
(2, 10, 30, 0.90,  27.00),
(2, 11, 20, 1.80,  36.00),
(2, 13, 25, 1.50,  37.50),
(3,  3, 60, 0.60,  36.00),
(3,  7, 50, 0.40,  20.00),
(3, 14, 40, 2.00,  80.00);
GO

-- ------------------------------------------------------------
-- 7. VENTA
-- ------------------------------------------------------------
INSERT INTO Venta (fecha_venta, total_venta, id_usuario) VALUES
('2026-03-02',  45.75, 2),
('2026-03-06',  82.00, 3),
('2026-03-11', 120.50, 2);
GO

-- ------------------------------------------------------------
-- 8. DETALLE VENTA
-- ------------------------------------------------------------
INSERT INTO DetalleVenta (id_venta, id_producto, cantidad, precio_unitario, subtotal) VALUES
(1,  1, 10, 0.75,  7.50),
(1,  4,  8, 1.00,  8.00),
(1, 10,  5, 1.50,  7.50),
(2,  3, 12, 1.25, 15.00),
(2, 15, 10, 1.20, 12.00),
(2, 16, 10, 1.10, 11.00),
(3, 13,  6, 3.00, 18.00),
(3, 14,  5, 4.00, 20.00),
(3,  7, 15, 0.90, 13.50);
GO

-- ------------------------------------------------------------
-- 9. MOVIMIENTO INVENTARIO
-- ------------------------------------------------------------
INSERT INTO MovimientoInventario (id_producto, tipo_movimiento, cantidad, fecha_movimiento, motivo, referencia) VALUES
( 1, 'Entrada',  50, '2026-03-01', 'Compra a proveedor',   'COMPRA-001'),
( 4, 'Entrada',  30, '2026-03-01', 'Compra a proveedor',   'COMPRA-001'),
(15, 'Entrada',  80, '2026-03-01', 'Compra a proveedor',   'COMPRA-001'),
( 1, 'Salida',   10, '2026-03-02', 'Venta al cliente',     'VENTA-001'),
( 4, 'Salida',    8, '2026-03-02', 'Venta al cliente',     'VENTA-001'),
(10, 'Entrada',  30, '2026-03-05', 'Compra a proveedor',   'COMPRA-002'),
(13, 'Entrada',  25, '2026-03-05', 'Compra a proveedor',   'COMPRA-002'),
( 3, 'Salida',   12, '2026-03-06', 'Venta al cliente',     'VENTA-002'),
(13, 'Salida',    6, '2026-03-11', 'Venta al cliente',     'VENTA-003'),
( 7, 'Ajuste',    5, '2026-03-12', 'Ajuste por inventario','AJUSTE-001');
GO

-- ------------------------------------------------------------
-- 10. REPORTE
-- ------------------------------------------------------------
INSERT INTO Reporte (tipo_reporte, fecha_inicio, fecha_fin, fecha_generacion, id_usuario) VALUES
('Ventas Diarias',       '2026-03-01', '2026-03-05',  '2026-03-05',  1),
('Inventario Bajo',      '2026-03-01', '2026-03-10',  '2026-03-10',  5),
('Compras del Mes',      '2026-03-01', '2026-03-11',  '2026-03-11',  1),
('Movimientos Generales','2026-03-01', '2026-03-12',  '2026-03-12',  5);
GO